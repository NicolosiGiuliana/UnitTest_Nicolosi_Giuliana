using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Responses
{
    public class AvailabilityResponse
    {
        public int Id { get; set; }
        public string Day { get; set; }
        public TimeSpan OpenHour { get; set; }
        public TimeSpan CloseHour { get; set; }


    }
}
