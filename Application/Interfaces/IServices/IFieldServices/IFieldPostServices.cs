using Application.DTOS.Request;
using Application.DTOS.Responses;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.IServices.IFieldServices
{
    public interface IFieldPostServices
    {
        Task<FieldResponse> CreateField(FieldRequest request);
    }
}
