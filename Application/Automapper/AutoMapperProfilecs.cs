using Application.DTOS.Request;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Application.DTOS.Responses;

namespace Application.Automapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<AvailabilityRequest, Availability>()
               .ForMember(dest => dest.OpenHour, opt => opt.MapFrom(src => src.OpenHour))
                .ForMember(dest => dest.CloseHour, opt => opt.MapFrom(src => src.CloseHour))
                .ForMember(dest => dest.DayName, opt => opt.MapFrom(src => src.Day))
                .ReverseMap();


            CreateMap<Availability, AvailabilityResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.AvailabilityID))
                .ForMember(dest => dest.OpenHour, opt => opt.MapFrom(src => src.OpenHour))
                .ForMember(dest => dest.CloseHour, opt => opt.MapFrom(src => src.CloseHour))
                .ForMember(dest => dest.Day, opt => opt.MapFrom(src => src.DayName))
                .ReverseMap();


            CreateMap<FieldRequest, Field>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.FieldTypeID, opt => opt.MapFrom(src => src.FieldType))
                .ReverseMap();
                



            CreateMap<Field, FieldResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FieldID))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.Availabilities))
                .ForMember(dest => dest.FieldType, opt => opt.MapFrom(src => src.FieldTypeNavigator))
                .ReverseMap();

            CreateMap<FieldType, FieldTypeResponse>()
                 .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.FieldTypeID))
                 .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                 .ReverseMap();
        }
    }
}
