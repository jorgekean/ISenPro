using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class WorkStepApproverDto : BaseDto
    {
        public int? Id { get; set; }

        public int WorkstepId { get; set; }

        public int UserAccountId { get; set; }

        public string? UserAccountName { get; set; }
    }
}
