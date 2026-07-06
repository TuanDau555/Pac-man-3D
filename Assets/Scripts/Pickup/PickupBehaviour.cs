using UnityEngine;

public class PickupBehaviour : MonoBehaviour
{
    [SerializeField] private PickupDataSO data;

    private void OnTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<PickupReceiver>(out var receiver)) return;

        data.Apply(receiver);

        GameplayEvents.RaisePickUpCollected(this, data);

        Destroy(gameObject); // will have a object pool to handle this 26/6
    }
    
}