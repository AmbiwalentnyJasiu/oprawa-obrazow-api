using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("frames", Schema = "oprawa_audit")]
public class FrameAudit : BaseAudit
{
}