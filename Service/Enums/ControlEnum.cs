using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Enums
{
    public enum ControlEnum
    {
        [Description("Create")]
        Create = 11,

        [Description("Edit")]
        Edit = 12,

        [Description("Delete")]
        Delete = 13,

        [Description("Cancel")]
        Cancel = 14,

        [Description("View")]
        View = 15,

        [Description("Approve")]
        Approve = 16,

        [Description("Disapprove")]
        Disapprove = 17,

        [Description("Print")]
        Print = 18,

        [Description("Submit")]
        Submit = 19,

        [Description("Save")]
        Save = 20,

        [Description("Restore")]
        Restore = 21,

        [Description("Graph")]
        Graph = 22,

        [Description("Generate RFQ / RSQ")]
        GenerateRFQRSQ = 23,

        [Description("Generate Abstract")]
        GenerateAbstract = 24,

        [Description("For Validation")]
        ForValidation = 25,

        [Description("Void")]
        Void = 26,

        [Description("PR Options")]
        PrOptions = 27,

        [Description("FailureOfCanvass")]
        FailureOfCanvass = 28,

        [Description("Issue Request")]
        IssueRequest = 29,

        [Description("Print Gate Pass")]
        PrintGatePass = 30,


        [Description("Print Invoice Receipt")]
        PrintInvoiceReceipt = 31

    };
}
