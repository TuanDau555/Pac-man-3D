
using System;

[Serializable]
public abstract class PickupEffect
{
    public abstract void Apply(PickupReceiver receiver);
}