using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Order;

[Table("orders", Schema = "oprawa_audit")]
public class OrderAudit : BaseAudit
{
}