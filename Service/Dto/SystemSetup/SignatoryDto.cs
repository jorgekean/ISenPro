using EF.Models.UserManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Dto.SystemSetup
{
    public class SignatoryDto : BaseDto
    {
        public int Id { get; set; }

        public int? Transactions { get; set; }

        public string? ModuleName { get; set; }

        public int? Sequence { get; set; }

        public int? SignatoryDesignationId { get; set; }

        public string? SignatoryDesignation { get; set; }

        public int? SignatoryOfficeId { get; set; }

        public string? SignatoryOffice { get; set; }

        public int? ReportSectionId { get; set; }

        public bool? WithCondition { get; set; }

        public double? MinimumAmount { get; set; }

        public double? MaximumAmount { get; set; }

        public int? PersonId { get; set; }

        public string? PersonOffice { get; set; }

        public virtual PersonDto? Person { get; set; }

        public bool? IsActive { get; set; }

        public long? CreatedByUserId { get; set; }

        public DateTime? CreatedDate { get; set; }
    }
}
