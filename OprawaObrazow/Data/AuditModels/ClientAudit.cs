using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("clients", Schema = "oprawa_audit")]
public class ClientAudit : BaseAudit
{
}