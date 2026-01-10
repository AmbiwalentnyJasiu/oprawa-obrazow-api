using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.Color;
using OprawaObrazow.Data.Frame;
using OprawaObrazow.Data.FramePiece;
using OprawaObrazow.Data.Order;
using OprawaObrazow.Data.Supplier;
using OprawaObrazow.Data.User;

namespace OprawaObrazow.Data;

public class AuditContext( DbContextOptions<AuditContext> options, IConfiguration configuration ) : DbContext( options )
{
  public virtual DbSet<FrameAudit> Frames { get; set; }
  public virtual DbSet<FramePieceAudit> FramePieces { get; set; }
  public virtual DbSet<OrderAudit> Orders { get; set; }

  public virtual DbSet<SupplierAudit> Suppliers { get; set; }
  public virtual DbSet<ColorAudit> Colors { get; set; }

  public virtual DbSet<UserAudit> Users { get; set; }

  protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    => optionsBuilder.UseSqlServer( configuration.GetConnectionString( "AuditDb" ) );

  protected override void OnModelCreating( ModelBuilder modelBuilder )
  {
    modelBuilder.Entity<FrameAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "frame_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );

    modelBuilder.Entity<FramePieceAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "frame_pieces_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );

    modelBuilder.Entity<OrderAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "orders_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );

    modelBuilder.Entity<SupplierAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "suppliers_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );

    modelBuilder.Entity<UserAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "users_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );

    modelBuilder.Entity<ColorAudit>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "colors_pk" );

      entity.Property( e => e.EntityData ).HasColumnType( "nvarchar(max)" );
    } );
  }
}