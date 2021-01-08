using UnityEngine;
using System;

// The basic attack action - renders a range circle and allows selection of a single target within that range.
public class AttackAction : Action
{
    // A gameobject for highlighting a target when the mouse hovers over them
    private GameObject crosshair;

    // A gameobject for highlighting the range of the attack around the player. 
    private FovMesh rangeMesh;

    // For storing how much damage this action will do.
    public float damageVal;
    public float attackRange;
    public float staminaVal;

    public override void Start()
    {
        base.Start();

        damageVal = 1f;
        attackRange = 3f;
        staminaVal = 5f;

        if (actor.stamina.val < staminaVal)
        {
            // Not enough stamina for attack - add sound effect for failure.
            FinishAction();
            return;
        }

        actor.stamina.dx = staminaVal;
        InitRange();
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        actor.stamina.dx = 0;
    }

    // Create and draw a the attack range.
    private void InitRange()
    {
        // Draw the range
        GameObject rangeObj = AddChildObject("RangeOverlay");
        rangeMesh = rangeObj.AddComponent<FovMesh>();
        rangeMesh.SetVision(attackRange);

        // Create a collider on the FovMesh, and check for participants which overlap this collider.
        PolygonCollider2D fovCollider = rangeObj.AddComponent<PolygonCollider2D>();
        fovCollider.isTrigger = true;

        Rigidbody2D rb2D = rangeObj.AddComponent<Rigidbody2D>();
        rb2D.bodyType = RigidbodyType2D.Static;

        fovCollider.points = Array.ConvertAll(rangeMesh.mesh.vertices, item => (Vector2)item);

        // Get a list of all actors within the range
        string[] layer_names = { "Participant" };
        int layer_mask = EnvironmentUtilities.GetLayerMaskFromNames(layer_names);
        Collider2D[] all_colliders = new Collider2D[10]; // TODO: Adaptively increase the number of possible targets based on the area of the FOV.
        ContactFilter2D collider_filter = new ContactFilter2D();
        collider_filter.layerMask = layer_mask;
        int num_colliders = Physics2D.OverlapCollider(rangeObj.GetComponent<PolygonCollider2D>(), collider_filter, all_colliders);
        for (int cid = 0; cid < num_colliders; cid++)
        {
            Collider2D col = all_colliders[cid];
            GameObject obj = col.gameObject;
            if (obj != gameObject && obj.GetComponent<Participant>() != null)
            {
                // Add a target component to the object, stored as a child of this action
                Target tgt = obj.AddComponent<Target>();
                tgt.SetCallback(DoDamage);
                ListChildBehaviour(tgt);
            }
        }

        // After creating the list of targets and target options, we don't need to keep a collider on the FOV.
        Destroy(rb2D);
        Destroy(fovCollider);
    }

    // Do damage to the target (Callback function for the target.)
    private void DoDamage(Target target)
    {
        Participant target_participant = target.gameObject.GetComponent<Participant>();
        target_participant.Damage(damageVal);

        actor.stamina.val -= staminaVal;

        // The action is over once we've done damage.
        FinishAction();
    }
}