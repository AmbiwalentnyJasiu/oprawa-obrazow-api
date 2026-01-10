using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Frame;

[Table("frames", Schema = "oprawa_audit")]
public class FrameAudit : BaseAudit
{
}