using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserInteraction : IInputUpdate<Dictionary<Collider2D, List<RaycastHit2D>>>, IDisable
{
    private LaserBase laser;
    private Dictionary<Collider2D, InteractedObject> interactedObjects;
    private List<Collider2D> objectsToExit;

    public LaserInteraction(LaserBase laser)
    {
        this.laser = laser;
        interactedObjects = new();
        objectsToExit = new();
    }

    public void Update(Dictionary<Collider2D, List<RaycastHit2D>> hits)
    {
        Enter(hits);
        Stay(hits);
        Exit();
    }
    public void Disable()
    {
        foreach(InteractedObject interactedObject in interactedObjects.Values)
        {
            interactedObject.OnExited();
        }
        interactedObjects.Clear();
    }

    public void Enter(Dictionary<Collider2D, List<RaycastHit2D>> hits)
    {
        foreach(Collider2D collider in hits.Keys)
        {
            if (interactedObjects.ContainsKey(collider) == false)
            {
                interactedObjects[collider] = new InteractedObject(laser, collider.transform);
                interactedObjects[collider].OnEntered(hits[collider]);
            }
        }      
    }

    public void Stay(Dictionary<Collider2D, List<RaycastHit2D>> hits)
    {
        objectsToExit.Clear();
        foreach(Collider2D collider in hits.Keys)
        {
            // 如果当前Collider在interactedObjects有记录，则继续执行
            if (interactedObjects.TryGetValue(collider, out InteractedObject value))
            {
                interactedObjects[collider]?.OnStay(hits[collider]);
            }
            else
            {
                objectsToExit.Add(collider);
            }
            
        }
    }

    public void Exit()
    {
        foreach(Collider2D  collider in objectsToExit)
        {
            interactedObjects[collider].OnExited();
            interactedObjects.Remove(collider);
        }
    }

}
