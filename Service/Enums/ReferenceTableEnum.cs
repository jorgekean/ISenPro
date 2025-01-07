using System.ComponentModel;

namespace Service.Enums
{
    public enum ReferanceTableModule
    {
        Title = 1,

        [Description("Employee Status")]
        EmployeeStatus = 2,

        Industry = 3,

        [Description("Report Section")]
        ReportSection = 4,

        [Description("Signatory Designation")]
        SignatoryDesignation = 5,

        [Description("Signatory Office")]
        SignatoryOffice = 6,

        [Description("APP Inflation Valuee")]
        APP = 7,

        [Description("PPMP Inflation Value")]
        PPMP = 8
    }
}
