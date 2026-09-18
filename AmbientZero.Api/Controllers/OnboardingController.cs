using AmbientZero.Data.Repositories;
using AmbientZero.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AmbientZero.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly IOnboardingService _onboardingService;
        public OnboardingController(IOnboardingService onboardingService)
        {
            _onboardingService = onboardingService ?? throw new ArgumentNullException(nameof(onboardingService));
        }

        [HttpGet]
        [Route("ValidateOnboarding")]
        public IActionResult ValidateOnboarding()
        {
            return Ok(_onboardingService.ValidateOnboardingStatus());
        }

        [HttpPatch]
        [Route("SetLanguage")]
        public IActionResult SetLanguage(string language)
        {
            if (_onboardingService.ValidateOnboardingSetup())
            {
                _onboardingService.SaveLanguage(language);
                return Ok();
            }

            return Forbid();
        }

        [HttpPatch]
        [Route("SetUnit")]
        public IActionResult SetUnit(string unitId)
        {
            if (_onboardingService.ValidateOnboardingSetup())
            {
                _onboardingService.SaveUnit(unitId);
                return Ok();
            }

            return Forbid();
        }

        [HttpPatch]
        [Route("SetAdminPassword")]
        public IActionResult SetAdminPassword(string password)
        {
            if (_onboardingService.ValidateOnboardingSetup())
            {
                bool result = _onboardingService.SaveAdminPassword(password.Trim());

                if (result)
                {
                    return Ok();
                }

                return Problem();
            }

            return Forbid();
        }

        [HttpPatch]
        [Route("SetTimeZone")]
        public IActionResult SetTimeZone(string timezone)
        {
            if (_onboardingService.ValidateOnboardingSetup())
            {
                _onboardingService.SaveTimeZone(timezone);
                return Ok();
            }

            return Forbid();
        }

        [HttpPatch]
        [Route("FinishOnboardingSetup")]
        public IActionResult FinishOnboardingSetup()
        {
            if (_onboardingService.ValidateOnboardingSetup())
            {
                bool result = _onboardingService.FinishOnboardingSetup();

                if (result)
                {
                    return Ok();
                }
                else
                {
                    return Problem();
                }
            }

            return Forbid();
        }

        [HttpPatch]
        [Route("FinishOnboardingTour")]
        public IActionResult FinishOnboardingTour()
        {
            bool result = _onboardingService.FinishOnboardingSetup();

            if (result)
            {
                return Ok();
            }
            else
            {
                return Problem();
            }
        }
    }
}
