using AutoMapper;
using FinanceTracker.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceTracker.Infrastructure.Mappers
{
    public class MappingProfileExpenses : Profile
    {
        public MappingProfileExpenses()
        {
            //---------------------------
            // 1) Model → DTO (READ)
            //---------------------------
            CreateMap<expense, ExpenseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.PaymentMethodId, opt => opt.MapFrom(src => src.PaymentMethodId))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                 // Include the child items if you want them returned in ExpenseDto
                .ForMember(dest => dest.ExpenseItems, opt => opt.MapFrom(src => src.ExpenseItems));

            CreateMap<ExpenseItem, ExpenseItemDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.ExpenseId, opt => opt.MapFrom(src => src.ExpenseId))
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.UpdatedBy, opt => opt.MapFrom(src => src.UpdatedBy))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => src.UpdatedAt))
                .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => src.IsDeleted))
                // The "Total" property in ExpenseItemDto is typically computed from "Amount"
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Amount));

            //---------------------------
            // 2) Create DTO → Model
            //---------------------------
            // For when a new Expense is created
            CreateMap<CreateExpenseDto, expense>()
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // PK auto-generated
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date ?? DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            // For creating new ExpenseItems (if you handle them in the same creation flow)
            CreateMap<CreateExpenseDto, ExpenseItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

            //---------------------------
            // 3) Update DTO → Model
            //---------------------------
            // For partial updates to an existing Expense
            CreateMap<UpdateExpenseDto, expense>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())       // PK from DB
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())// Don’t overwrite creation time
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());// Usually set in controller or by DB

            // For partial updates to existing ExpenseItems
            CreateMap<UpdateExpenseDto, ExpenseItem>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ExpenseId, opt => opt.Ignore())// Link remains the same
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        }
    }
}
