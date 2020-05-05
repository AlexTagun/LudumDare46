using UnityEngine;
using UnityEditor;

public class ReadOnlyAttribute : PropertyAttribute { }

[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}


public abstract class TimelineVisualElementObject : MonoBehaviour {
    internal abstract TimelineElement attachedElement { get; }
}


[ExecuteInEditMode]
public class TimelineMomentVisualElementObject : TimelineVisualElementObject
{
    //Timeline API
    internal void init(TimelineMomentElement inElement) {
        _element = inElement;
    }

    internal override TimelineElement attachedElement {
        get { return _element; }
    }

    private void Awake() {
        rectTransform.hideFlags = rectTransform.hideFlags | HideFlags.HideInInspector;
    }

    private void Update() {
        if (null == _element) return;
        updateElementFromSettings();
        updateVisual();
    }

    private void updateVisual() {
        rectTransform.anchorMin = new Vector2(_momentTimeSeconds, rectTransform.anchorMin.y);
        rectTransform.anchorMax = new Vector2(_momentTimeSeconds, rectTransform.anchorMax.y);
        rectTransform.pivot = new Vector2(_momentTimeSeconds, rectTransform.pivot.y);

        _textTime.text = _momentTimeSeconds.ToString("0.0");
    }

    private void updateElementFromSettings() {
        if (_momentTimeSeconds != _previouseMomentTimeSeconds) {
            _element._time = _momentTimeSeconds;
            _element._previouseTime = _momentTimeSeconds;
            _previouseMomentTimeSeconds = _momentTimeSeconds;
        }
    }

    internal RectTransform rectTransform => GetComponent<RectTransform>();

    //Fields
    [SerializeField] internal float _momentTimeSeconds = 0f;
    [SerializeField, HideInInspector] internal float _previouseMomentTimeSeconds = 0f;

    [SerializeField, ReadOnly] private TimelineMomentElement _element = null;

    [SerializeField] private UnityEngine.UI.Text _textTime = null;
}
