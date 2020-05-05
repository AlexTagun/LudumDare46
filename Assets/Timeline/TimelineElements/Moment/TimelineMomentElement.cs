using UnityEngine;

public class TimelineElement : MonoBehaviour {
    [SerializeField, ReadOnly] internal protected TimelineVisualElementObject _visual = null;
}

[ExecuteInEditMode]
public class TimelineMomentElement : TimelineElement
{
    protected float time => _time;
    internal string caption => _caption;

    protected virtual void init(float inTime, string inCaption) {
        _time = inTime;
        _caption = inCaption;
    }

    private void Update() {
        updateElementVisualFromSettings();
    }

    private void updateElementVisualFromSettings() {
        if (null == visualCasted) return;
        if (_time != _previouseTime) {
            visualCasted._momentTimeSeconds = _time;
            visualCasted._previouseMomentTimeSeconds = _time;
            _previouseTime = _time;
        }
    }

    TimelineMomentVisualElementObject visualCasted => (TimelineMomentVisualElementObject)_visual;

    //Fields
    [SerializeField] internal float _time = 0f;
    [SerializeField, HideInInspector] internal float _previouseTime = 0f;

    [SerializeField, HideInInspector] private string _caption = null;
}
