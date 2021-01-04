using UnityEngine;
using System;

// The Target behaviour listens to the mouse to see if it hovers over a participant.
//  If it does, it calls a callback function.
public class Target : MonoBehaviour
{
    //private Sprite inactiveSprite;
    //private Sprite highlightSprite;

    private bool mouseInside;

    public delegate void CallbackFunction(Target target);
    private CallbackFunction callbackFunction;

    public void Start()
    {
        //highlightSprite = Resources.Load("green_ring", typeof(Sprite)) as Sprite;
        //inactiveSprite = Resources.Load("red_ring", typeof(Sprite)) as Sprite;

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
            if (Input.GetMouseButtonDown(0))
            {
                callbackFunction(this);
            }
        }
    }

    public void OnMouseEnter()
    {
        print("Mouse Entered");
        mouseInside = true;
    }

    public void OnMouseExit()
    {
        print("Mouse Exited");
        mouseInside = false;
    }

    private void EmptyCallback(Target target)
    {
        print("NO CALLBACK SET!");
    }

}