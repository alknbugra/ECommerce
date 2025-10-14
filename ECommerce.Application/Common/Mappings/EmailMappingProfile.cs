using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Email mapping profile
/// </summary>
public class EmailMappingProfile : Profile
{
    public EmailMappingProfile()
    {
        CreateMap<EmailTemplate, EmailTemplateDto>();
        CreateMap<EmailLog, EmailDto>()
            .ForMember(dest => dest.PlainTextContent, opt => opt.Ignore())
            .ForMember(dest => dest.Variables, opt => opt.Ignore());
    }
}
