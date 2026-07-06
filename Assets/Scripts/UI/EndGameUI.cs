using System;
using DG.Tweening;
using UnityEngine;

public class EndGameUI : MonoBehaviour
{
    [Header("Canvas Ref")]
    [SerializeField] private CanvasGroup endGameCanvas;

    [Header("Animation")]
    [SerializeField] private float fadeDuration = 0.35f;

    private Tween currentTween;

    #region Execute

    private void Start()
    {
        GameplayEvents.OnExitReached += HandleOnExit;
        Hide();
    }

    private void OnDisable()
    {
        GameplayEvents.OnExitReached -= HandleOnExit;
        currentTween?.Kill();
    }

    #endregion

    #region Events

    private void HandleOnExit(object sender, EventArgs e)
    {
        Show();
    }

    #endregion

    private void Hide()
    {
        currentTween?.Kill();
        endGameCanvas.alpha = 0;
        endGameCanvas.interactable = false;
        endGameCanvas.blocksRaycasts = false;
    }

    private void Show()
    {
        currentTween?.Kill();
        endGameCanvas.interactable = true;
        endGameCanvas.blocksRaycasts = true;

        currentTween = endGameCanvas
            .DOFade(1f, fadeDuration)
            .From(0f)
            .SetUpdate(true)
            .OnComplete(() =>
            {
                Time.timeScale = 0f;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            });

    }
}