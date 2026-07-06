using System;

public class ReachScoreObjectiveRuntime : ObjectiveRuntime
{
    private readonly ReachScoreObjectiveSO _objective;

    private readonly ScoreManager _scoreManager;

    public ReachScoreObjectiveRuntime(ReachScoreObjectiveSO objective, ScoreManager scoreManager) : base(objective)
    {
        _objective = objective;
        _scoreManager = scoreManager;
        TargetProgress = objective.TargetScore;
    }

    public override void Register()
    {
        GameplayEvents.OnScoreChanged += HandleScoreChanged;

        HandleScoreChanged(_scoreManager, _scoreManager.Score);
    }

    public override void Unregister()
    {
        GameplayEvents.OnScoreChanged -= HandleScoreChanged;
    }

    private void HandleScoreChanged(object sender, int score)
    {
        if (IsCompleted)
            return;

        CurrentProgress++;
        
        if (score >= _objective.TargetScore)
        {
            Complete();
        }

        SetProgress(score);
    }
}