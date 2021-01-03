using UnityEngine;

// The UI_ghost is a behaviour designed to show the 'spatial' consequences of an action
// Ex  - the player move destination
// Ex2 - The location of a set of enemies after an explosion with blowback.
public class UI_ghost : MonoBehaviour
{
    // The actor that this ghost represents
    private ActorParticipant actor;

    private float ghost_alpha = 0.25f;

    // Set the actor of the UI_Ghost and update the ghost accordingly.
    public void SetActor(ActorParticipant new_actor)
    {
        actor = new_actor;

        // Copy the renderer of the actor, update the sorting layer.
        System.Type sr_type = actor.gameObject.GetComponent<SpriteRenderer>().GetType();
        SpriteRenderer actor_rend = actor.gameObject.GetComponent<SpriteRenderer>();
        SpriteRenderer ghost_rend = gameObject.AddComponent<SpriteRenderer>();

        ghost_rend.sprite = actor_rend.sprite;
        ghost_rend.sortingLayerName = "UI_front";

        // Update the alpha of the ghost.
        Color tmp_color = actor_rend.color;
        Color new_color = new Color(
            tmp_color.r, tmp_color.g, tmp_color.b, ghost_alpha
        );
        ghost_rend.color = new_color;
    }

}