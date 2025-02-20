using EF.Models;
using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class WorkStepDto : BaseDto
    {
        public int? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? Sequence { get; set; }

        public bool? IsLastStep { get; set; }

        public int? RequiredApprover { get; set; }

        public bool? CanModify { get; set; }

        public int? WorkflowId { get; set; }

        public virtual UmWorkFlow? Workflow { get; set; } = null!;

        public virtual List<WorkStepApproverDto> WorkStepApprovers { get; set; } = new List<WorkStepApproverDto>();
    }
}
