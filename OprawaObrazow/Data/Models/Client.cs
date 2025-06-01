using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.Models;

[Table("clients", Schema = "oprawa")]
public partial class Client : ISoftDelete
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("first_name")]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;

    [Column("last_name")]
    [StringLength(50)]
    public string LastName { get; set; } = null!;

    [Column("phone_number")]
    [StringLength(15)]
    public string? PhoneNumber { get; set; }

    [Column("email_address")]
    [StringLength(50)]
    public string? EmailAddress { get; set; }

    [Column("is_deleted")]
    [DefaultValue(false)]
    public bool IsDeleted { get; set; }
    
    [InverseProperty("Client")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}
