using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.FramePiece;

[Table("frame_pieces", Schema = "oprawa_audit")]
public class FramePieceAudit : BaseAudit
{
}