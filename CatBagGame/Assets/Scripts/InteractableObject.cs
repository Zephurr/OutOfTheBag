using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public bool hasBeenInteracted = false;

    public InteractableObject()
    {

    }

    public virtual void Interact()
    {
        hasBeenInteracted = false;
    }
}
