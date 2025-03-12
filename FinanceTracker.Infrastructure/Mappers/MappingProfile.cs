using AutoMapper;
using FinanceTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Map BaseModel-derived classes to DTOs
            CreateMap<PaymentMethod, PaymentMethodDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.MethodName, opt => opt.MapFrom(src => src.MethodName))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted));

            // Map Create DTO to Model
            CreateMap<PaymentMethodCreateDto, PaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Auto-incremented by DB
                .ForMember(dest => dest.CreatedAt, opt => opt.PreCondition(src => !src.CreatedAt.HasValue))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()); // Handled during updates

            // Map Update DTO to Model
            CreateMap<PaymentMethodUpdateDto, PaymentMethod>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // ID comes from route
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()) // Never updated
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Never updated
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore()) // Explicitly set in controller
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                .ForMember(dest => dest.MethodName, opt => opt.MapFrom(src => src.MethodName))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy));
        }


    }
}
