using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [Header("Entity Movement Attributes")]
    public EntityMovementAttributes movementAttributes;

    [Header("Entity Movement Smoothing Attributes")]
    public EntityMovementSmoothingAttributes movementSmoothingAttributes;

    [HideInInspector]
    public float entityCurrentMovementSmootVelocity = 0f;
    [HideInInspector]
    public float entityMovementSmoothVelocity = 0f;

    [Header("Entity Rotation Smoothing Attributes")]
    public EntityRotationSmoothingAttributes rotationSmoothingAttributes;
    
    [HideInInspector]
    public float entityRotationSmoothVelocity = 0f;

    [Header("Entity Gravity Attributes")]
    public EntityGravityAttributes gravityAttributes;

    [HideInInspector]
    public Vector3 entityGravityVelocity;

    [Header("Entity Movement")]
    private Rigidbody rb;

    private void Start()
    {
        InitializeMovement();
    }

    public virtual void InitializeMovement()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void ManageEntityMovement(InputInfo input, EntityCollisionInfo collision)
    {        
        Vector3 movementVelocity = ReturnMovementVelocity(ReturnInputMovementVector(input), collision);

        //Vector3 gravityVelocity = ReturnGravityVelocity(collision);
        
        MoveEntity(movementVelocity);
    }
    public virtual void MoveEntity(Vector3 movement)
    {
        if (movement != Vector3.zero)
        {
            rb.MovePosition(rb.position + (movement * Time.fixedDeltaTime));
        }
    }

    public virtual void ManageEntityRotation(InputInfo input, EntityCollisionInfo collision)
    {
        Vector3 newRotation = ReturnRotationVelocity(ReturnInputRotationVector(input), collision);
        
        if (newRotation != Vector3.zero)
        {
            float newTargetRotation = Mathf.Atan2(newRotation.x, newRotation.y) * Mathf.Rad2Deg;
            float targetRotationSmoothing = 1 * Mathf.SmoothDampAngle(rb.rotation.eulerAngles.y, newTargetRotation, ref entityRotationSmoothVelocity, ReturnRotationSmoothTime(collision));
            
            Vector3 newRotationEulerAngles = new Vector3(rb.rotation.eulerAngles.x, targetRotationSmoothing, rb.rotation.eulerAngles.z);
            
            RotateEntity(Quaternion.Euler(newRotationEulerAngles));
        }
    }

    public virtual void RotateEntity(Quaternion rotation)
    {
        rb.rotation = rotation;
    }

    public float ReturnTargetMovementSpeed(Vector3 input)
    {
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        return input.magnitude * movementAttributes.baseMovementSpeed;
    }

    public float ReturnMovementSmoothTime(EntityCollisionInfo collision)
    {
        if (collision.isGrounded)
        {
            return movementSmoothingAttributes.groundedMovementSmoothing;
        }
        else
        {
            return movementSmoothingAttributes.airborneMovementSmoothing;
        }
    }

    public float ReturnRotationSmoothTime(EntityCollisionInfo collision)
    {
        if (collision.isGrounded)
        {
            return rotationSmoothingAttributes.groundedRotationSmoothing;
        }
        else
        {
            return rotationSmoothingAttributes.airborneRotationSmoothing;
        }
    }
    public virtual Vector3 ReturnMovementVelocity(Vector3 inputVector, EntityCollisionInfo collision)
    {
        Vector3 newMovementVelocity = Vector3.ProjectOnPlane(inputVector, collision.groundInfo.averageGroundNormal);

        if (newMovementVelocity.magnitude > 1)
        {
            newMovementVelocity.Normalize();
        }

        entityCurrentMovementSmootVelocity = Mathf.SmoothDamp(entityCurrentMovementSmootVelocity, ReturnTargetMovementSpeed(inputVector), ref entityMovementSmoothVelocity, ReturnMovementSmoothTime(collision));

        newMovementVelocity *= entityCurrentMovementSmootVelocity;

        return newMovementVelocity;
    }

    public virtual Vector3 ReturnRotationVelocity(Vector3 inputVector, EntityCollisionInfo collisionInfo)
    {
        Vector3 newRotationVelocity = inputVector;

        return newRotationVelocity;
    }

    public virtual Vector3 ReturnGravityVelocity(EntityCollisionInfo collision)
    {
        if (collision.isGrounded)
        {
            entityGravityVelocity = Vector3.zero;
            
            return Vector3.zero;
        }
        else
        {
            entityGravityVelocity += gravityAttributes.gravityVelocity * Time.fixedDeltaTime;

            return entityGravityVelocity;
        }
    }
    public virtual Vector3 ReturnInputMovementVector(InputInfo input)
    {
        float xAxis = input.ReturnCurrentAxis("HorizontalAxis").axisValue;
        float yAxis = input.ReturnCurrentAxis("VerticalAxis").axisValue;
        float zAxis = 0f;

        return new Vector3(xAxis, zAxis, yAxis);
    }

    public virtual Vector3 ReturnInputRotationVector(InputInfo input)
    {
        float xAxis = input.ReturnCurrentAxis("HorizontalAxis").axisValue;
        float yAxis = input.ReturnCurrentAxis("VerticalAxis").axisValue;
        float zAxis = 0f;

        return new Vector3(xAxis, zAxis, yAxis);
    }
}

[System.Serializable]
public struct EntityMovementAttributes
{
    [Header("Entity Movement Speed Attributes")]
    public float baseMovementSpeed;
}

[System.Serializable]
public struct EntityMovementSmoothingAttributes
{
    [Header("Entity Movement Smoothing Attributes")]
    [Range(0, 1)]
    public float groundedMovementSmoothing;
    [Range(0, 1)]
    public float airborneMovementSmoothing;
}

[System.Serializable]
public struct EntityRotationSmoothingAttributes
{
    [Header("Entity Movement Smoothing Attributes")]
    [Range(0, 1)]
    public float groundedRotationSmoothing;
    [Range(0, 1)]
    public float airborneRotationSmoothing;
}

[System.Serializable]
public struct EntityGravityAttributes
{
    [Header("Entity Gravity Attributes")]
    public Vector3 gravityVelocity;
}


