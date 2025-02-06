using Application.DTOS.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Request
{
    public class FieldRequest
    {
        public string Name { get; set; }
        public string Size { get; set; }
        public int FieldType { get; set; }
    }
}
