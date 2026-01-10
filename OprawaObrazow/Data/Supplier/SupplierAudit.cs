using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Supplier;

[Table("suppliers", Schema = "oprawa_audit")]
public class SupplierAudit : BaseAudit
{
}