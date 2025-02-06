using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IQuery
{
    public interface IFieldQuery
    {
        Task<IEnumerable<Field>> GetFields(string? name, string? sizeoffield, int? type, int? availability, int? offset, int? size);
        Task<Field> GetFieldById(Guid id);
        Task<Field> GetFieldByName(string name);

    }
}
