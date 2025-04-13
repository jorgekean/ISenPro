using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.Transaction
{
    public class TransactionStatusDto
    {
        public int Id { get; set; }

        public int PageId { get; set; }

        public int ProcessByUserId { get; set; }

        public bool IsCurrent { get; set; }

        public int Count { get; set; }

        public string? Status { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedDate { get; set; }

        public int TransactionId { get; set; }

        public bool IsDone { get; set; }

        public int? WorkstepId { get; set; }

        public string? Action { get; set; }


        //public string? WorkStepName { get; set; }
        public bool Approved { get; set; }
        public bool Disapproved { get; set; }

        // default value true
        public bool IsActive { get; set; } = true;
    }
}
