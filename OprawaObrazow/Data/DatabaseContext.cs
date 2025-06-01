using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.Models;
using OprawaObrazow.Interceptors;

namespace OprawaObrazow.Data;

public partial class DatabaseContext(DbContextOptions<DatabaseContext> options, IConfiguration configuration, SoftDeleteInterceptor softDeleteInterceptor, AuditInterceptor auditInterceptor)
    : DbContext(options)
{
    public virtual DbSet<Client> Clients { get; set; }
    public virtual DbSet<Delivery> Deliveries { get; set; }
    public virtual DbSet<Frame> Frames { get; set; }
    public virtual DbSet<FramePiece> FramePieces { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<Supplier> Suppliers { get; set; }
    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder
            .UseSqlServer(configuration.GetConnectionString("OprawaDb"))
            .AddInterceptors(softDeleteInterceptor, auditInterceptor);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<Delivery>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("deliveries_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Deliveries)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("deliveries_suppliers_fk");
        });

        modelBuilder.Entity<Frame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frame_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(d => d.Supplier).WithMany(p => p.Frames)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frame_suppliers_id_fk");
        });

        modelBuilder.Entity<FramePiece>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frame_pieces_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(d => d.Delivery).WithMany(p => p.FramePieces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frame_pieces_deliveries_fk");

            entity.HasOne(d => d.Frame).WithMany(p => p.FramePieces)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("frame_pieces_frame_id_fk");

            entity.HasOne(d => d.Order).WithMany(p => p.FramePieces).HasConstraintName("frame_pieces_orders_id_fk");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);

            entity.HasOne(d => d.Client).WithMany(p => p.Orders)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orders_clients_id_fk");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");
            
            entity.HasQueryFilter(e => !e.IsDeleted);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
