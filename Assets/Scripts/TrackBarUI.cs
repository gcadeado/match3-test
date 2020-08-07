using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TrackBarUI : GameEventListener
{
    private void Start()
    {
        UpdateBarValue();
    }

    private void OnValidate()
    {
        UpdateBarValue();
    }

    public void UpdateBarValue()
    {
        RectTransform r = GetComponent<RectTransform>();
        int value = ((IntVariable)Event).Value;

        float scaleX = (float)value / 1000 > 1.0f ? 1.0f : (float)value / 1000;
        r.localScale = new Vector3(scaleX, 1f, 1f);
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateBarValue();
    }
}
