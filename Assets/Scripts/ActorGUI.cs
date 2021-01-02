using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Handles updating the ActorGUI based on the current Actor.
public class ActorGUI : MonoBehaviour
{

    private ActorParticipant currentActor;

    private List<GameObject> resourceGUIs;

    private Text actorName;

    // Start is called before the first frame update
    void Start()
    {
        resourceGUIs = new List<GameObject>();
        actorName = gameObject.transform.Find("ActorName").GetComponent<Text>();
        ClearActor();
    }

    // Updates the current actor based on input, and sets up the GUI from the actor configuration.
    public void SetCurrentActor(ActorParticipant actor)
    {
        // Set the current actor
        currentActor = actor;

        // Initialize the Actor GUI based on the input actor.
        actorName.text = actor.participantName;

        foreach (Resource resource in actor.GetResourceList())
        {
            GameObject GUIObject = resource.GetGUIObject();

            //GUIObject.GetComponent<ResourceGUI>().SetResource(resource);

            resourceGUIs.Add(Instantiate(GUIObject, Vector3.zero, Quaternion.identity, transform));
            resourceGUIs[resourceGUIs.Count - 1].GetComponent<ResourceGUI>().Init();
            resourceGUIs[resourceGUIs.Count - 1].GetComponent<ResourceGUI>().SetResource(resource);

            GUIObject.GetComponent<ResourceGUI>().UpdateResourceGUI();
        }
    }

    // Clears the current actor and GUI
    public void ClearActor()
    {
        // Clear the current actor
        currentActor = null;

        // Clear all children of the actor GUI
        actorName.text = "";
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
                resourceGUI.GetComponent<ResourceGUI>().UpdateResourceGUI();
            }
        }
    }
}
