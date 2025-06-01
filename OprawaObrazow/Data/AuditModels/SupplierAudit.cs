using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("suppliers", Schema = "oprawa_audit")]
public class SupplierAudit : BaseAudit
{
}