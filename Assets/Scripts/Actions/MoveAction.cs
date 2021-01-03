using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The mover behavior is intended to be added to an object when the player decides to move the character
// (Ie - takes the move action)
// When instantiated, a GUI element showing how much movement is available should be displayed,
// And the moving process should begin.
// 'isMoving' is true when the player is moving, when it is false - the player can do other things while this component tracks the movement
// that they have left.
public class MoveAction : Action
{
    private const float lineWidth = 0.1f;

    private LineRenderer lineRend; // The linerendering object for showing the player -> mouseptr line.

    private bool isMoving; // True when we should have the player -> mouseptr line drawn and distance displaying

    //private float movement; // Float for tracking the amount of movement available to the mover.

    private Vector3 mousePos; // For storing the current mouse posiion. The other vertex of the line will be the center of the mover object.

    private Vector3 participantPos;

    private MoveBar moveBar;

    public override void Start()
    {
        base.Start();

        // Get the current participant location
        UpdateParticipantPos();

        // Load the speed from the participant's stat component
        //movement = gameObject.GetComponent<Stats>().speed;

        // Add a linerenderer object to the GameObject and set it up appropriately.
        InitLineRenderer();

        // Initialize the movement bar using the current stats
        InitBar();

        // Start the movement feedback GUI
        StartMoving();
    }

    // Initializes the movement bar for showing the user their remaining movement.
    private void InitBar()
    {
        //moveBar = FindObjectOfType<MoveBar>();
        //moveBar.valMax = movement;
        UpdateBar();
    }

    // Updates the movement bar.
    private void UpdateBar()
    {
        //moveBar.val = movement;
        //moveBar.dx = 0;
        //moveBar.updateBar();
    }

    // Initializes the line renderer for movement feedback.
    private void InitLineRenderer()
    {
        lineRend = AddChildBehaviour<LineRenderer>();
        lineRend.material = new Material(Shader.Find("Sprites/Default"));
        lineRend.startWidth = lineWidth;
        lineRend.startColor = Color.clear;
        lineRend.endColor = Color.clear;
        lineRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRend.sortingLayerName = "UI_front";
        lineRend.SetPosition(0, participantPos);
    }


    // Sets the color of the movement feedback line.
    private void SetLineColor(Color C)
    {
        lineRend.endColor = C;
    }

    // Start the movement GUI and updates.
    public void StartMoving()
    {
        isMoving = true;
        SetLineColor(Color.white);
    }

    // Stop the movement GUI and updates.
    private void StopMoving()
    {
        isMoving = false;
        SetLineColor(Color.clear);
        gameObject.GetComponent<ActorParticipant>().stamina.dx = 0;
        //UpdateBar();
    }

    // Gets the distance between a destination and the participant.
    private float GetDistance(Vector3 destination)
    {
        destination.z = 0;
        return Vector3.Distance(destination, participantPos);
    }

    // Returns true if a movement is valid, false otherwise
    private bool IsValidMove(Vector3 destination)
    {
        return (GetDistance(destination) <= GetStamina());
    }

    // Finds the movement destination based on mouse position
    private Vector3 FindMovePos(Vector3 mousePos)
    {
        // Return the mouse position if it's a valid move.
        if (IsValidMove(mousePos)) { return mousePos; }

        // Check for the furthest valid position along the mouse line
        Vector3 move = GetStamina() * Vector3.Normalize(mousePos - participantPos);
        return participantPos + move;
    }

    private float GetStamina()
    {
        return gameObject.GetComponent<ActorParticipant>().stamina.val;
    }

    void Update()
    {
        // If we're currently listening to user input
        if (isMoving)
        {
            // Get the current mouse position
            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Needs to be set to 0 since unity works in 3D.

            // Get the appropriate move position and update the GUI based on it.
            Vector3 movePos = FindMovePos(mousePos);

            lineRend.SetPosition(1, movePos);
            //moveBar.dx = GetDistance(movePos);
            gameObject.GetComponent<ActorParticipant>().stamina.dx = GetDistance(movePos);
            //moveBar.updateBar();

            // Is the user right-clicks, or presses escape, then cancel the movement.
            if (Input.GetKeyDown("escape") || Input.GetMouseButtonDown(1))
            {
                StopMoving();
            }

            // Left click moves the participant to the new location
            if (Input.GetMouseButton(0) && IsValidMove(movePos))
            {
                MoveParticipant(movePos);
            }
        }
    }

    // For updating the participantPos variable.
    private void UpdateParticipantPos()
    {
        participantPos = transform.position;
        participantPos.z = 0;
    }

    // Moves the participant to a new location.
    private void MoveParticipant(Vector3 movePos)
    {
        transform.position = transform.position + (movePos - participantPos);
        gameObject.GetComponent<ActorParticipant>().stamina.val = GetStamina() - Vector3.Distance(movePos, participantPos);
        gameObject.GetComponent<ActorParticipant>().stamina.dx = 0;
        UpdateParticipantPos();
        lineRend.SetPosition(0, participantPos);

        //UpdateBar();
        StopMoving();
    }

}