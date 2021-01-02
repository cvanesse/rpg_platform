using UnityEngine;
using System.Collections;


// Creates and starts the TurnTracker
public class CombatSystem : MonoBehaviour
{

    public GameObject turnTrackerObject;
    public int turnTrackerX;
    public int turnTrackerY;

    public GameObject actorGUIObject;
    public int actorGUIX;
    public int actorGUIY;

    public void Start()
    {
        createActorGUI();

        StartCoroutine("StartTurnTracker");
    }

    private void createActorGUI()
    {
        var guitf = FindObjectOfType<GUICanvas>().transform;
        actorGUIObject = Instantiate(actorGUIObject, Vector3.zero, Quaternion.identity, guitf);
        actorGUIObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(actorGUIX, actorGUIY, 0);
    }

    // Creates and Starts the TurnTracker
    private IEnumerator StartTurnTracker()
    {
        var guitf = FindObjectOfType<GUICanvas>().transform;
        turnTrackerObject = Instantiate(turnTrackerObject, Vector3.zero, Quaternion.identity, guitf);
        turnTrackerObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(turnTrackerX, turnTrackerY, 0);
        yield return turnTrackerObject;
    }

}