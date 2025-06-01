using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("deliveries", Schema = "oprawa_audit")]
public class DeliveryAudit : BaseAudit
{
}