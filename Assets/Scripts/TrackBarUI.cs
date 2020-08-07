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
        int score = ((IntVariable)events[0]).Value;

        int target = ((IntVariable)events[1]).Value;

        float barProportion = (float)score / target;

        float scaleX = barProportion > 1.0f ? 1.0f : barProportion;
        r.localScale = new Vector3(scaleX, 1f, 1f);
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateBarValue();
    }
}
