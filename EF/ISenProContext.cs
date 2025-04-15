using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF.Models
{
    public partial class ISenProContext : DbContext
    {

        public virtual DbSet<UserTransactionPermissions> UserTransactionPermissions { get; set; }

        public async Task<UserTransactionPermissions> GetUserTransactionPermissionsAsync(
        int transactionId,
        int userAccountId,
        int moduleId = 25)
        {
            return await UserTransactionPermissions
                .FromSqlInterpolated($"SELECT * FROM dbo.fn_GetUserTransactionPermissions({transactionId}, {userAccountId}, {moduleId})")
                .FirstOrDefaultAsync();
        }

        // Partial method for additional model configuration
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTransactionPermissions>(entity =>
            {
                entity.HasNoKey();
                entity.Property(e => e.WorkStepId).HasColumnName("WorkStepId");
                entity.Property(e => e.CanApprove).HasColumnName("CanApprove");
                entity.Property(e => e.CanModify).HasColumnName("CanModify");
                entity.Property(e => e.WorkStepName).HasColumnName("WorkStepName");
                entity.Property(e => e.RequiredApprover).HasColumnName("RequiredApprover");
            });
        }
    }

    public class UserTransactionPermissions
    {
        public int WorkStepId { get; set; }
        public bool CanApprove { get; set; }
        public bool CanModify { get; set; }
        public string WorkStepName { get; set; } = null!;
        public int RequiredApprover { get; set; }
    }
}
