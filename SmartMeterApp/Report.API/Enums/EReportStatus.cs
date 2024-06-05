using System.ComponentModel;

namespace ReportApi.Enums
{
    public enum EReportStatus
    {
        [Description("Completed")]
        Completed = 1,
        [Description("Failed to Generate")]
        FailedToGenerate = 2,
        [Description("In Progress")]
        InProgress = 3,
    }
}
