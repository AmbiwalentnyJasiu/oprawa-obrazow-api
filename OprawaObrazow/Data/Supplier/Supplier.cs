using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OprawaObrazow.Data.Base;

namespace OprawaObrazow.Data.Supplier;

[Table( "suppliers", Schema = "oprawa" )]
public sealed class Supplier : BaseEntity
{
  [Column( "name" )]
  [StringLength( 200 )]
  public string Name { get; set; } = null!;

  [InverseProperty( "Supplier" )]
  public ICollection<Frame.Frame> Frames { get; set; } = new List<Frame.Frame>();
}