using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Interceptors;

namespace OprawaObrazow.Data;

public class DatabaseContext(
  DbContextOptions<DatabaseContext> options,
  IConfiguration configuration,
  SoftDeleteInterceptor softDeleteInterceptor,
  AuditInterceptor auditInterceptor )
  : DbContext( options )
{
  public virtual DbSet<Frame.Frame> Frames { get; set; }
  public virtual DbSet<FramePiece.FramePiece> FramePieces { get; set; }
  public virtual DbSet<Order.Order> Orders { get; set; }

  public virtual DbSet<Supplier.Supplier> Suppliers { get; set; }
  public virtual DbSet<Color.Color> Colors { get; set; }
  public virtual DbSet<User.User> Users { get; set; }

  protected override void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
    => optionsBuilder
       .UseSqlServer( configuration.GetConnectionString( "OprawaDb" ) )
       .AddInterceptors( auditInterceptor, softDeleteInterceptor );

  protected override void OnModelCreating( ModelBuilder modelBuilder )
  {
    modelBuilder.Entity<Frame.Frame>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "frame_pk" );

      entity.Property( e => e.Code )
            .UseCollation( "SQL_Latin1_General_CP1_CI_AS" );

      entity.HasIndex( e => e.Code )
            .IsUnique()
            .HasDatabaseName( "frames_code_unique" )
            .HasFilter( "[code] IS NOT NULL" );

      entity.HasQueryFilter( e => !e.IsDeleted );

      entity.HasOne( d => d.Supplier ).WithMany( p => p.Frames )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "frame_suppliers_id_fk" );
    } );

    modelBuilder.Entity<FramePiece.FramePiece>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "frame_pieces_pk" );

      entity.Property( e => e.StorageLocation )
            .UseCollation( "SQL_Latin1_General_CP1_CI_AS" );

      entity.HasQueryFilter( e => !e.IsDeleted );

      entity.HasOne( d => d.Frame ).WithMany( p => p.FramePieces )
            .OnDelete( DeleteBehavior.ClientSetNull )
            .HasConstraintName( "frame_pieces_frame_id_fk" );

      entity.HasOne( d => d.Order ).WithMany( p => p.FramePieces ).HasConstraintName( "frame_pieces_orders_id_fk" );
    } );

    modelBuilder.Entity<Order.Order>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "orders_pk" );

      entity.HasQueryFilter( e => !e.IsDeleted );
    } );

    modelBuilder.Entity<Supplier.Supplier>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "suppliers_pk" );

      entity.Property( e => e.Name )
            .UseCollation( "SQL_Latin1_General_CP1_CI_AS" );
    } );

    modelBuilder.Entity<Color.Color>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "colors_pk" );

      entity.Property( e => e.Name )
            .UseCollation( "SQL_Latin1_General_CP1_CI_AS" );
    } );

    modelBuilder.Entity<User.User>( entity =>
    {
      entity.HasKey( e => e.Id ).HasName( "users_pk" );

      entity.Property( e => e.Username )
            .UseCollation( "SQL_Latin1_General_CP1_CI_AS" );

      entity.HasQueryFilter( e => !e.IsDeleted );
    } );
  }
}