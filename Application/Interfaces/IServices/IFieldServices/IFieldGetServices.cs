using Application.DTOS.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IFieldServices
{
    public interface IFieldGetServices
    {
        Task<FieldResponse> GetFieldById(Guid id);
        Task<List<FieldResponse>> GetAllFields(string? name, string? sizeoffield, int? type, int? availability, int? offset, int? size);
    }
}
