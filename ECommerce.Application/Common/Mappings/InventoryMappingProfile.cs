using AutoMapper;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Common.Mappings;

/// <summary>
/// Stok mapping profile
/// </summary>
public class InventoryMappingProfile : Profile
{
    public InventoryMappingProfile()
    {
        CreateMap<Inventory, InventoryDto>();
        CreateMap<InventoryMovement, InventoryMovementDto>();
        CreateMap<StockAlert, StockAlertDto>();
    }
}
