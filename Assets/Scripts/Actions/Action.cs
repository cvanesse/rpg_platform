using UnityEngine;
using System.Collections.Generic;

// The action class handles creation and annihilation of components which can be created by an action
//  ie - for rendering, other GUI elements, etc.
public class Action : MonoBehaviour
{

    // The list for tracking child behaviors for removing them on destruction
    private List<Component> childComponents;

    // List for tracking child objects so we can remove them on desctruction
    private List<GameObject> childObjects;

    public virtual void Start()
    {
        childComponents = new List<Component>();
        childObjects = new List<GameObject>();
    }

    // Adds a new component to the parent object, while tracking them in a list of behaviours for removal later.
    protected T AddChildBehaviour<T>() where T : Component
    {
        var compRef = gameObject.AddComponent<T>();
        childComponents.Add(compRef);
        return compRef;
    }

    protected GameObject AddChildObject(UnityEngine.Object obj)
    {
        var objRef = (GameObject)Instantiate(obj, gameObject.transform);
        childObjects.Add(objRef);
        return objRef;
    }

    // Instantiate a new GameObject and keep track of it in the list of objects
    protected GameObject AddChildObject(GameObject obj)
    {
        var objRef = Instantiate(obj, gameObject.transform);
        childObjects.Add(objRef);
        return objRef;
    }

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