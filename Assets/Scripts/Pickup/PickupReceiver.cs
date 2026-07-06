using UnityEngine;

/// <summary>
/// Attach this script to player
/// </summary>
public class PickupReceiver : MonoBehaviour
{
    [SerializeField] private ScoreManager scoreManager;
    
    public PlayerHealthController Health { get; private set; }

    public ScoreManager Score => scoreManager;
}