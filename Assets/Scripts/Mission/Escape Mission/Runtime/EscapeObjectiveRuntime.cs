using System;

public class EscapeObjectiveRuntime : ObjectiveRuntime
{
    public EscapeObjectiveRuntime(EscapeObjectiveSO objective) : base(objective)
    {
        // TargetProgress = 1;
    }

    public override void Register()
    {
        GameplayEvents.OnExitReached += HandleExitReached;
    }

    public override void Unregister()
    {
        GameplayEvents.OnExitReached -= HandleExitReached;
    }

    private void HandleExitReached(object sender, EventArgs e)
    {
        if (IsCompleted)
            return;

        SetProgress(0);

        Complete();
    }
}