using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.AuditModels;

[Table("frame_pieces", Schema = "oprawa_audit")]
public class FramePieceAudit : BaseAudit
{
}