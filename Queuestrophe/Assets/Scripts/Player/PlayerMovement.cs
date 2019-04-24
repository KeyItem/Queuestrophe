using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : EntityMovement
{
    [Header("Player Movement Rotational Offset")]
    private Quaternion movementRotationOffset;
    
    private void Start()
    {
        InitializeMovement();
    }

    public override void InitializeMovement()
    {
        movementRotationOffset = Quaternion.Euler(0, Camera.main.transform.rotation.eulerAngles.y, 0);
        
        base.InitializeMovement();
    }

    public override void MoveEntity(Vector3 movement)
    {
        Debugger.DrawCustomDebugRay(transform.position, movement, Color.blue);
        
        base.MoveEntity(movement);
    }

    public override Vector3 ReturnMovementVelocity(Vector3 inputVector, EntityCollisionInfo collision)
    {
        Vector3 newMovementVelocity = movementRotationOffset * inputVector;
        
        newMovementVelocity = Vector3.ProjectOnPlane(newMovementVelocity, collision.groundInfo.averageGroundNormal);

        if (newMovementVelocity.magnitude > 1)
        {
            newMovementVelocity.Normalize();
        }

        entityCurrentMovementSmootVelocity = Mathf.SmoothDamp(entityCurrentMovementSmootVelocity, ReturnTargetMovementSpeed(inputVector), ref entityMovementSmoothVelocity, ReturnMovementSmoothTime(collision));

        newMovementVelocity *= entityCurrentMovementSmootVelocity;

        return newMovementVelocity;
    }

    public override Vector3 ReturnRotationVelocity(Vector3 inputVector, EntityCollisionInfo collisionInfo)
    {
        Vector3 newRotationVelocity = movementRotationOffset * inputVector;

        newRotationVelocity.Set(newRotationVelocity.x, newRotationVelocity.z, newRotationVelocity.y);
        
        return newRotationVelocity;
    }
}
