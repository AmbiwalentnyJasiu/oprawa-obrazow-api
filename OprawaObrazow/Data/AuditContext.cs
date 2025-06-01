using Microsoft.EntityFrameworkCore;
using OprawaObrazow.Data.AuditModels;

namespace OprawaObrazow.Data;

public class AuditContext(DbContextOptions<AuditContext> options, IConfiguration configuration) : DbContext(options)
{
    public virtual DbSet<ClientAudit> Clients { get; set; }
    public virtual DbSet<DeliveryAudit> Deliveries { get; set; }
    public virtual DbSet<FrameAudit> Frames { get; set; }
    public virtual DbSet<FramePieceAudit> FramePieces { get; set; }
    public virtual DbSet<OrderAudit> Orders { get; set; }
    public virtual DbSet<SupplierAudit> Suppliers { get; set; }
    public virtual DbSet<UserAudit> Users { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(configuration.GetConnectionString("AuditDb"));
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ClientAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("clients_pk");
        });
        
        modelBuilder.Entity<DeliveryAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("deliveries_pk");
        });
        
        modelBuilder.Entity<FrameAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frame_pk");
        });

        modelBuilder.Entity<FramePieceAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("frame_pieces_pk");
        });

        modelBuilder.Entity<OrderAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("orders_pk");
        });

        modelBuilder.Entity<SupplierAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("suppliers_pk");
        });

        modelBuilder.Entity<UserAudit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pk");
        });
    }
}