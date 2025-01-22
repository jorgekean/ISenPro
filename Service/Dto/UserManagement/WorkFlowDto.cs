using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.UserManagement
{
    public class WorkFlowDto : BaseDto
    {
        public int? Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? ModuleId { get; set; }

        public string? ModuleName { get; set; }

        public virtual IEnumerable<WorkStepDto> WorkSteps { get; set; } = new List<WorkStepDto>();
    }
}
