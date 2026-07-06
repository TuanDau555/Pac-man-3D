using System;

public class ObjectiveFactory : IObjectiveFactory
{
    private readonly ScoreManager _scoreManager;

    public ObjectiveFactory(ScoreManager scoreManager)
    {
        _scoreManager = scoreManager;
    }

    public ObjectiveRuntime Create(ObjectiveDataSO data)
    {
        return data switch
        {
            CollectObjectiveSO collect => new CollectObjectiveRuntime(collect),
            EscapeObjectiveSO escape => new EscapeObjectiveRuntime(escape),
            ReachScoreObjectiveSO reachScore => new ReachScoreObjectiveRuntime(reachScore, _scoreManager),

            _ => throw new NotSupportedException($"Mission type {data.GetType().Name} is not supported.")
        };
    }
}