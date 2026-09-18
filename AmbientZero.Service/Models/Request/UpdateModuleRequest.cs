namespace AmbientZero.Service.Models.Request;

public class UpdateModuleRequest
{
    public Guid ModuleId { get; set; }
    public bool IsForcedUpdate { get; set; }
    public bool HasFile { get; set; }
}