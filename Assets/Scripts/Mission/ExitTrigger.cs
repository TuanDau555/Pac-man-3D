using System;
using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PickupReceiver>(out _))
            return;

        GameplayEvents.RaiseExitReached(this, EventArgs.Empty);
    }
}