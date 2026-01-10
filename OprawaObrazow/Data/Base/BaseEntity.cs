using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OprawaObrazow.Data.Base;

public class BaseEntity
{
  [Key]
  [Column( "id" )]
  public int Id { get; set; }
}