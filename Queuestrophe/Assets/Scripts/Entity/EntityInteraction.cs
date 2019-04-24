using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInteraction : MonoBehaviour
{
    [Header("Entity Interaction Check Attributes")]
    public EntityInteractionCheckAttributes checkAttributes;

    public virtual void ManageInteraction(InputInfo input, EntityCollisionInfo collision)
    {
        if (CheckForInteractionInput(input))
        {
            if (CheckForInteractionObjects())
            {
                Interactable newInteractable = ReturnInteractable();
                
                newInteractable.Interact(gameObject);
            }
        }
    }

    public virtual bool CheckForInteractionInput(InputInfo input)
    {
        return input.ReturnButtonReleaseState("Interact");
    }

    public virtual bool CheckForInteractionObjects()
    {
        Collider[] interactColliders = Physics.OverlapSphere(transform.position, checkAttributes.interactionCheckRadius, checkAttributes.interactionMask);

        for (int i = 0; i < interactColliders.Length; i++)
        {
            Vector3 interceptVector = interactColliders[i].transform.position - transform.position;
            float interceptAngle = Vector3.Angle(interceptVector, transform.forward);
            
            if (interceptAngle < checkAttributes.interactionCheckMaxAngle)
            {
                return true;
            }       
                        
            Debug.Log("Missed Interaction because angle != maxAngle :: " + interceptAngle);
        }

        return false;
    }

    public virtual Interactable ReturnInteractable()
    {
        Collider[] interactColliders = Physics.OverlapSphere(transform.position, checkAttributes.interactionCheckRadius, checkAttributes.interactionMask);
        
        for (int i = 0; i < interactColliders.Length; i++)
        {
            Vector3 interceptVector = interactColliders[i].transform.position - transform.position;
            
            if (Vector3.Angle(interceptVector, transform.forward) < checkAttributes.interactionCheckMaxAngle)
            {
                Interactable newInteractable = interactColliders[i].GetComponent<Interactable>();

                if (newInteractable.canBeInteractedWith)
                {
                    return newInteractable;
                }
            }
        }

        return null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, checkAttributes.interactionCheckRadius);

        Gizmos.color = Color.blue;
    }
}

[System.Serializable]
public struct EntityInteractionCheckAttributes
{
    [Header("Entity Interaction Check Attributes")]
    public float interactionCheckRadius;

    [Space(10)]
    public float interactionCheckMaxAngle;

    [Space(10)]
    public LayerMask interactionMask;
}
