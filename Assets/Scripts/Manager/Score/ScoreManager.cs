using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region Parameters

    public int Score { get; private set; }

    #endregion

    public event EventHandler<int> OnScoreChanged;

    public void Add(int amount)
    {
        Score += amount;
        OnScoreChanged?.Invoke(this, Score);

        GameplayEvents.RaiseScoreChanged(this, Score);
    }
}