using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

// The actor participant handles creation and annihilation of action components on the actor gameObject
/// <summary>
/// The ActorParticipant is a behaviour attached to participants in combat who perform player-controlled actions.
/// This class listens to player input and creates/destroys action objects when the player wants to start/cancal an action
/// </summary>
public class ActorParticipant : Participant
{
    /// <summary>
    /// Holds a reference to the current Action so that it can be destroyed on the end of the turn.
    /// </summary>
    private Action currentAction;

    /// <summary>
    /// Holds the list of available actions for this actor.
    /// </summary>
    private List<Type> availableActions;

    /// <summary>
    /// For holding the input string at every frame (to parse for control.)
    /// </summary>
    private String inputString;

    /// <summary>
    /// Flag - set to true if the actor is currently in the middle of an action - false otherwise.
    /// </summary>
    private bool isActing;

    /// <summary>
    /// Stamina information for the actor.
    /// </summary>
    public ContinuousResource stamina;
    public Color staminaColor;
    public float maxStamina;
    public float staminaBarX;
    public float staminaBarY;

    /// <summary>
    /// Health information for the actor.
    /// </summary>
    public ContinuousResource health;
    public Color healthColor;
    public float maxHealth;
    public float healthBarX;
    public float healthBarY;

    // Inheritance requires that we use the override keyword for extensions of base classes.
    /// <summary>
    /// Initializes the health and stamina of the actor - eventually should load the stats and available actions from saved player information.
    /// </summary>
    public override void Start()
    {
        base.Start();

        availableActions = new List<Type>();
        LoadActions();

        isActing = false;

        stamina = new ContinuousResource("Stamina", maxStamina, staminaColor, new Vector2(staminaBarX, staminaBarY));
        health = new ContinuousResource("Health", maxHealth, healthColor, new Vector2(healthBarX, healthBarY));
    }

    /// <summary>
    /// Loads the list of available actions from saved character information.
    /// </summary>
    private void LoadActions() {
        // Currently a placeholder - eventually there should be a reference to some serialized data with player information.
        
        // Right now these are my only available actions - basic actions like Move, Dodge, etc... should not use up the player's hotbar (which is easily implemented using keys 1-0)
        // Since that unnecessarily takes up action real estate in the player's hotbar. 
        // IE - Move should have a dedicated control.
        availableActions.Add(typeof(MoveAction));
        availableActions.Add(typeof(AttackAction));
    }

    /// <summary>
    /// Gets a list of the resources available to this actor, for updating the GUI.
    /// </summary>
    /// <returns>A list containing all of the resources available to the actor.</returns>
    public List<Resource> GetResourceList()
    {
        List<Resource> resources = new List<Resource>();
        resources.Add(stamina);
        resources.Add(health);
        return resources;
    }

    /// <summary>
    /// Does damage to the actor.
    /// </summary>
    /// <param name="damage">The amount of damage to do.</param>
    public override void Damage(float damage)
    {
        base.Damage(damage);
        health.val -= damage;
    }


    /// <summary>
    /// Starts the turn of this Actor - the Participant base class handles setting the isTurn flag, 
    /// this class handles resetting the available action points/stamins of the actor. Furthermore, this
    /// tells the ActorGUI that this actor's turn has started.
    /// 
    /// TODO: Try and remove the callback to the ActorGUI - this is ugly - and shouldn't be handled by this class.
    /// </summary>
    public override void StartTurn()
    {
        base.StartTurn();

        stamina.val = stamina.maxVal; // Reset the AP/Stamina of the actor.

        // Update the actor GUI
        FindObjectOfType<ActorGUI>().SetCurrentActor(this);
    }

    /// <summary>
    /// Ends the turn for this actor.
    /// </summary>
    protected override void EndTurn()
    {
        // Update the actor GUI
        FindObjectOfType<ActorGUI>().ClearActor();

        base.EndTurn();
    }

    /// <summary>
    /// Called by actions if they are completed successfully (ie - not cancelled by the player.)
    /// </summary>
    public void EndAction() {
        Destroy(currentAction);
        isActing = false;
    }

    /// <summary>
    /// Called when starting a new action
    /// </summary>
    /// <param name="actionType">The action type to start.</param>
    private void StartAction(Type actionType) {
        currentAction = gameObject.AddComponent(actionType) as Action;
        isActing = true;
    }

    /// <summary>
    /// Listens for player input, and creates/cancels actions by adding/removing action components.
    /// 
    /// TODO: Change the control listening to use GetButton instead of GetKey.
    /// </summary>
    public override void Update()
    {
        base.Update();

        if (!isTurn) {return;}

        // If there's no current actions - listen for player input and start the appropriate actions. Otherwise, listen for a cancel request from the player.
        if (!isActing)
        {
            // Check for basic/common action inputs.
            // 'p' is the primary action, 'm' is move, and 1-0 are the player's minor action hotbar.

            inputString = Input.inputString;

            // Check if the key is in the hotbar (numeric keys) to start, otherwise check the hardcoded controls for basic actions.
            try {
                int inputID = Int32.Parse(inputString);
                if (inputID == 0) {inputID = 10;}
                if (inputID <= availableActions.Count) {
                    StartAction(availableActions[inputID - 1]);
                }
            } catch {
                // TODO: Preset key-controls for basic actions.
            }
        }else  {
            if (Input.GetKeyDown("escape") || Input.GetMouseButton(1)) {
                // Destroy the current action
                EndAction();
            }
        }
    }

}
