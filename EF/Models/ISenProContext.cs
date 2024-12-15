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
    public virtual DbSet<UmBureau> UmBureaus { get; set; }

    public virtual DbSet<UmDepartment> UmDepartments { get; set; }

    public virtual DbSet<UmDivision> UmDivisions { get; set; }

    public virtual DbSet<UmPerson> UmPeople { get; set; }

    public virtual DbSet<UmSection> UmSections { get; set; }

    public virtual DbSet<UmPolicy> UmPolicies { get; set; }

    public virtual DbSet<UmRole> UmRoles { get; set; }

    public virtual DbSet<UmUserAccount> UmUserAccounts { get; set; }
    public virtual DbSet<UmControl> UmControls { get; set; }

    public virtual DbSet<UmModule> UmModules { get; set; }

    public virtual DbSet<UmModuleControl> UmModuleControls { get; set; }

    public virtual DbSet<UmPage> UmPages { get; set; }
    #endregion

    #region System Setup

    public virtual DbSet<SsPurchasingType> SsPurchasingTypes { get; set; }
    public virtual DbSet<SsSupplier> SsSuppliers { get; set; }

    public virtual DbSet<SsAccountCode> SsAccountCodes { get; set; }

    public virtual DbSet<SsItemType> SsItemTypes { get; set; }

    public virtual DbSet<SsMajorCategory> SsMajorCategories { get; set; }

    public virtual DbSet<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; }

    public virtual DbSet<SsSubCategory> SsSubCategories { get; set; }

    public virtual DbSet<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; }

    public virtual DbSet<SsUnitOfMeasurement> SsUnitOfMeasurements { get; set; }

    #endregion

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        #region User Management
        modelBuilder.Entity<UmBureau>(entity =>
        {
            entity.HasKey(e => e.BureauId);

            entity.ToTable("UM_Bureau");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Division).WithMany(p => p.UmBureaus)
                .HasForeignKey(d => d.DivisionId)
                .HasConstraintName("FK_UM_Bureau_UM_Division");
        });

        modelBuilder.Entity<UmDepartment>(entity =>
        {
            entity.HasKey(e => e.DepartmentId);

            entity.ToTable("UM_Department");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Bureau).WithMany(p => p.UmDepartments)
                .HasForeignKey(d => d.BureauId)
                .HasConstraintName("FK_UM_Department_UM_Bureau");
        });

        modelBuilder.Entity<UmDivision>(entity =>
        {
            entity.HasKey(e => e.DivisionId);

            entity.ToTable("UM_Division");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<UmPerson>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("UM_Person");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ContactNo).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Designation).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);

            entity.HasOne(d => d.Department).WithMany(p => p.UmPeople)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UM_Person_UM_Department");

            entity.HasOne(d => d.Section).WithMany(p => p.UmPeople)
                .HasForeignKey(d => d.SectionId)
                .HasConstraintName("FK_UM_Person_UM_Section");
        });

        modelBuilder.Entity<UmSection>(entity =>
        {
            entity.HasKey(e => e.SectionId);

            entity.ToTable("UM_Section");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Department).WithMany(p => p.UmSections)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK_UM_Section_UM_Department");
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

        modelBuilder.Entity<UmControl>(entity =>
        {
            entity.HasKey(e => e.ControlId);

            entity.ToTable("UM_Control");

            entity.Property(e => e.ControlId).ValueGeneratedNever();
            entity.Property(e => e.ControlName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
        });

        modelBuilder.Entity<UmModule>(entity =>
        {
            entity.HasKey(e => e.ModuleId);

            entity.ToTable("UM_Module");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Page).WithMany(p => p.UmModules)
                .HasForeignKey(d => d.PageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_Module_UM_Page");
        });

        modelBuilder.Entity<UmModuleControl>(entity =>
        {
            entity.HasKey(e => e.ModuleControlId);

            entity.ToTable("UM_ModuleControl");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Control).WithMany(p => p.UmModuleControls)
                .HasForeignKey(d => d.ControlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_ModuleControl_UM_Control");

            entity.HasOne(d => d.Module).WithMany(p => p.UmModuleControls)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_ModuleControl_UM_Module");
        });

        modelBuilder.Entity<UmPage>(entity =>
        {
            entity.HasKey(e => e.PageId);

            entity.ToTable("UM_Page");

            entity.Property(e => e.PageId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(200);
            entity.Property(e => e.PageName).HasMaxLength(50);
        });
        #endregion


        #region System Setup

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
        
        modelBuilder.Entity<SsSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__SS_Suppl__4BE666B472D0BDEC");

            entity.ToTable("SS_Suppliers");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FaxNumber).HasMaxLength(50);
            entity.Property(e => e.Tin).HasMaxLength(30);
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

        modelBuilder.Entity<SsPsdbmcatalogue>(entity =>
        {
            entity.HasKey(e => e.PsdbmcatalogueId);

            entity.ToTable("SS_PSDBMCatalogue");

            entity.Property(e => e.PsdbmcatalogueId).HasColumnName("PSDBMCatalogueId");
            entity.Property(e => e.CatalogueYear).HasColumnType("datetime");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Thumbnail).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(19, 5)");

            entity.HasOne(d => d.AccountCode).WithMany(p => p.SsPsdbmcatalogues)
                .HasForeignKey(d => d.AccountCodeId)
                .HasConstraintName("FK_SS_PSDBMCatalogue_SS_AccountCode");

            entity.HasOne(d => d.ItemType).WithMany(p => p.SsPsdbmcatalogues)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_SS_PSDBMCatalogue_SS_ItemType");

            entity.HasOne(d => d.MajorCategory).WithMany(p => p.SsPsdbmcatalogues)
                .HasForeignKey(d => d.MajorCategoryId)
                .HasConstraintName("FK_SS_PSDBMCatalogue_SS_MajorCategory");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.SsPsdbmcatalogues)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK_SS_PSDBMCatalogue_SS_SubCategory");

            entity.HasOne(d => d.UnitOfMeasurement).WithMany(p => p.SsPsdbmcatalogues)
                .HasForeignKey(d => d.UnitOfMeasurementId)
                .HasConstraintName("FK_SS_PSDBMCatalogue_SS_UnitOfMeasurement");
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

        modelBuilder.Entity<SsSupplementaryCatalogue>(entity =>
        {
            entity.HasKey(e => e.SupplementaryCatalogueId);

            entity.ToTable("SS_SupplementaryCatalogue");

            entity.Property(e => e.CatalogueYear).HasColumnType("datetime");
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Thumbnail).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(19, 5)");

            entity.HasOne(d => d.AccountCode).WithMany(p => p.SsSupplementaryCatalogues)
                .HasForeignKey(d => d.AccountCodeId)
                .HasConstraintName("FK_SS_SupplementaryCatalogue_SS_AccountCode");

            entity.HasOne(d => d.ItemType).WithMany(p => p.SsSupplementaryCatalogues)
                .HasForeignKey(d => d.ItemTypeId)
                .HasConstraintName("FK_SS_SupplementaryCatalogue_SS_ItemType");

            entity.HasOne(d => d.MajorCategory).WithMany(p => p.SsSupplementaryCatalogues)
                .HasForeignKey(d => d.MajorCategoryId)
                .HasConstraintName("FK_SS_SupplementaryCatalogue_SS_MajorCategory");

            entity.HasOne(d => d.SubCategory).WithMany(p => p.SsSupplementaryCatalogues)
                .HasForeignKey(d => d.SubCategoryId)
                .HasConstraintName("FK_SS_SupplementaryCatalogue_SS_SubCategory");

            entity.HasOne(d => d.UnitOfMeasurement).WithMany(p => p.SsSupplementaryCatalogues)
                .HasForeignKey(d => d.UnitOfMeasurementId)
                .HasConstraintName("FK_SS_SupplementaryCatalogue_SS_UnitOfMeasurement");
        });

        modelBuilder.Entity<SsUnitOfMeasurement>(entity =>
        {
            entity.HasKey(e => e.UnitOfMeasurementId);

            entity.ToTable("SS_UnitOfMeasurement");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        #endregion

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
