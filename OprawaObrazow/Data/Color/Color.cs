using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Color;

[Table( "colors", Schema = "oprawa" )]
[Index( "Name", IsUnique = true )]
public sealed class Color : BaseEntity
{
  [Column( "name" )]
  [StringLength( 30 )]
  public string Name { get; set; } = null!;

  [InverseProperty( "Color" )]
  public ICollection<Frame.Frame> Frames { get; set; } = new List<Frame.Frame>();
}