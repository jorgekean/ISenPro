using System;
using System.Collections.Generic;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace EF.Models;

public partial class ISenProContext : DbContext
{
    public ISenProContext()
    {
    }

    public ISenProContext(DbContextOptions<ISenProContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UmPerson> UmPeople { get; set; }

    public virtual DbSet<UmPolicy> UmPolicies { get; set; }

    public virtual DbSet<UmRole> UmRoles { get; set; }

    public virtual DbSet<UmUserAccount> UmUserAccounts { get; set; }

    public virtual DbSet<SsUnitOfMeasurement> SsUnitOfMeasurements { get; set; }

    public virtual DbSet<SsPurchasingType> SsPurchasingTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region User Management
        modelBuilder.Entity<UmPerson>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("UM_Person");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ContactNo).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Designation).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(50);
        });

        modelBuilder.Entity<UmPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId);

            entity.ToTable("UM_Policy");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<UmRole>(entity =>
        {
            entity.HasKey(e => e.RoleId);

            entity.ToTable("UM_Role", tb => tb.HasTrigger("trg_UM_Role_Changes"));

            entity.Property(e => e.Code)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UmUserAccount>(entity =>
        {
            entity.HasKey(e => e.UserAccountId);

            entity.ToTable("UM_UserAccount");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");
        });
        #endregion


        #region System Setup
        modelBuilder.Entity<SsUnitOfMeasurement>(entity =>
        {
            entity.HasKey(e => e.UnitOfMeasurementId);

            entity.ToTable("SS_UnitOfMeasurement");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<SsPurchasingType>(entity =>
        {
            entity.HasKey(e => e.PurchasingTypeId);

            entity.ToTable("SS_PurchasingType");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.MaximumAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);
        });
        #endregion

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
