using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IQuery
{
    public interface IFieldTypeQuery
    {
        public Task<List<FieldType>> GetListFieldTypes();

        public Task<FieldType> GetFieldTypeById(int id);
    }
}
