namespace Kato.vNext.Models
{
    public class StatusChangedArgs
    {
        public StatusChangedArgs(BuildStatus oldValue, BuildStatus newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public BuildStatus OldValue { get; private set; }
        public BuildStatus NewValue { get; private set; }
    }
}