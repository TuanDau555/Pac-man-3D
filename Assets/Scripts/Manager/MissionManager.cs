using System;
using System.Collections.Generic;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    [SerializeField] private MissionDataSO missionData;
    [SerializeField] private LevelDataSO levelData;
    [SerializeField] private ScoreManager scoreManager;


    private int _completeMission;
    private IObjectiveFactory _factory;

    public event EventHandler AllMissionCompleted;

    public MissionRuntime Runtime { get; private set; }

    #region Execute

    private void Awake()
    {
        _factory = new ObjectiveFactory(scoreManager);

    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        if (Runtime != null)
        {
            Runtime.MissionCompleted -= HandleMissionCompleted;
            Runtime.Stop();
        }
    }

    #endregion

    #region Public

    public void Init(LevelDataSO levelData)
    {
        Runtime?.Stop();

        Runtime = new MissionRuntime(levelData.Mission, _factory);

        Runtime.MissionCompleted += HandleMissionCompleted;

        Runtime.Start();
    }

    #endregion

    private void HandleMissionCompleted(object sender, MissionRuntime e)
    {
        AllMissionCompleted?.Invoke(this, EventArgs.Empty);
    }
}