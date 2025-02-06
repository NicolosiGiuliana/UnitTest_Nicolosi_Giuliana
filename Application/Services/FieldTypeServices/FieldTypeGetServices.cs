using Application.DTOS.Responses;
using Application.Exceptions;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IFieldTypeServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.FieldTypeServices
{
    public class FieldTypeGetServices : IFieldTypeGetServices
    {
        private readonly IFieldTypeQuery _query;
        private readonly IMapper _mapper;

        public FieldTypeGetServices(IFieldTypeQuery query, IMapper mapper)
        {
            _query = query;
            _mapper = mapper;
        }

        public async Task<List<FieldTypeResponse>> GetAll()
        {
            var result = await _query.GetListFieldTypes();
            List<FieldTypeResponse> list = _mapper.Map<List<FieldTypeResponse>>(result);

            return list;

        }

        public async Task<FieldTypeResponse> GetFieldTypeById(int id)
        {
            try
            {
                FieldType fieldType = await _query.GetFieldTypeById(id);

                if (fieldType == null)
                {
                    throw new NotFoundException("Field Type not found");
                }

                FieldTypeResponse response = _mapper.Map<FieldTypeResponse>(fieldType);

                return response;
            }
            catch (NotFoundException ex)
            {
                throw new NotFoundException(ex.Message);
            }
        }
    }
}
