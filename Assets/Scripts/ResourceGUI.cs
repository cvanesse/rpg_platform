using UnityEngine;


public class ResourceGUI : MonoBehaviour
{

    // The resource that the GUI is linked to.
    protected Resource resource;

    public virtual void Init() { }

    public void SetResource(Resource newResource)
    {
        gameObject.SetActive(true);
        resource = newResource;
        InitResourceGUI();
    }

    // For initializing the Resource GUI after a resource is added
    protected virtual void InitResourceGUI() { }

    // Updates the resource GUI based on the information stored about the resource.
    public virtual void UpdateResourceGUI()
    {
        if (resource == null)
        {
            gameObject.SetActive(false);
        }
    }

}