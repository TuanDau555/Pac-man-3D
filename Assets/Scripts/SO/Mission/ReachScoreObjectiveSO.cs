using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Reach Score Mission")]
public class ReachScoreObjectiveSO : ObjectiveDataSO
{
    [SerializeField]
    private int targetScore;

    public int TargetScore => targetScore;
}