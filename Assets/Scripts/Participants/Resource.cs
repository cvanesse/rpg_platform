using UnityEngine;
using UnityEngine.UI;

// A placeholder class for an actor resource (for stamina, mana, ammo, etc...)
public class Resource
{
    public string resourceName;

    public virtual GameObject GetGUIObject() { return null; }
}

public class ContinuousResource : Resource
{
    public float maxVal;
    public float val;
    public float dx;

    private Color barColor;
    public Vector2 barPos;

    public ContinuousResource(string name, float max_val, Color bar_color, Vector2 bar_pos)
    {
        this.resourceName = name;
        this.maxVal = max_val;
        this.val = max_val;
        this.barColor = bar_color;
        this.barPos = bar_pos;
    }

    public ContinuousResource(string name, float max_val, Color bar_color, Vector2 bar_pos, float val) {
        this.resourceName = name;
        this.maxVal = max_val;
        this.val = val;
        this.barColor = bar_color;
        this.barPos = bar_pos;
    }

    public override GameObject GetGUIObject()
    {
        var guiObj = (GameObject)Resources.Load("Prefabs/Bar");

        var fill = guiObj.transform.Find("Fill").Find("FillColor");
        fill.GetComponent<Image>().color = barColor;
        guiObj.GetComponent<Bar>().barPos = barPos;

        return guiObj;
    }
}