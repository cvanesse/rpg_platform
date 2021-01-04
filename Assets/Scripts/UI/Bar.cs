using UnityEngine;
using UnityEngine.UI;

// The behavior for a basic bar
public class Bar : ResourceGUI<ContinuousResource>
{
    private Slider fillSlider;
    private Slider dxSlider;

    public Vector2 barPos;

    public override void Init()
    {
        GameObject fill = gameObject.transform.Find("Fill").gameObject;
        GameObject dx_bar = gameObject.transform.Find("Using").gameObject;
        dxSlider = dx_bar.GetComponent<Slider>();
        fillSlider = fill.GetComponent<Slider>();

        gameObject.GetComponent<RectTransform>().anchoredPosition = barPos;

        resource = null;

        UpdateResourceGUI();
    }

    public override void UpdateResourceGUI()
    {
        base.UpdateResourceGUI();

        if (resource != null)
        {
            dxSlider.value = resource.dx / resource.maxVal;
            fillSlider.value = resource.val / resource.maxVal;
        }
    }
}