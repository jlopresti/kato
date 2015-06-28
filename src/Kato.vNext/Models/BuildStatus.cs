namespace Kato.vNext.Models
{
    public enum BuildStatus
    {
        Unknown,
        Disabled,
        AbortedAndBuilding,
        Aborted,
        FailedAndBuilding,
        Failed,
        SuccessAndBuilding,
        Success,
    }
}