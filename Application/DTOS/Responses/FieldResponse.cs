using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOS.Responses
{
    public class FieldResponse
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Size { get; set; }

        public ICollection<AvailabilityResponse> Availabilities { get; set; }

        public FieldTypeResponse FieldType { get; set; }

    }
}
