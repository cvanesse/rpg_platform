using UnityEngine.UI;
using UnityEngine;

public class UI_Constructors
{
    public static GameObject AddTextObject(string str, float height, Transform parent,
            TextAnchor alignment = TextAnchor.MiddleCenter, string hiername = "",
            bool anchorTop = true)
    {
        if (hiername.Length == 0) { hiername = str.Split(' ')[0]; }

        GameObject gameObject = new GameObject(hiername);
        Text text = gameObject.AddComponent<Text>();
        RectTransform transform = gameObject.GetComponent<RectTransform>();

        // Setup the font of the nameplate
        text.font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        text.text = str;
        text.lineSpacing = 1;
        text.alignment = alignment;
        text.alignByGeometry = true;
        text.resizeTextForBestFit = true;

        // Setup the location of the nameplate.
        transform.SetParent(parent);

        // Set to stretch with canvas on L/R and anchor at top of canvas
        if (anchorTop)
        {
            transform.anchorMin = new Vector2(0, 1);
            transform.anchorMax = new Vector2(1, 1);
        }
        else
        {
            transform.anchorMin = new Vector2(0, 0);
            transform.anchorMax = new Vector2(1, 0);
        }

        // Zero out the scale and position
        transform.localScale = new Vector3(1, 1, 1);

        // Set nameplate transform location.
        transform.sizeDelta = new Vector2(0, height);
        transform.offsetMin = new Vector2(0, transform.offsetMin.y);
        transform.offsetMax = new Vector2(0, transform.offsetMax.y);

        return gameObject;
    }


}
