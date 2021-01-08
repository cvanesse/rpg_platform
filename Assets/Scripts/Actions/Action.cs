using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A base class for Actions performed by an ActorParticipant.
/// Handles creation, tracking, and cleanup of child gameObjects and Components.
/// </summary>
public class Action : MonoBehaviour
{
    /// <summary>
    /// The list for tracking child behaviours so they can be removed on destruction
    /// </summary>
    private List<Component> childComponents;

    /// <summary>
    /// The list for tracking child objects so they can be removed on destruction.
    /// </summary>
    private List<GameObject> childObjects;

    /// <summary>
    /// A reference to the actor that started this action behaviour
    /// </summary>
    protected ActorParticipant actor;

    /// <summary>
    /// On creation, the actor initializes the childComponents and childObjects lists,
    /// and gets the reference to the actor.
    /// </summary>
    public virtual void Start()
    {
        childComponents = new List<Component>();
        childObjects = new List<GameObject>();
        actor = gameObject.GetComponent<ActorParticipant>();
    }

    /// <summary>
    /// Adds a new component to the participant gameObject which created this action, 
    /// and stores a reference to it in the list of child components.
    /// </summary>
    /// <typeparam name="T">The Monobehaviour type to create.</typeparam>
    /// <returns>A reference to the new component</returns>
    protected T AddChildBehaviour<T>() where T : Component
    {
        var compRef = gameObject.AddComponent<T>();
        childComponents.Add(compRef);
        return compRef;
    }

    /// <summary>
    /// Adds an existing component to the list of children.
    /// This is useful when components are added to other objects (like Target,)
    /// but still need to be destroyed when the action ends.
    /// </summary>
    /// <param name="comp">A reference to the component which should be listed as a child.</param>
    protected void ListChildBehaviour(Component comp)
    {
        childComponents.Add(comp);
    }

    /// <summary>
    /// Instantiates a new gameObject using an Object/Prefab as reference as a child of the participant gameObject in the hierarchy,
    /// and adds it to the list of child gameObjects so it can be destroyed on destruction
    /// of the Action.
    /// </summary>
    /// <param name="obj">A reference to the object/prefab which should be instantiated</param>
    /// <returns>A reference to the instantiated object.</returns>
    protected GameObject AddChildObject(UnityEngine.Object obj)
    {
        var objRef = (GameObject)Instantiate(obj, gameObject.transform);
        childObjects.Add(objRef);
        return objRef;
    }

    /// <summary>
    /// Instantiates a new empty gameObject with the given name as a child of the participant
    /// in the hierarchy, and adds it to the list of child gameObjects so it can be destroyed
    /// on destruction of the Action.
    /// </summary>
    /// <param name="obj_name">The name of the empty gameObject</param>
    /// <returns>A reference to the instantiated object.</returns>
    protected GameObject AddChildObject(string obj_name)
    {
        var objRef = new GameObject(obj_name);
        objRef.transform.parent = transform;
        objRef.transform.position = transform.position;
        childObjects.Add(objRef);
        return objRef;
    }

    /// <summary>
    /// Instantiates a clone of an existing GameObject as a child of the participant object,
    /// and adds it to the list of child gameObjects so it can be destroyed on destruction
    /// of the Action 
    /// </summary>
    /// <param name="obj">A reference to the gameObject to clone</param>
    /// <returns>A reference to the new gameObject</returns>
    protected GameObject AddChildObject(GameObject obj)
    {
        var objRef = Instantiate(obj, gameObject.transform);
        childObjects.Add(objRef);
        return objRef;
    }

    /// <summary>
    /// Best practice: Objects should never destroy themselves.
    /// When actions are completed - they need to tell the actor that they are done, and the actor can update their tracking and destroy the action.
    /// </summary>
    protected void FinishAction() {
        actor.EndAction();
    }

    /// <summary>
    /// On destruction, Actions need to be removed from the list of Actions
    /// in the ActorParticipant Behaviour, and all of their children need to be destroyed.
    /// This loops through all of the child objects and destroys all children.
    /// </summary>
    public virtual void OnDestroy()
    {
        foreach (Component comp in childComponents)
        {
            Destroy(comp);
        }
        foreach (GameObject obj in childObjects)
        {
            Destroy(obj);
        }
    }
}