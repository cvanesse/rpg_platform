using UnityEngine;
using System.Collections.Generic;
using System.Linq;

// The actor participant handles creation and annihilation of action components on the actor gameObject
public class ActorParticipant : Participant
{

    // Holds a list of actionComponents created on the turn so that they can be removed at the end of the turn.
    private List<Action> actions;

    // Stamina and health information
    public ContinuousResource stamina;
    public Color staminaColor;
    public float maxStamina;
    public float staminaBarX;
    public float staminaBarY;
    public ContinuousResource health;
    public Color healthColor;
    public float maxHealth;
    public float healthBarX;
    public float healthBarY;

    // Inheritance requires that we use the override keyword for extensions of base classes.
    public override void Start()
    {
        base.Start();
        actions = new List<Action>();

        stamina = new ContinuousResource("Stamina", maxStamina, staminaColor, new Vector2(staminaBarX, staminaBarY));
        health = new ContinuousResource("Health", maxHealth, healthColor, new Vector2(healthBarX, healthBarY));
    }

    public List<Resource> GetResourceList()
    {
        List<Resource> resources = new List<Resource>();
        resources.Add(stamina);
        resources.Add(health);
        return resources;
    }

    private void ClearActions()
    {
        foreach (MonoBehaviour comp in actions)
        {
            Destroy(comp);
        }
        actions.Clear();
    }

    public override void StartTurn()
    {
        base.StartTurn();

        stamina.val = stamina.maxVal;

        // Update the actor GUI
        FindObjectOfType<ActorGUI>().SetCurrentActor(this);
    }

    protected override void EndTurn()
    {
        ClearActions();

        // Update the actor GUI
        FindObjectOfType<ActorGUI>().ClearActor();

        base.EndTurn();
    }

    public override void Update()
    {
        base.Update();

        if (isTurn)
        {
            if (Input.GetKeyDown("m"))
            {
                // Add the mover component and track it in the list of action components, if it doesn't exist already.
                if (!actions.OfType<MoveAction>().Any())
                {
                    actions.Add(gameObject.AddComponent<MoveAction>());
                }
                else
                {
                    gameObject.GetComponent<MoveAction>().StartMoving();
                }
            }
        }
    }

}
