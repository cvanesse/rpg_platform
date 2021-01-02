using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnTracker : MonoBehaviour
{
    // A prefab containing the name text for each participant
    [Tooltip("The prefab for the name text.")]
    public GameObject nameText;

    // Contains the list of participants in the tracker
    // private List<Participant> participants;
    private List<Participant> participants;

    // Contains the index of the current participant.
    private int currentParticipant;


    // Information about placing the name strings
    private const float nameDist = 0f;
    private float nameHeight;
    private float top;

    // A list containing references participant name text components
    private List<Text> participantNames = new List<Text>();


    // Start is called before the first frame update
    void Start()
    {
        // Enumerate the participants
        participants = new List<Participant>(FindObjectsOfType<Participant>());

        // Initialize the turntracker GUI
        InitTurnTrackerGUI();

        // Start the first turn
        currentParticipant = -1;
        NextTurn();
    }

    void NextTurn()
    {
        // Iterate the participant index
        currentParticipant++;
        currentParticipant = currentParticipant % participants.Count;

        // Update the GUI according to who's turn it is.
        participantNames[currentParticipant].fontStyle = FontStyle.Bold;

        // Tell the next participant that it's their turn
        participants[currentParticipant].StartTurn();
    }

    // Ends the current participants turn and moves to the next participant
    public IEnumerator EndTurn()
    {
        yield return new WaitForFixedUpdate();
        participantNames[currentParticipant].fontStyle = FontStyle.Normal;
        NextTurn();
    }

    void InitTurnTrackerGUI()
    {
        nameHeight = nameText.GetComponent<RectTransform>().rect.height;
        top = -GetComponent<RectTransform>().rect.height / 2;

        // Clear any existing GameObjects in the canvas
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Loop through each participant, get their name, and put it in the turntracker GUI
        for (int pid = 0; pid < participants.Count; pid++)
        {
            var P = participants[pid];

            // Get the appropriate y-offset for the name
            float y_coord = top + ((float)pid + 0.5f) * nameHeight + pid * nameDist;

            // Instantiate the name prefab as a child of this object, and add it to the list of names
            var P_name = Instantiate(nameText, Vector3.zero, Quaternion.identity, transform);
            P_name.transform.localPosition = new Vector3(0, -y_coord, 0);
            P_name.GetComponent<Text>().text = participants[pid].participantName;
            participantNames.Add(P_name.GetComponent<Text>());
        }
    }
}
