using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// Takes care of tracking the turn order for participants. Tells actors when it's their turn, and listens for when they're done.
/// </summary>
public class TurnTracker : MonoBehaviour
{   
    /// <summary>
    /// The list of participants in the tracker
    /// </summary>
    private List<Participant> participants;

    /// <summary>
    /// A list containing references to the participant nameplates in the turnorder GUI.
    /// </summary>
    private List<Text> participantNames = new List<Text>();

    /// <summary>
    /// An integer containing the ID of the current participant in the 'participants' list.
    /// </summary>
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

    /// <summary>
    /// Starts the next player's turn
    /// </summary>
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

    /// <summary>
    /// Used as a callback function by participants to advance the turnorder.
    /// </summary>
    public IEnumerator EndTurn()
    {
        yield return new WaitForFixedUpdate();
        participantNames[currentParticipant].fontStyle = FontStyle.Normal;
        NextTurn();
    }

    /// <summary>
    /// Adds a participant to the list of participants.
    /// </summary>
    /// <param name="participant">A reference to the participant to add to the turnorder</param>
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

        // Update the callback function of the target for ending the turn.
        participant.SetEndTurnCallback(EndTurn);

        // Update the nameplate position based on location in turnorder
        float y_coord = -((float)pid + 0.5f) * nameheight + pid * namedist;
        nameplate_tfm.anchoredPosition = new Vector3(0, y_coord, 0);
    }

    /// <summary>
    /// Initializes the TurnTracker GUI
    /// </summary>
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
