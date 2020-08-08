using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class TrackBarUI : GameEventListener
{
    [SerializeField]
    private IntVariable maxSizeObject = null;

    public int maxSize
    {
        get
        {
            return maxSizeObject.Value;
        }
        set
        {
            maxSizeObject.Value = value;
        }
    }

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
        if (maxSize == 0)
        {
            return;
        }
        RectTransform r = GetComponent<RectTransform>();
        int value = ((IntVariable)Event).Value;
        float sizeProportion = (float)value / maxSize;
        float scaleX = sizeProportion > 1.0f ? 1.0f : sizeProportion;
        r.localScale = new Vector3(scaleX, 1f, 1f);
    }

    //When the attached IntVariable is modified, call this function
    public override void OnEventRaised()
    {
        base.OnEventRaised();

        UpdateBarValue();
    }
}
