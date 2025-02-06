using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Request
{
    public class GetFieldsRequest
    {
        public string? Name { get; set; }
        public string? Sizeoffield { get; set; }
        public int? Type { get; set; }
        public int? Availability { get; set; }
        public int? Offset { get; set; }
        public int? Size { get; set; }
    }
}
