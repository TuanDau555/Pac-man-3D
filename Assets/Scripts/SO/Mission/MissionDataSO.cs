using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Mission Data")]
public class MissionDataSO : ScriptableObject
{
    [SerializeField] private string missionName;
    [SerializeField] private List<ObjectiveDataSO> objectives;

    public IReadOnlyList<ObjectiveDataSO> Objectives => objectives;

}