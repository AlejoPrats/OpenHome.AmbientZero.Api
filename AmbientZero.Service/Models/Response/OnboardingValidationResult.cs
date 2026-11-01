using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AmbientZero.Service.Models.Response
{
    public class OnboardingValidationResult
    {
        public bool IsApplicationTourPending { get; set; }
        public bool IsSetupPending { get; set; }
    }
}
