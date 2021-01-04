using UnityEngine;

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

        actor.stamina.dx = staminaVal;

        InitRange();

        if (actor.stamina.val < staminaVal)
        {
            print("Not enough stamina for attack.");
            Destroy(this);
        }
    }

    public void Update()
    {
        // Unlike the move action, we don't need to know when the player is using or not using the attack action
        //  If they are using it, this object exists - otherwise it doesn't.

        // Escape or right click ends the action.
        if (Input.GetMouseButton(1) || Input.GetKey("escape"))
        {
            Destroy(this);
        }
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

        // Get a list of all actors within the range
        string[] layer_names = { "Participant" };
        int layer_mask = EnvironmentUtilities.GetLayerMaskFromNames(layer_names);
        Collider2D[] all_colliders = new Collider2D[10];
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
                print(tgt.gameObject.name);
                ListChildBehaviour(tgt);
            }
        }
    }

    // Do damage to the target
    private void DoDamage(Target target)
    {

        print("Doing damage to: " + target.gameObject.name);
        Participant target_participant = target.gameObject.GetComponent<Participant>();
        target_participant.Damage(damageVal);

        actor.stamina.val -= staminaVal;

        Destroy(this);
    }


}