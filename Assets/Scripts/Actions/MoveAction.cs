using UnityEngine;

// The mover behavior is intended to be added to an object when the player decides to move the character
// (Ie - takes the move action)
public class MoveAction : Action
{
    private LineRenderer lineRend; // The linerendering object for showing the player -> mouseptr line.
    private GameObject ghost; // The UI Ghost for showing player destination.

    // The mouse and participant locations.
    private Vector2 mousePos;
    private Vector2 participantPos;

    // The participant's collider
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

    public override void OnDestroy()
    {
        base.OnDestroy();
        actor.stamina.dx = 0;
    }

    public void Update()
    {
        // Get the current mouse position
        mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get the appropriate move position and update the GUI based on it.
        Vector2 movePos = FindMovePos(mousePos);

        lineRend.SetPosition(1, movePos);
        actor.stamina.dx = Vector2.Distance(movePos, participantPos);
        ghost.transform.position = movePos;

        // Left click moves the participant to the new location
        if (Input.GetMouseButton(0))
        {
            MoveParticipant(movePos);
        }
    }

    // Initialize a UI_ghost for placing at the destination
    private void InitUIGhost()
    {
        // Create the UI Ghost game object
        Object ghost_prefab = Resources.Load("Prefabs/UI_Ghost");
        ghost = AddChildObject(ghost_prefab);

        // Set the actor of the ghost
        ghost.GetComponent<UI_ghost>().SetActor(actor);
    }

    // Initializes the line renderer for movement feedback.
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

    // Finds the movement destination based on mouse position
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