using System.ComponentModel;

namespace Service.Enums
{
    public enum ParentModule
    {
        [Description("User Management")]
        UserManagement = 11,

        [Description("System Setup")]
        SystemSetup = 12,

        [Description("Transactions")]
        Transactions = 13,

        [Description("Reports")]
        Reports = 14,

        [Description("Monitoring")]
        Monitoring = 15
    }
}
