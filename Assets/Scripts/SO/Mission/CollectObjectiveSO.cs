using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Collect Mission")]
public class CollectObjectiveSO : ObjectiveDataSO
{
    [Header("Collect")]

    [SerializeField]
    private PickupDataSO targetPickup;

    [SerializeField]
    private int targetAmount = 1;

    public PickupDataSO TargetPickup => targetPickup;

    public int TargetAmount => targetAmount;
}