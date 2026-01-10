using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Color;

[Table("colors", Schema = "oprawa_audit")]
public class ColorAudit : BaseAudit
{
    
}