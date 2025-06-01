using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace OprawaObrazow.Data.Models;

[Table("users", Schema = "oprawa")]
[Index("Username", IsUnique = true)]
public partial class User : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [StringLength(50)]
    public string Username { get; set; } = null!;

    [Column("password")]
    [StringLength(500)]
    public string Password { get; set; } = null!;

    [StringLength(50)]
    public string PasswordSalt { get; set; } = null!;
    
    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
}
