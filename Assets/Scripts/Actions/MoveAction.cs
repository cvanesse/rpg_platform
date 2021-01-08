using UnityEngine;

/// <summary>
/// The behaviour associated with players taking a move action.
/// </summary>
public class MoveAction : Action
{
    /// <summary>
    /// The linerendering object for showing the player -> mouseptr line.
    /// </summary>
    private LineRenderer lineRend;

    /// <summary>
    /// The ghost object for showing the player destination.
    /// </summary>
    private GameObject ghost;

    /// <summary>
    /// A reference to the player position, for cleaner use throughout the script.
    /// </summary>
    private Vector2 participantPos;

    /// <summary>
    /// A reference to the participant's collider.
    /// </summary>
    private Rigidbody2D participantCollider;

    public override void Start()
    {
        base.Start();

        // Get the current participant location
        participantPos = (Vector2)transform.position;
        participantCollider = gameObject.GetComponent<Rigidbody2D>();

        // Add a linerenderer object to the GameObject and set it up appropriately.
        InitLineRenderer();

        // Add a UI ghost for the destination.
        InitUIGhost();
    }

    /// <summary>
    /// The MoveAction needs to reset actor.stamina.dx to zero when it's destroyed.
    /// </summary>
    public override void OnDestroy()
    {
        base.OnDestroy();
        actor.stamina.dx = 0;
    }

    public void Update()
    {
        // Get the appropriate move position and update the GUI based on it.
        Vector2 movePos = FindMovePos(
            (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
        );

        lineRend.SetPosition(1, movePos);
        actor.stamina.dx = Vector2.Distance(movePos, participantPos);
        ghost.transform.position = movePos;

        // Left click moves the participant to the new location
        if (Input.GetMouseButton(0))
        {
            MoveParticipant(movePos);
        }
    }

    /// <summary>
    /// Creates the UI Ghost of the participant and initializes it to show the actor destination.
    /// </summary>
    private void InitUIGhost()
    {
        // Create the UI Ghost game object
        Object ghost_prefab = Resources.Load("Prefabs/UI_Ghost");
        ghost = AddChildObject(ghost_prefab);

        // Set the actor of the ghost
        ghost.GetComponent<UI_ghost>().SetActor(actor);
    }

    /// <summary>
    /// Creates and initializes the line renderer to show the actor destination.
    /// </summary>
    private void InitLineRenderer()
    {
        lineRend = AddChildBehaviour<LineRenderer>();
        lineRend.material = new Material(Shader.Find("Sprites/Default"));
        lineRend.startWidth = 0.1f;
        lineRend.startColor = Color.clear;
        lineRend.endColor = Color.white;
        lineRend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        lineRend.sortingLayerName = "UI_front";
        lineRend.SetPosition(0, participantPos);
    }

    /// <summary>
    /// Gets the movement destination based on the mouse position. This will return either the maximum move distance based on AP/stamina
    /// and the mouse position, the maximum distance moveable based on any collisions with the environment, or the mouse position itself.
    /// </summary>
    /// <param name="mousePos">The current mouse position</param>
    /// <returns>The actor destination based on the current mouse position.</returns>
    private Vector2 FindMovePos(Vector2 mousePos)
    {
        // Create a ray pointing from the participant to the mouse
        Vector2 direction = mousePos - participantPos;
        direction.Normalize();

        float max_distance = Vector2.Distance(mousePos, participantPos); // Maximum distance is from the mouse to the participant.

        // Check for collisions in the wall or participant layers on that ray
        RaycastHit2D[] hits = new RaycastHit2D[10];// = Physics2D.RaycastAll(origin, direction, max_distance);

        // collider cast the player collider
        int numHits = participantCollider.Cast(direction, hits, max_distance);

        // TODO: Integrate movement modifiers from different floor tiles along the ray

        // Determine the maximum move distance in the direction of the mouse.
        float move_dist = max_distance;

        float d_move = 0.1f; // TODO: Find a better solution than this.

        if (numHits > 0)
        {
            // Find the closest hit
            Transform closest_object = hits[0].transform;
            float collision_dist = hits[0].distance;

            // If there is more than 1 hit, loop through the rest and determine the closest one to the participant.
            if (numHits > 1)
            {
                for (int hid = 1; hid < numHits; hid++)
                {
                    RaycastHit2D hit = hits[hid];

                    float hit_dist = hit.distance;
                    if (hit_dist < collision_dist)
                    {
                        closest_object = hit.transform;
                        collision_dist = hit_dist;
                    }
                }
            }

            // Update the move distance based on the collision distance
            move_dist = collision_dist - d_move;
        }

        // collision_dist stores the distance to the nearest collision
        if (move_dist > actor.stamina.val)
        {
            move_dist = actor.stamina.val;
        }

        Vector2 destination = participantPos + direction * move_dist;

        return destination;
    }

    // Moves the participant to a new location.
    /// <summary>
    /// Moves the participant to a destination, updates the actor stamina based on that movement, and finished the move action.
    /// </summary>
    /// <param name="destination">The destination of the actor.</param>
    private void MoveParticipant(Vector2 destination)
    {
        transform.position = destination;
        actor.stamina.val = actor.stamina.val - Vector3.Distance(destination, participantPos);
        actor.stamina.dx = 0;
        participantPos = (Vector2)transform.position;
        lineRend.SetPosition(0, participantPos);

        FinishAction();
    }

}