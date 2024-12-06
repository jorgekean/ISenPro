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

        public int UnitOfMeasurementId { get; set; }

        public bool? IsOriginal { get; set; }

        public string? UnitOfMeasurementCode { get; set; }
        public virtual SsUnitOfMeasurement UnitOfMeasurement { get; set; } = null!;
    }
}