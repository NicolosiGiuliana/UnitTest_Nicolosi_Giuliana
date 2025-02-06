using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Request
{
    public class AvailabilityRequest
    {
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }
        public string Day { get; set; }
    }
}
