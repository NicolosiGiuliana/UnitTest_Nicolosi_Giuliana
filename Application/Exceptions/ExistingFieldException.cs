using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Exceptions
{
    public class ExistingFieldException : Exception
    {
        public string Message;

        public ExistingFieldException(string message) : base(message)
        {
            Message = message;
        }
    }
}
