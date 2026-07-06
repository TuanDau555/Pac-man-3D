
public class CollectObjectiveRuntime : ObjectiveRuntime
{
    private readonly CollectObjectiveSO _objective;

    private int _currentAmount;

    public CollectObjectiveRuntime(CollectObjectiveSO objective) : base(objective)
    {
        this._objective = objective;

        // TargetProgress = mission.TargetAmount;
    }

    public override void Register()
    {
        GameplayEvents.OnPickupColleted += HandlePickup;
    }

    public override void Unregister()
    {
        GameplayEvents.OnPickupColleted -= HandlePickup;
    }

    private void HandlePickup(object sender, PickupDataSO pickup)
    {
        if (IsCompleted)
            return;

        if (pickup != _objective.TargetPickup)
            return;

        _currentAmount++;

        if (_currentAmount >= _objective.TargetAmount)
        {
            Complete();
        }

        SetProgress(CurrentProgress + 1);
    }

}