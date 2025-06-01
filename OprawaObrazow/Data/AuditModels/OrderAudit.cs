using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("orders", Schema = "oprawa_audit")]
public class OrderAudit : BaseAudit
{
}