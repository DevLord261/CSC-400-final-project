using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Campaign>().HasKey(c => c.Id);
        modelBuilder.Entity<Users>().HasKey(c => c.Id);
        modelBuilder.Entity<Users>().HasIndex(c => c.Email).IsUnique();

        modelBuilder.Entity<Campaign>().HasOne(u => u.owner).WithMany().HasForeignKey(c => c.ownerId);
        modelBuilder.Entity<Campaign>().Property(c => c.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(c => new { c.UserId, c.RoleId });



    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    public DbSet<Users> Users { get; set; }

    public DbSet<Campaign> campaigns { get; set; }

    public DbSet<IdentityUserClaim<string>> userClaims { get; set; }
    public DbSet<IdentityUserRole<string>> userRole { get; set; }

    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityRoleClaim<string>> claimRoles { get; set; }


}