using UnityEngine;
using System.Collections;

/// <summary>
/// The participant behaviour handles communication with the TurnTracker.
/// This baseclass can be used by the 'Actors' (PCs), NPCs, and AI-controlled NPCs.
/// </summary>
public class Participant : MonoBehaviour
{
    /// <summary>
    /// Delegate for the turn-ending callback function
    /// </summary>
    /// <returns>A string - it's an IEnumerator since it needs to be runnable as a Coroutine.</returns>
    public delegate IEnumerator EndTurnCallback();

    /// <summary>
    /// A reference to the callback function which advances the turnorder.
    /// </summary>
    private EndTurnCallback endTurnCallback;


    /// <summary>
    /// The name of the participant, which will appear in the turnorder.
    /// </summary>
    [Tooltip("The name of the participant, which will appear in the turn order.")]
    public string participantName;

    /// <summary>
    /// A flag - true if it's currently the participant's turn, false otherwise.
    /// </summary>
    protected bool isTurn;

    /// <summary>
    /// Updates the callback function of the participant - to be called by the TurnTracker.
    /// </summary>
    /// <param name="callback">The callback function which advances the turn order.</param>
    public void SetEndTurnCallback (EndTurnCallback callback) {
        endTurnCallback = callback;
    }

    /// <summary>
    /// Use public override void Start() to add additional startup routinges for other participants.
    /// </summary>
    public virtual void Start()
    {
        isTurn = false;
    }

    /// <summary>
    /// Starts the turn of the participant - override to add additional behaviour at the beginning of a participant's turn.
    /// </summary>
    public virtual void StartTurn()
    {
        isTurn = true;
    }

    /// <summary>
    /// Called when the participant intends to end it's turn. Override to add additional turn-end behaviour on the participant side.
    /// </summary>
    protected virtual void EndTurn()
    {
        isTurn = false;
        StartCoroutine(endTurnCallback());
    }

    /// <summary>
    /// Damages the participant.
    /// </summary>
    /// <param name="damage">The amount of damage to do.</param>
    public virtual void Damage(float damage) { }

    /// <summary>
    /// Listens for the basic controls of all participants, override to add additional functionality for the participant.
    /// </summary>
    public virtual void Update()
    {
        // We never run any update code if it's not the player's turn.
        if (!isTurn) {return;}

        if (Input.GetKeyDown("n"))
        {
            print(participantName);
        }
        if (Input.GetKeyUp("space"))
        {
            EndTurn();
        }
    }
}
