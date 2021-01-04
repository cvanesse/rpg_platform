using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles updating the ActorGUI based on the current Actor.
public class ActorGUI : MonoBehaviour
{
    // For storing the current actor
    private ActorParticipant currentActor;

    // Holds a list of resourceGUIs for updating them on every frame
    private List<GameObject> resourceGUIs;

    // The nameplate of the actorName
    private GameObject actorNameplate;

    // Start is called before the first frame update
    void Start()
    {
        resourceGUIs = new List<GameObject>();
        ClearActor();
    }

    // Updates the current actor based on input, and sets up the GUI from the actor configuration.
    public void SetCurrentActor(ActorParticipant actor)
    {
        // Set the current actor
        currentActor = actor;

        // Initialize the Actor GUI based on the input actor.
        float height = 30f;
        actorNameplate = UI_Constructors.AddTextObject(
            actor.participantName, height, gameObject.transform, anchorTop: false
        );
        actorNameplate.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, height / 2 + 5);

        foreach (Resource resource in actor.GetResourceList())
        {
            GameObject GUIObject = resource.GetGUIObject();

            GUIObject = Instantiate(GUIObject, Vector3.zero, Quaternion.identity, transform);

            GUIObject.GetComponent<ResourceGUIInterface>().Init();
            GUIObject.GetComponent<ResourceGUIInterface>().SetResource(resource);
            GUIObject.GetComponent<ResourceGUIInterface>().UpdateResourceGUI();

            resourceGUIs.Add(GUIObject);
        }
    }

    // Clears the current actor and GUI
    public void ClearActor()
    {
        // Clear the current actor
        currentActor = null;

        // Clear all children of the actor GUI
        Destroy(actorNameplate);
        foreach (GameObject resourceGUI in resourceGUIs)
        {
            Destroy(resourceGUI);
        }
        resourceGUIs.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentActor != null)
        {
            // Tell every resource GUI to update.
            foreach (GameObject resourceGUI in resourceGUIs)
            {
                resourceGUI.GetComponent<ResourceGUIInterface>().UpdateResourceGUI();
            }
        }
    }
}
