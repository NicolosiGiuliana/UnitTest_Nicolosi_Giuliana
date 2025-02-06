using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Field
    {
        public Guid FieldID { get; set; }
        public string Name { get; set; }
        public string Size { get; set; }
        public virtual ICollection<Availability> Availabilities { get; set; }
        public int FieldTypeID { get; set; }  // FK 

        public bool IsActive { get; set; }
        public FieldType FieldTypeNavigator { get; set; }
    }
}
