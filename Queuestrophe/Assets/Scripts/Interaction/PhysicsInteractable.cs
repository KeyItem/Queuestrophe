using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsInteractable : Interactable
{
    public override void Interact(GameObject interacter)
    {
        Vector3 interceptVector = transform.position - interacter.transform.position;
        interceptVector.y = -interceptVector.y;
        
        AddForce(interceptVector.normalized);
        
        base.Interact(interacter);
    }

    private void AddForce(Vector3 forceDirection)
    {
        Rigidbody myRB = GetComponent<Rigidbody>();

        myRB.AddForce(forceDirection * 50f, ForceMode.Impulse);
    }
}
