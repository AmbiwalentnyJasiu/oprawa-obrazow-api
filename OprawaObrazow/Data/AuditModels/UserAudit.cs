using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("users", Schema = "oprawa_audit")]
public class UserAudit : BaseAudit
{
}