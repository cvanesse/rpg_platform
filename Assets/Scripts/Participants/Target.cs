using UnityEngine;
using System;

// The Target behaviour listens to the mouse to see if it hovers over a participant.
//  If it does, it calls a callback function.
public class Target : MonoBehaviour
{
    private Sprite inactiveSprite;
    private Sprite highlightSprite;
    private GameObject targetOverlayObject;
    private SpriteRenderer targetSprite;

    private bool mouseInside;
    private bool highlighted;

    public delegate void CallbackFunction(Target target);
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