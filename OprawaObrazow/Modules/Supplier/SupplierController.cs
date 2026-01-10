using Microsoft.AspNetCore.Mvc;
using OprawaObrazow.Modules.Base;
using OprawaObrazow.Modules.Supplier.Dto;

namespace OprawaObrazow.Modules.Supplier;

[ApiController]
[Route( "api-main/[controller]" )]
public class SupplierController( ISupplierService supplierService, ILogger<SupplierController> logger )
  : BaseController<ISupplierService, SupplierEditDto, SupplierViewDto, SupplierViewDto, SupplierFiltersDto>(
    supplierService, logger )
{
  protected override string GetEntityName() => "supplier";
}