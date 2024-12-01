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

    #region User Management
    public virtual DbSet<UmPerson> UmPeople { get; set; }

    public virtual DbSet<UmPolicy> UmPolicies { get; set; }

    public virtual DbSet<UmRole> UmRoles { get; set; }

    public virtual DbSet<UmUserAccount> UmUserAccounts { get; set; }
    #endregion

    #region System Setup

    public virtual DbSet<SsUnitOfMeasurement> SsUnitOfMeasurements { get; set; }

    public virtual DbSet<SsPurchasingType> SsPurchasingTypes { get; set; }

    public virtual DbSet<SsAccountCode> SsAccountCodes { get; set; }

    public virtual DbSet<SsItemType> SsItemTypes { get; set; }

    public virtual DbSet<SsMajorCategory> SsMajorCategories { get; set; }

    public virtual DbSet<SsSubCategory> SsSubCategories { get; set; }

    public virtual DbSet<SsSupplier> SsSuppliers { get; set; }
    #endregion

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

        modelBuilder.Entity<SsAccountCode>(entity =>
        {
            entity.HasKey(e => e.AccountCodeId);

            entity.ToTable("SS_AccountCode");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ItemType).WithMany(p => p.SsAccountCodes)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_SS_AccountCode_SS_ItemType");
        });

        modelBuilder.Entity<SsItemType>(entity =>
        {
            entity.HasKey(e => e.ItemTypeId);

            entity.ToTable("SS_ItemType");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SsMajorCategory>(entity =>
        {
            entity.HasKey(e => e.MajorCategoryId);

            entity.ToTable("SS_MajorCategory");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.AccountCode).WithMany(p => p.SsMajorCategories)
                .HasForeignKey(d => d.AccountCodeId)
                .HasConstraintName("FK_SS_MajorCategory_SS_AccountCode");
        });

        modelBuilder.Entity<SsSubCategory>(entity =>
        {
            entity.HasKey(e => e.SubCategoryId);

            entity.ToTable("SS_SubCategory");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.MajorCategory).WithMany(p => p.SsSubCategories)
                .HasForeignKey(d => d.MajorCategoryId)
                .HasConstraintName("FK_SS_SubCategory_SS_MajorCategory");
        });

        modelBuilder.Entity<SsSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__SS_Suppl__4BE666B4504D5BF9");

            entity.ToTable("SS_Suppliers");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.DeletedDate).HasColumnType("datetime");
            entity.Property(e => e.RestoredDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
        });

        #endregion

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
