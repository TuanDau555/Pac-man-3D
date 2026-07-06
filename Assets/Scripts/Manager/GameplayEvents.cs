using System;
using UnityEngine;

public static class GameplayEvents
{
    public static event EventHandler<PickupDataSO> OnPickupColleted;
    public static event EventHandler<GameObject> EnemyKilled;
    public static event EventHandler<int> OnScoreChanged;
    public static event EventHandler OnExitReached;

    public static void RaisePickUpCollected(object sender, PickupDataSO pickup)
    {
        OnPickupColleted?.Invoke(sender, pickup);
        Debug.Log("[GameplayEvents] Pick Up Collected");
    }

    public static void RaiseEnemyKilled(object sender, GameObject enemy)
    {
        EnemyKilled?.Invoke(sender, enemy);
    }

    public static void RaiseScoreChanged(object sender, int score)
    {
        OnScoreChanged?.Invoke(sender, score);
    }

    public static void RaiseExitReached(object sender, EventArgs e)
    {
        OnExitReached?.Invoke(sender, EventArgs.Empty);
    }
}