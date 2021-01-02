using UnityEngine;

public class Participant : MonoBehaviour
{
    [Tooltip("The name of the participant, which will appear in the turn order.")]
    public string participantName;

    protected bool isTurn;

    // Use public override void Start() to add additional startup routines for other participants.
    public virtual void Start()
    {
        isTurn = false;
    }

    // Starts the player's turn
    public virtual void StartTurn()
    {
        isTurn = true;
    }

    // Ends the player's turn - only accessible by the participant itself.
    protected virtual void EndTurn()
    {
        isTurn = false;
        StartCoroutine(FindObjectOfType<TurnTracker>().EndTurn());
    }

    public virtual void Update()
    {
        // We only want to run any update code if it's currently the player's turn.
        if (isTurn)
        {
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
}
