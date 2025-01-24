using System;
using System.Collections.Generic;
using EF.Models.SystemSetup;
using EF.Models.UserManagement;
using Microsoft.EntityFrameworkCore;

namespace EF.Models;

public partial class TempContext : DbContext
{
    public TempContext()
    {
    }

    public TempContext(DbContextOptions<TempContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SsReferenceTable> SsReferenceTables { get; set; }

    public virtual DbSet<SsSignatory> SsSignatories { get; set; }

    public virtual DbSet<UmBureau> UmBureaus { get; set; }

    public virtual DbSet<UmControl> UmControls { get; set; }

    public virtual DbSet<UmDepartment> UmDepartments { get; set; }

    public virtual DbSet<UmDivision> UmDivisions { get; set; }

    public virtual DbSet<UmModule> UmModules { get; set; }

    public virtual DbSet<UmModuleControl> UmModuleControls { get; set; }

    public virtual DbSet<UmPage> UmPages { get; set; }

    public virtual DbSet<UmPerson> UmPeople { get; set; }

    public virtual DbSet<UmPolicyModuleControl> UmPolicyModuleControls { get; set; }

    public virtual DbSet<UmPolicyRole> UmPolicyRoles { get; set; }

    public virtual DbSet<UmRole> UmRoles { get; set; }

    public virtual DbSet<UmSection> UmSections { get; set; }

    public virtual DbSet<UmUserAccount> UmUserAccounts { get; set; }

    public virtual DbSet<UmWorkFlow> UmWorkFlows { get; set; }

    public virtual DbSet<UmWorkStep> UmWorkSteps { get; set; }

    public virtual DbSet<UmWorkStepApprover> UmWorkStepApprovers { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=(LocalDB)\\MSSQLLocalDB;Database=TestinganDB;Integrated Security=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SsReferenceTable>(entity =>
        {
            entity.HasKey(e => e.ReferenceTableId).HasName("PK__SS_Refer__65F2CF6F263D4BA4");

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

            entity.ToTable("UM_Role");

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

            entity.HasOne(d => d.Module).WithMany(p => p.UmWorkFlows)
                .HasForeignKey(d => d.ModuleId)
                .HasConstraintName("FK_UM_WorkFlows_UM_Modules");
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

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
