using EF.Models;
using Service.Dto.Transaction;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto
{
    public class BaseDto
    {
        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CreatedBy { get; set; }
        public string? CreatedByStr { get; set; }


        public string CreatedDateStr => CreatedDate.ToString("d", CultureInfo.InvariantCulture);
    }

    public class TransactionBaseDto : BaseDto
    {
        public bool IsSubmitted { get; set; }

        public DateTime? SubmittedDate { get; set; }

        public int? SubmittedBy { get; set; }
        public bool IsDeleted { get; set; }

        public DateTime? DeletedDate { get; set; }

        public int? DeletedBy { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? Updatedby { get; set; }

        public UserTransactionPermissions? UserTransactionPermissions { get; set; }
        public TransactionStatusDto? TransactionStatus { get; set; }
        public string Status { get; set; } = null!;

    }
}
