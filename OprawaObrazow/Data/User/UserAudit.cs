using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.User;

[Table("users", Schema = "oprawa_audit")]
public class UserAudit : BaseAudit
{
}