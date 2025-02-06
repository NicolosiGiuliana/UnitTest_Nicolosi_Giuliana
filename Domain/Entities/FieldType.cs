using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class FieldType
    {
        public int FieldTypeID { get; set; }
        public string Description { get; set; }
        public virtual List<Field> Fields { get; set; }
    }
}
