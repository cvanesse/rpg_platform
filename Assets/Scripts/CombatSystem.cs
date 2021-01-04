using UnityEngine;
using System.Collections;


// Creates and starts the TurnTracker
public class CombatSystem : MonoBehaviour
{

    public GameObject GUICanvas;

    public GameObject turnTrackerObject;
    public int turnTrackerX;
    public int turnTrackerY;

    public GameObject actorGUIObject;
    public int actorGUIX;
    public int actorGUIY;

    public void Start()
    {
        AddNewUIElement(actorGUIObject, new Vector2(actorGUIX, actorGUIY));

        StartCoroutine("StartTurnTracker");
    }

    private void AddNewUIElement(GameObject obj, Vector2 position)
    {
        obj = Instantiate(obj, Vector3.zero, Quaternion.identity, GUICanvas.transform);
        obj.GetComponent<RectTransform>().anchoredPosition = (Vector3)position;
    }

    // Creates and Starts the TurnTracker
    private IEnumerator StartTurnTracker()
    {
        AddNewUIElement(turnTrackerObject, new Vector2(turnTrackerX, turnTrackerY));
        yield return turnTrackerObject;
    }

}