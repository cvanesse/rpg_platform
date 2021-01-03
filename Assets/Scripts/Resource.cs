using UnityEngine;

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

    public override GameObject GetGUIObject()
    {
        var guiObj = (GameObject)Resources.Load("Prefabs/MoveBar");
        return guiObj;
    }
}

public class ConsumableContinuousResource : ContinuousResource
{
    public float dx;
}