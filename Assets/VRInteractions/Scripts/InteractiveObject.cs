using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InteractiveObject : MonoBehaviour
{
    public virtual void OnTouchStart()
    {
    }

    public virtual void OnTouchEnd()
    {
    }

    public virtual bool OnGrabbed(Grabber hand)
    {
        return false;
    }

    public virtual void OnDropped()
    {
    }

    public virtual void OnTriggerStart()
    {
    }

    public virtual void OnTriggerEnd()
    {
    }
}
