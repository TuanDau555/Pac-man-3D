using System;
using System.Collections.Generic;

public class MissionRuntime
{
    #region Parameters

    public MissionDataSO Data { get; }
    public bool IsCompleted { get; private set; }

    public event EventHandler<MissionRuntime> MissionCompleted;
    public event EventHandler<ObjectiveRuntime> CurrentObjectiveChanged;

    private readonly List<ObjectiveRuntime> _objectives = new();
    private int _currentObjectiveIndex;

    #endregion

    #region Constructor

    public MissionRuntime(MissionDataSO data, IObjectiveFactory factory)
    {
        Data = data;

        foreach (var objectiveData in data.Objectives)
        {
            _objectives.Add(factory.Create(objectiveData));
        }
    }

    #endregion

    #region Public Api

    public void Start()
    {
        if (_objectives.Count == 0)
        {
            CompleteMission();
            return;
        }

        RegisterCurrentObjective();
    }

    public void Stop()
    {
        if (CurrentObjective == null)
            return;

        CurrentObjective.Completed -= HandleObjectiveCompleted;
        CurrentObjective.Unregister();
    }

    #endregion

    #region Objective flow

    private void RegisterCurrentObjective()
    {
        CurrentObjective.Completed += HandleObjectiveCompleted;
        CurrentObjective.Register();

        CurrentObjectiveChanged?.Invoke(this, CurrentObjective);
    }

    private void HandleObjectiveCompleted(object sender, ObjectiveRuntime runtime)
    {
        runtime.Completed -= HandleObjectiveCompleted;
        runtime.Unregister();

        _currentObjectiveIndex++;

        if (_currentObjectiveIndex >= _objectives.Count)
        {
            CompleteMission();
            return;
        }

        RegisterCurrentObjective();
    }

    #endregion

    #region Mission

    private void CompleteMission()
    {
        if (IsCompleted) return;

        IsCompleted = true;
        MissionCompleted?.Invoke(this, this);
    }

    #endregion

    public ObjectiveRuntime CurrentObjective
        => _currentObjectiveIndex < _objectives.Count ? _objectives[_currentObjectiveIndex] : null;
}