using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityController : MonoBehaviour
{
    [Header("Entity Controller Base Attributes")]
    public bool isEntityAwake = true;

    [Header("Entity Info")]
    public EntityInfo entityInfo;

    [Header("Entity Input")]
    public EntityInput input;

    [Space(10)]
    [HideInInspector]
    public InputInfo inputValues;

    [Space(10)]
    public bool isEntityCapturingInput = true;

    [Header("Entity Movement")]
    public EntityMovement movement;

    [Space(10)]
    public bool isEntityCapturingMovement = true;

    [Header("Entity Collision")]
    public EntityCollision collision;

    [HideInInspector]
    public EntityCollisionInfo collisionInfo;

    [Space(10)]
    public bool isEntityCapturingCollisions = true;

    [Header("Entity Interaction")]
    public EntityInteraction interaction;

    [Space(10)]
    public bool isEntityCapturingInteraction = true;

    private void Start()
    {
        InitializeEntity();
    }

    private void Update()
    {
        if (isEntityAwake)
        {
            ManageInput();
            ManageCollision();
            ManageInteraction();
        } 
    }

    private void FixedUpdate()
    {
        if (isEntityAwake)
        {
            ManageMovement();
        }
    }

    public virtual void InitializeEntity()
    {
        input = GetComponent<EntityInput>();
        movement = GetComponent<EntityMovement>();
        collision = GetComponent<EntityCollision>();
        interaction = GetComponent<EntityInteraction>();
    }

    public virtual void ManageInput()
    {
        if (isEntityCapturingInput)
        {
            input.GetInput();

            inputValues = input.ReturnInput();
        }      
    }

    public virtual void ManageMovement()
    {
        if (isEntityCapturingMovement)
        {
            movement.ManageEntityRotation(inputValues, collisionInfo);
            movement.ManageEntityMovement(inputValues, collisionInfo);
        }
    }

    public virtual void ManageCollision()
    {
        if (isEntityCapturingCollisions)
        {
            collisionInfo = collision.ReturnCollisionInfo();
        }
    }

    public virtual void ManageInteraction()
    {
        if (isEntityCapturingInteraction)
        {
            interaction.ManageInteraction(inputValues, collisionInfo);
        }
    }
}

[System.Serializable]
public struct EntityInfo
{
    [Header("Base Entity Info")]
    public string entityName;

    [Space(10)]
    public ENTITY_TYPE entityType;

    [Space(10)]
    public GameObject entityObject;

    public EntityInfo (string newEntityName, ENTITY_TYPE newEntityType, GameObject newEntityObject)
    {
        this.entityName = newEntityName;
        this.entityType = newEntityType;
        this.entityObject = newEntityObject;
    }
}

public enum ENTITY_TYPE
{
    NONE,
    PLAYER,
    ENEMY
}
