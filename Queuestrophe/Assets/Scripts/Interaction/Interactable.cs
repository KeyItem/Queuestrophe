using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Interaction Attributes")]
    public bool canBeInteractedWith = true;
    
    public virtual void Interact(GameObject interacter)
    {
        Debug.Log("I am being interacted with! :: " + gameObject.name);
    }
}
