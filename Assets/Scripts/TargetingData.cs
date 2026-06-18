public class TargetingData
{
    public int Range { get; }
    public ActionType ActionType { get; }

    public TargetingData(int range, ActionType actionType)
    {
        Range = range;
        ActionType = actionType;
    }
}
