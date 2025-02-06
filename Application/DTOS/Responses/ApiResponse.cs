using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Responses
{
    public class ApiResponse
    {
        public string Message { get; set; }

        public int StatusCode { get; set; }
    }
}
