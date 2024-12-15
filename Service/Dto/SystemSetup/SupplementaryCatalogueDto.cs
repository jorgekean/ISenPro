using EF.Models.SystemSetup;
using System;
using System.Collections.Generic;

namespace Service.Dto.SystemSetup
{
    public partial class SupplementaryCatalogueDto : BaseDto
    {
        public int Id { get; set; }

        public DateTime? CatalogueYear { get; set; }

        public string? Code { get; set; }

        public string? Description { get; set; }

        public decimal? UnitPrice { get; set; }

        public bool? IsCurrent { get; set; }

        public string? Remarks { get; set; }

        public string? Thumbnail { get; set; }

        public int? UnitOfMeasurementId { get; set; }

        public int? ItemTypeId { get; set; }

        public int? AccountCodeId { get; set; }

        public int? MajorCategoryId { get; set; }

        public int? SubCategoryId { get; set; }

        public bool? IsOriginal { get; set; }

        public string? ItemTypeName { get; set; }
        public string? AccountCodeDescription { get; set; }
        public string? MajorCategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? UnitOfMeasurementCode { get; set; }
        public virtual UnitOfMeasurementDto? UnitOfMeasurement { get; set; } = null!;

        public virtual ItemTypeDto? ItemType { get; set; } = null!;

        public virtual AccountCodeDto? AccountCode { get; set; } = null!;

        public virtual MajorCategoryDto? MajorCategory { get; set; } = null!;

        public virtual SubCategoryDto? SubCategory { get; set; } = null!;
    }
}