using TMPro;
using UnityEngine;

public class MissionView : MonoBehaviour
{
    #region Parameters

    [Header("UI Ref")]
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;

    private int _currentProgress;
    private int _targetProgress;

    #endregion

    #region Setup

    public void SetTitle(string title)
    {
        titleText.text = title;
    }

    public void SetDescription(string description, int currentProgress, int targetProgress)
    {
        descriptionText.text = description + $" {currentProgress}/{targetProgress}";
    }

    public void ShowCompleted()
    {
        descriptionText.text = "Completed";
    }

    public void SetCurrentProgress(int currentProgress)
    {
        _currentProgress = currentProgress;
    }

    public void SetTargetProgress(int targetProgress)
    {
        _targetProgress = targetProgress;
    }

    #endregion
}