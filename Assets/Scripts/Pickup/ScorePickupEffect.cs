using System;
using UnityEngine;

[Serializable]
public class ScorePickupEffect : PickupEffect
{
    [SerializeField] private int score = 5; 
    
    public override void Apply(PickupReceiver receiver)
    {
        receiver.Score.Add(score);
        Debug.Log($"Pick up {score} score");
    }
}