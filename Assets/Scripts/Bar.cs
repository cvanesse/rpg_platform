using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// The behavior for a basic bar
public class Bar : ResourceGUI
{
    public float val;
    public float valMax;

    public float dx;

    private GameObject fill;
    private GameObject dx_bar;

    private Slider fillSlider;
    private Slider dxSlider;

    public override void Init()
    {
        gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector3(-27, 96, 0);
        fill = gameObject.transform.Find("Fill").gameObject;
        dx_bar = gameObject.transform.Find("Using").gameObject;

        dxSlider = dx_bar.GetComponent<Slider>();
        fillSlider = fill.GetComponent<Slider>();

        resource = null;

        UpdateResourceGUI();
    }

    protected override void InitResourceGUI()
    {
        val = ((ConsumableContinuousResource)resource).val;
        valMax = ((ConsumableContinuousResource)resource).maxVal;
        dx = ((ConsumableContinuousResource)resource).dx;
        UpdateResourceGUI();
    }

    public override void UpdateResourceGUI()
    {
        base.UpdateResourceGUI();

        if (resource != null)
        {
            dx = ((ConsumableContinuousResource)resource).dx;
            val = ((ConsumableContinuousResource)resource).val;
            dxSlider.value = dx / valMax;
            fillSlider.value = val / valMax;

        }
    }
}