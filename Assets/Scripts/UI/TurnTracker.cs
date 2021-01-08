using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class TurnTracker : MonoBehaviour
{
    // Contains the list of participants in the tracker
    private List<Participant> participants;

    // A list containing references participant name text components
    private List<Text> participantNames = new List<Text>();

    // Contains the index of the current participant.
    private int currentParticipant;

    void Start()
    {
        participants = new List<Participant>();

        // Initialize the TurnTracker
        InitTurnTracker();

        // Start the first turn
        currentParticipant = -1;
        NextTurn();
    }

    // Starts the next player's turn
    private void NextTurn()
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

    // Adds a nameplate to the TurnTracker for a given actor
    private void AddParticipant(Participant participant)
    {
        float nameheight = 15f;
        float namedist = 0f;

        // First, create and update the text gameobject
        // Instantiate and setup the participant nameplate
        GameObject text_object = UI_Constructors.AddTextObject(participant.name, nameheight,
                        gameObject.transform, TextAnchor.MiddleLeft, participant.name + "_nameplate");
        Text nameplate = text_object.GetComponent<Text>();
        RectTransform nameplate_tfm = text_object.GetComponent<RectTransform>();

        // Add the participant to the turnorder
        participants.Add(participant);
        participantNames.Add(nameplate);
        int pid = participants.Count - 1;

        // Update the nameplate position based on location in turnorder
        float y_coord = -((float)pid + 0.5f) * nameheight + pid * namedist;
        nameplate_tfm.anchoredPosition = new Vector3(0, y_coord, 0);
    }

    // Initializes the TurnTracker GUI
    void InitTurnTracker()
    {
        // Clear any existing GameObjects in the canvas
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        // Add all participants in the scene to the turn order.
        foreach (Participant P in FindObjectsOfType<Participant>())
        {
            AddParticipant(P);
        }
    }
}
