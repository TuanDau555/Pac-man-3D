using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PickupDataSO")]
public class PickupDataSO : ScriptableObject
{
    [SerializeReference, SubclassSelector]
    private List<PickupEffect> effects = new();

    public void Apply(PickupReceiver receiver)
    {
        foreach(var effects in effects)
        {
            effects.Apply(receiver);
        }
    }
}