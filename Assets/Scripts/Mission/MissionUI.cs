using System;
using UnityEngine;

public class MissionUI : MonoBehaviour
{
    [SerializeField] private MissionView view;

    private MissionRuntime _mission;
    private ObjectiveRuntime _objective;

    private void OnDestroy()
    {
        if (_mission != null)
            _mission.CurrentObjectiveChanged -= HandleObjectiveChanged;

        if (_objective != null)
        {
            _objective.ProgressChanged -= HandleProgressChanged;
            _objective.Completed -= HandleCompleted;
        }
    }

    #region Bind

    public void Bind(MissionRuntime runtime)
    {

        _mission = runtime;

        _mission.CurrentObjectiveChanged += HandleObjectiveChanged;

        HandleObjectiveChanged(this, _mission.CurrentObjective);
    }

    #endregion

    #region Events

    private void HandleObjectiveChanged(object sender, ObjectiveRuntime runtime)
    {
        if (_objective != null)
        {
            _objective.ProgressChanged -= HandleProgressChanged;
            _objective.Completed -= HandleCompleted;
        }

        _objective = runtime;

        if (_objective == null)
            return;

        _objective.ProgressChanged += HandleProgressChanged;
        _objective.Completed += HandleCompleted;

        Refresh();
    }

    private void HandleCompleted(object sender, ObjectiveRuntime runtime)
    {
        Refresh();
    }

    private void HandleProgressChanged(object sender, ObjectiveRuntime runtime)
    {
        Refresh();
    }

    #endregion

    private void Refresh()
    {
        view.SetTitle(_objective.Data.Title);

        view.SetCurrentProgress(_objective.CurrentProgress);
        view.SetTargetProgress(_objective.TargetProgress);

        view.SetDescription(_objective.Data.Description, _objective.CurrentProgress, _objective.TargetProgress);


        if (_objective.IsCompleted)
        {
            view.ShowCompleted();
        }
    }
}