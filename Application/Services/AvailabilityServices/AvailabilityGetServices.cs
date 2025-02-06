using Application.DTOS.Request;
using Application.DTOS.Responses;
using Application.Exceptions;
using Application.Interfaces.ICommand;
using Application.Interfaces.IQuery;
using Application.Interfaces.IServices.IAvailabilityServices;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Application.Services.AvailabilityServices
{

    public class AvailabilityGetServices : IAvailabilityGetServices
    {
        private readonly IAvailabilityQuery _queries;
        private readonly IMapper _mapper;


        public AvailabilityGetServices(IAvailabilityQuery queries, IMapper mapper)
        {
            _queries = queries;
            _mapper = mapper;
        }


        public async Task<Availability> GetAvailabilityById(int id)
        {
            var availability = await _queries.GetAvailabilityByID(id);

            if (availability == null)
            {
                throw new NotFoundException("Availability not found");
            }

            availability = await _queries.GetAvailabilityByID(id);


            return availability;
        }

    }
}
