using AmbientZero.Service.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace AmbientZero.Service.Services
{
    public class TemporalJsonService<T> : ITemporalJsonService<T> where T : class
    {
        private readonly string _activeFilePath;
        private readonly string _tempFilePath;
        private readonly object _stateLock = new();
        private readonly SemaphoreSlim _writeSemaphore = new(1, 1);
        private readonly ILogger<TemporalJsonService<T>> _logger;

        private T? _currentSnapshot;

        public TemporalJsonService(ILogger<TemporalJsonService<T>> logger)
        {
            _logger = logger;

            const string baseDirectory = "/dev/shm";
            string fileName = typeof(T).Name.ToLower();

            _activeFilePath = Path.Combine(baseDirectory, $"{fileName}.json");
            _tempFilePath = Path.Combine(baseDirectory, $"{fileName}.json.tmp");
        }

        public void UpdateState(T newState)
        {
            lock (_stateLock)
            {
                _currentSnapshot = newState;
            }
        }

        public void SaveBackground()
        {
            T? snapshotToSerialize;

            lock (_stateLock)
            {
                snapshotToSerialize = _currentSnapshot;
            }

            if (snapshotToSerialize == null)
            {
                _logger.LogWarning("SaveBackground called for {TypeName} but current snapshot is null.", typeof(T).Name);
                return;
            }

            // COMPILES SAFELY: Discarding a named async method avoids the Task.Run lambda mess entirely
            _ = ExecuteSaveAsync(snapshotToSerialize);
        }

        // Explicitly isolated async flow so exceptions are caught and logged properly
        private async Task ExecuteSaveAsync(T snapshotToSerialize)
        {
            if (!await _writeSemaphore.WaitAsync(0))
            {
                _logger.LogDebug("Background save for {TypeName} skipped because a previous save operation is still active.", typeof(T).Name);
                return;
            }

            try
            {
                // Yield control immediately back to the main thread callers
                await Task.Yield();

                using (var createStream = File.Create(_tempFilePath))
                {
                    await JsonSerializer.SerializeAsync(createStream, snapshotToSerialize);
                }

                File.Move(_tempFilePath, _activeFilePath, overwrite: true);
                _logger.LogInformation("Successfully updated state for {TypeName} in RAM folder.", typeof(T).Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Background save failed for domain entity {TypeName}.", typeof(T).Name);
            }
            finally
            {
                _writeSemaphore.Release();
            }
        }
    }
}