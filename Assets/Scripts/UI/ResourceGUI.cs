using UnityEngine;

// Interface for generally communiating with ResourceGUIs
public interface ResourceGUIInterface
{
    void SetResource(Resource resource);
    void Init();
    void UpdateResourceGUI();
}

// Resource GUIs must be defined in conjunction with a ResourceType.
public class ResourceGUI<ResourceType> : MonoBehaviour, ResourceGUIInterface where ResourceType : Resource
{
    // The resource that the GUI is linked to.
    protected ResourceType resource;

    public virtual void Init() { }

    public void SetResource(Resource newResource)
    {
        gameObject.SetActive(true);
        resource = (ResourceType)newResource; // Cast to ResourceType on storage.
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