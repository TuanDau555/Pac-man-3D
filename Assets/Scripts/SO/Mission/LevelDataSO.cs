using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Mission/Level Data")]
public class LevelDataSO : ScriptableObject
{
    [SerializeField]
    private MissionDataSO missions;

    public MissionDataSO Mission => missions;
}