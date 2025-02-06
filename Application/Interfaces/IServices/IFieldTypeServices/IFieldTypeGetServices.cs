using Application.DTOS.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IFieldTypeServices
{
    public interface IFieldTypeGetServices
    {
        public Task<List<FieldTypeResponse>> GetAll();
        public Task<FieldTypeResponse> GetFieldTypeById(int id);
    }
}
