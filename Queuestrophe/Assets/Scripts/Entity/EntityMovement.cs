using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMovement : MonoBehaviour
{
    [Header("Entity Movement Attributes")]
    public EntityMovementAttributes movementAttributes;

    [Header("Entity Movement Smoothing Attributes")]
    public EntityMovementSmoothingAttributes movementSmoothingAttributes;

    private float entityCurrentMovementSmootVelocity = 0f;
    private float entityMoveSmoothVelocity = 0f;

    [Header("Entity Movement")]
    private Rigidbody rb;

    private void Start()
    {
        InitializeMovement();
    }

    private void InitializeMovement()
    {
        rb = GetComponent<Rigidbody>();
    }

    public virtual void MoveEntity(InputInfo input, EntityCollisionInfo collision)
    {
        Vector3 movementVelocity = Vector3.zero;
        
        Vector3 inputVector = ReturnInputMovementVector(input);

        movementVelocity = ReturnMovementVelocity(inputVector, collision);

        if (movementVelocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + movementVelocity * Time.fixedDeltaTime);
        }
    }

    private float ReturnTargetMovementSpeed(Vector3 input)
    {
        if (input.sqrMagnitude > 1)
        {
            input.Normalize();
        }

        return input.magnitude * movementAttributes.entityBaseMovementSpeed;
    }

    private float ReturnMovementSmoothTime(EntityCollisionInfo collision)
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
    private Vector3 ReturnMovementVelocity(Vector3 inputVector, EntityCollisionInfo collision)
    {
        Vector3 newMovementVelocity = Vector3.zero;

        newMovementVelocity = Vector3.ProjectOnPlane(inputVector, collision.groundInfo.averageGroundNormal);

        if (newMovementVelocity.magnitude > 1)
        {
            newMovementVelocity.Normalize();
        }

        entityCurrentMovementSmootVelocity = Mathf.SmoothDamp(entityCurrentMovementSmootVelocity, ReturnTargetMovementSpeed(inputVector), ref entityMoveSmoothVelocity, ReturnMovementSmoothTime(collision));

        newMovementVelocity *= entityCurrentMovementSmootVelocity;

        return newMovementVelocity;
    }
    private Vector3 ReturnInputMovementVector(InputInfo input)
    {
        float xAxis = input.ReturnCurrentAxis("HorizontalAxis").axisValue;
        float yAxis = 0f;
        float zAxis = input.ReturnCurrentAxis("VerticalAxis").axisValue;

        return new Vector3(xAxis, yAxis, zAxis);
    }

    private Vector3 ReturnInputRotationVector(InputInfo input)
    {
        float xAxis = input.ReturnCurrentAxis("HorizontalAxis").axisValue;
        float yAxis = 0f;
        float zAxis = input.ReturnCurrentAxis("VerticalAxis").axisValue;

        return new Vector3(xAxis, yAxis, zAxis);
    }
}

[System.Serializable]
public struct EntityMovementAttributes
{
    [Header("Entity Movement Speed Attributes")]
    public float entityBaseMovementSpeed;
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

