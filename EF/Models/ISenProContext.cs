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

    public virtual DbSet<ChangeDetailsTable> ChangeDetailsTables { get; set; }

    public virtual DbSet<ChangeTrackingTable> ChangeTrackingTables { get; set; }

    public virtual DbSet<CtUmRole> CtUmRoles { get; set; }

    public virtual DbSet<CtUmRoleDetail> CtUmRoleDetails { get; set; }

    public virtual DbSet<SsAccountCode> SsAccountCodes { get; set; }

    public virtual DbSet<SsItemType> SsItemTypes { get; set; }

    public virtual DbSet<SsMajorCategory> SsMajorCategories { get; set; }

    public virtual DbSet<SsPsdbmcatalogue> SsPsdbmcatalogues { get; set; }

    public virtual DbSet<SsPurchasingType> SsPurchasingTypes { get; set; }

    public virtual DbSet<SsReferenceTable> SsReferenceTables { get; set; }

    public virtual DbSet<SsSignatory> SsSignatories { get; set; }

    public virtual DbSet<SsSubCategory> SsSubCategories { get; set; }

    public virtual DbSet<SsSupplementaryCatalogue> SsSupplementaryCatalogues { get; set; }

    public virtual DbSet<SsSupplier> SsSuppliers { get; set; }

    public virtual DbSet<SsUnitOfMeasurement> SsUnitOfMeasurements { get; set; }

    public virtual DbSet<UmBureau> UmBureaus { get; set; }

    public virtual DbSet<UmControl> UmControls { get; set; }

    public virtual DbSet<UmDepartment> UmDepartments { get; set; }

    public virtual DbSet<UmDivision> UmDivisions { get; set; }

    public virtual DbSet<UmModule> UmModules { get; set; }

    public virtual DbSet<UmModuleControl> UmModuleControls { get; set; }

    public virtual DbSet<UmPage> UmPages { get; set; }

    public virtual DbSet<UmPerson> UmPeople { get; set; }

    public virtual DbSet<UmPolicy> UmPolicies { get; set; }

    public virtual DbSet<UmPolicyModuleControl> UmPolicyModuleControls { get; set; }

    public virtual DbSet<UmPolicyRole> UmPolicyRoles { get; set; }

    public virtual DbSet<UmRole> UmRoles { get; set; }

    public virtual DbSet<UmSection> UmSections { get; set; }

    public virtual DbSet<UmUserAccount> UmUserAccounts { get; set; }

    public virtual DbSet<UmWorkFlow> UmWorkFlows { get; set; }

    public virtual DbSet<UmWorkStep> UmWorkSteps { get; set; }

    public virtual DbSet<UmWorkStepApprover> UmWorkStepApprovers { get; set; }

    public virtual DbSet<SsSupplierContactPerson> SsSupplierContactPeople { get; set; }

    public virtual DbSet<SsItemStatus> SsItemStatuses { get; set; }

    public virtual DbSet<SsModeOfProcurement> SsModeOfProcurements { get; set; }

    public virtual DbSet<SsMopDetail> SsMopDetails { get; set; }

    public virtual DbSet<SsSubStatus> SsSubStatuses { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChangeDetailsTable>(entity =>
        {
            entity.HasKey(e => new { e.ChangeId, e.FieldName }).HasName("PK__ChangeDe__648DB5CDAA106B05");

            entity.ToTable("ChangeDetailsTable");

            entity.Property(e => e.ChangeId).HasColumnName("ChangeID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(128)
                .IsUnicode(false);

            entity.HasOne(d => d.Change).WithMany(p => p.ChangeDetailsTables)
                .HasForeignKey(d => d.ChangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChangeDet__Chang__267ABA7A");
        });

        modelBuilder.Entity<ChangeTrackingTable>(entity =>
        {
            entity.HasKey(e => e.ChangeId).HasName("PK__ChangeTr__0E05C5B70781BC56");

            entity.ToTable("ChangeTrackingTable");

            entity.Property(e => e.ChangeId).HasColumnName("ChangeID");
            entity.Property(e => e.Operation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<CtUmRole>(entity =>
        {
            entity.HasKey(e => e.ChangeId).HasName("PK__CT_UM_Ro__0E05C5B78A2B9E4C");

            entity.ToTable("CT_UM_Role");

            entity.Property(e => e.ChangeId).HasColumnName("ChangeID");
            entity.Property(e => e.Operation)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.Timestamp).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(128)
                .IsUnicode(false)
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<CtUmRoleDetail>(entity =>
        {
            entity.HasKey(e => new { e.ChangeId, e.FieldName }).HasName("PK__CT_UM_Ro__648DB5CD2C17F4CD");

            entity.ToTable("CT_UM_Role_Details");

            entity.Property(e => e.ChangeId).HasColumnName("ChangeID");
            entity.Property(e => e.FieldName)
                .HasMaxLength(128)
                .IsUnicode(false);

            entity.HasOne(d => d.Change).WithMany(p => p.CtUmRoleDetails)
                .HasForeignKey(d => d.ChangeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CT_UM_Rol__Chang__2B3F6F97");
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

        modelBuilder.Entity<SsReferenceTable>(entity =>
        {
            entity.HasKey(e => e.ReferenceTableId).HasName("PK__SS_Refer__65F2CF6FB04FE3BE");

            entity.ToTable("SS_ReferenceTable");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.InflationValue).HasColumnType("decimal(19, 5)");
        });

        modelBuilder.Entity<SsSignatory>(entity =>
        {
            entity.HasKey(e => e.SignatoryId);

            entity.ToTable("SS_Signatories");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.SsSignatories)
                .HasForeignKey(d => d.PersonId)
                .HasConstraintName("FK_SS_Signatories_SS_Person");

            entity.HasOne(d => d.ReportSection).WithMany(p => p.SsSignatoryReportSections)
                .HasForeignKey(d => d.ReportSectionId)
                .HasConstraintName("FK_SS_Signatories_SS_Reference_Report_Section");

            entity.HasOne(d => d.SignatoryDesignation).WithMany(p => p.SsSignatorySignatoryDesignations)
                .HasForeignKey(d => d.SignatoryDesignationId)
                .HasConstraintName("FK_SS_Signatories_SS_Reference_Signatory_Designation");
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

        modelBuilder.Entity<SsSupplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("PK__SS_Suppl__4BE666B41D945853");

            entity.ToTable("SS_Suppliers");

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.CompanyName).HasMaxLength(200);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.EmailAddress).HasMaxLength(100);
            entity.Property(e => e.FaxNumber).HasMaxLength(50);
            entity.Property(e => e.Tin).HasMaxLength(30);
        });

        modelBuilder.Entity<SsSupplierContactPerson>(entity =>
        {
            entity.HasKey(e => e.SupplierContactPersonId);

            entity.ToTable("SS_SupplierContactPerson");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Supplier).WithMany(p => p.SsSupplierContactPeople)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_SS_SupplierContactPerson_SS_Suppliers");
        });

        modelBuilder.Entity<SsUnitOfMeasurement>(entity =>
        {
            entity.HasKey(e => e.UnitOfMeasurementId);

            entity.ToTable("SS_UnitOfMeasurement");

            entity.Property(e => e.Code).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

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

        modelBuilder.Entity<UmControl>(entity =>
        {
            entity.HasKey(e => e.ControlId);

            entity.ToTable("UM_Control");

            entity.Property(e => e.ControlId).ValueGeneratedNever();
            entity.Property(e => e.ControlName).HasMaxLength(50);
            entity.Property(e => e.Description).HasMaxLength(200);
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

        modelBuilder.Entity<UmPerson>(entity =>
        {
            entity.HasKey(e => e.PersonId);

            entity.ToTable("UM_Person");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ContactNo).HasMaxLength(100);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Designation).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeStatus).HasMaxLength(50);
            entity.Property(e => e.EmployeeTitle).HasMaxLength(50);
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

        modelBuilder.Entity<UmPolicy>(entity =>
        {
            entity.HasKey(e => e.PolicyId);

            entity.ToTable("UM_Policy");

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(200);
        });

        modelBuilder.Entity<UmPolicyModuleControl>(entity =>
        {
            entity.HasKey(e => e.PolicyModuleControlId);

            entity.ToTable("UM_PolicyModuleControls");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ModuleControl).WithMany(p => p.UmPolicyModuleControls)
                .HasForeignKey(d => d.ModuleControlId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_PolicyModuleControls_UM_ModuleControl");
        });

        modelBuilder.Entity<UmPolicyRole>(entity =>
        {
            entity.HasKey(e => e.PolicyRoleId);

            entity.ToTable("UM_PolicyRoles");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UmPolicyRoles)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_PolicyRoles_UM_Role");
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

        modelBuilder.Entity<UmUserAccount>(entity =>
        {
            entity.HasKey(e => e.UserAccountId).HasName("PK_UM_UserAccount_1");

            entity.ToTable("UM_UserAccount");

            entity.HasIndex(e => e.PersonId, "IX_UM_UserAccount").IsUnique();

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
            entity.Property(e => e.ExpireDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(100);
            entity.Property(e => e.UserId)
                .HasMaxLength(50)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Person).WithOne(p => p.UmUserAccount)
                .HasForeignKey<UmUserAccount>(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_UserAccount_UM_Person");

            entity.HasOne(d => d.Role).WithMany(p => p.UmUserAccounts)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UM_UserAccount_UM_Role");
        });

        modelBuilder.Entity<UmWorkFlow>(entity =>
        {
            entity.HasKey(e => e.WorkflowId).HasName("PK_UM_WorkFlows");

            entity.ToTable("UM_WorkFlow");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<UmWorkStep>(entity =>
        {
            entity.HasKey(e => e.WorkstepId);

            entity.ToTable("UM_WorkStep");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.Workflow).WithMany(p => p.UmWorkSteps)
                .HasForeignKey(d => d.WorkflowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_WorkSteps_UM_WorkFlows");
        });

        modelBuilder.Entity<UmWorkStepApprover>(entity =>
        {
            entity.HasKey(e => e.WorkstepApproverId);

            entity.ToTable("UM_WorkStepApprover");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.UserAccount).WithMany(p => p.UmWorkStepApprovers)
                .HasForeignKey(d => d.UserAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_WorkStepApprovers_UM_UserAccounts");

            entity.HasOne(d => d.Workstep).WithMany(p => p.UmWorkStepApprovers)
                .HasForeignKey(d => d.WorkstepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UM_WorkStepApprovers_UM_WorkSteps");
        });

        modelBuilder.Entity<SsItemStatus>(entity =>
        {
            entity.HasKey(e => e.ItemStatusId);

            entity.ToTable("SS_ItemStatus");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SsModeOfProcurement>(entity =>
        {
            entity.HasKey(e => e.ModeOfProcurementId);

            entity.ToTable("SS_ModeOfProcurement");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<SsMopDetail>(entity =>
        {
            entity.HasKey(e => e.MopDetailId);

            entity.ToTable("SS_MopDetail");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ModeOfProcurement).WithMany(p => p.SsMopDetails)
                .HasForeignKey(d => d.ModeOfProcurementId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SS_ModeOfProcurement_SS_MopDetail");
        });

        modelBuilder.Entity<SsSubStatus>(entity =>
        {
            entity.HasKey(e => e.SubStatusId);

            entity.ToTable("SS_SubStatus");

            entity.Property(e => e.CreatedDate).HasColumnType("datetime");

            entity.HasOne(d => d.ItemStatus).WithMany(p => p.SsSubStatuses)
                .HasForeignKey(d => d.ItemStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_SS_ItemStatus_SS_SubStatus");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
