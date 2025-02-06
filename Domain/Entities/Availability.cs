using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Availability
    {
        public int AvailabilityID { get; set; }

        public Guid FieldID { get; set; }

        public string DayName { get; set; }

        public TimeSpan OpenHour { get; set; }

        public TimeSpan CloseHour { get; set; }

        public virtual Field FieldNavigator { get; set; }

    }
}
