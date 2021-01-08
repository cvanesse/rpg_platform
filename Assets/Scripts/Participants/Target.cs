using UnityEngine;

// The Target behaviour listens to the mouse to see if it hovers over a participant.
//  If it does, it calls a callback function.
/// <summary>
/// The Target behaviour listens to the mouse to see if it hovers over the parent object - 
/// When it hovers over, it gets highlighted, and if it's clicked it calls a callback function.
/// </summary>
public class Target : MonoBehaviour
{
    /// <summary>
    /// The sprite used when the target is not highlighted. 
    /// </summary>
    private Sprite inactiveSprite;

    /// <summary>
    /// The sprite used when the target is highlighted
    /// </summary>
    private Sprite highlightSprite;
    
    /// <summary>
    /// A gameobject for holding the targetOverlay - placed as a child of the gameObject for which this behaviour sits. 
    /// </summary>
    private GameObject targetOverlayObject;

    /// <summary>
    /// The spriterenderer of the target overlay - for setting the non-highlghted and highlighted sprites. 
    /// </summary>
    private SpriteRenderer targetSprite;

    /// <summary>
    /// Flag - true when the mouse is inside the object, and false otherwise. 
    /// </summary>
    private bool mouseInside;

    /// <summary>
    /// Flag - true when the target is currently highlighted, false otherwise. 
    /// </summary>
    private bool highlighted;

    /// <summary>
    /// A delegate for the target callback function: A void which passes a reference to the target so the target object can be identified. 
    /// </summary>
    /// <param name="target">A reference to this target object, when called</param>
    public delegate void CallbackFunction(Target target);

    /// <summary>
    /// The local reference to the callback function, set by SetCallbackFunction()
    /// </summary>
    private CallbackFunction callbackFunction;

    public void Start()
    {
        highlightSprite = Resources.Load("green_ring", typeof(Sprite)) as Sprite;
        inactiveSprite = Resources.Load("red_ring", typeof(Sprite)) as Sprite;

        targetOverlayObject = new GameObject("target_overlay");
        targetOverlayObject.transform.parent = transform;
        targetOverlayObject.transform.position = transform.position;
        targetSprite = targetOverlayObject.AddComponent<SpriteRenderer>();
        targetSprite.sprite = inactiveSprite;
        targetSprite.sortingLayerName = "UI_front";

        mouseInside = false;
    }

    /// <summary>
    /// Sets the callback function to the given function reference
    /// </summary>
    /// <param name="F">The callback method to assign to this target</param>
    public void SetCallback(CallbackFunction F)
    {
        callbackFunction = F;
    }

    public void Update()
    {
        if (mouseInside)
        {
            if (!highlighted)
            {
                targetSprite.sprite = highlightSprite;
                highlighted = true;
            }

            if (Input.GetMouseButtonDown(0))
            {
                callbackFunction(this);
            }
        }
        else {
            if (highlighted) {
                targetSprite.sprite = inactiveSprite;
                highlighted = false;
            }
        }
    }

    public void OnDestroy() {
        Destroy(targetOverlayObject);
    }

    public void OnMouseOver() 
    {
        if (!mouseInside) {
            mouseInside = true;
        }
    }

    public void OnMouseEnter()
    {
        mouseInside = true;
    }

    public void OnMouseExit()
    {
        mouseInside = false;
    }

}