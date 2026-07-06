using System;
using UnityEngine;

public class GameplayManager : Singleton<GameplayManager>
{

    #region Paramter

    [Header("Mission")]
    [SerializeField] private MissionManager missionManager;
    [SerializeField] private MissionUI missionUI;
    [SerializeField] private LevelDataSO currentLevel;
    [SerializeField] private GameObject normalCube;
    [SerializeField] private GameObject winCube;

    [Header("Enviroment")]
    [SerializeField] private float _skyBoxRotationSpeed = 2f;

    #endregion

    #region Excute

    protected override void Awake()
    {
        base.Awake();
        missionManager.AllMissionCompleted += HandleMissionCompleted;
    }

    private void OnDestroy()
    {
        missionManager.AllMissionCompleted -= HandleMissionCompleted;
    }

    private void Start()
    {
        missionManager.Init(currentLevel);

        missionManager.AllMissionCompleted += HandleMissionCompleted;

        // missionManager.StartMission();

        missionUI.Bind(missionManager.Runtime);
        
    }

    private void Update()
    {
        SkyBoxRotation();
    }

    #endregion

    #region Events

    private void HandleMissionCompleted(object sender, EventArgs e)
    {
        TriggerWin();
    }

    #endregion

    #region Enviroment

    private void SkyBoxRotation()
    {
        if (RenderSettings.skybox != null)
        {
            float rot = RenderSettings.skybox.GetFloat("_Rotation");
            RenderSettings.skybox.SetFloat("_Rotation", rot + _skyBoxRotationSpeed * Time.deltaTime);
        }
    }

    #endregion

    #region Game Condition

    public void TriggerWin()
    {
        Debug.Log("Game win");
        normalCube.gameObject.SetActive(false);
        winCube.gameObject.SetActive(true);
    }

    #endregion
}