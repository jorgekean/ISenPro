using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Enums
{
    public enum FilterCriteriaEnum
    {
        [Description("View only transaction of my office")]
        TransofMyDepartment = 11,

        [Description("View only my transactions")]
        MyTransaction = 12,

        [Description("View only transactions that needs my approval")]
        TransNeedMyApproval = 13,

        [Description("View only Approved Transactions")]
        ApprovedTransactions = 14,

        [Description("View only Supplies")]
        Supplies = 15,

        [Description("View only Equipment")]
        Equipments = 16,

        [Description("View all transactions specific to the following Offices :")]
        TransInDepartment = 18,

        [Description("View all transactions specific to the following Bureaus :")]
        TransInBureau = 17,

        [Description("View all transactions specific to the following Sections :")]
        TransInSection = 19
    };
}
