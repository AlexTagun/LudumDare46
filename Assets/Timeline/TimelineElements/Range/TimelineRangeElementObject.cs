using UnityEngine;

[ExecuteAlways]
public class TimelineRangeElementObject : MonoBehaviour
{
    private void Awake() {
        rectTransform.hideFlags = rectTransform.hideFlags | HideFlags.HideInInspector;
    }

    private void Update() {
        updateVisual();
    }

    private void updateVisual() {
        rectTransform.anchorMin = new Vector2(_timeSecondsFrom, rectTransform.anchorMin.y);
        rectTransform.anchorMax = new Vector2(_timeSecondsTo, rectTransform.anchorMax.y);
        rectTransform.pivot = Vector2.zero;
        rectTransform.offsetMin = new Vector2(0f, rectTransform.offsetMin.y);
        rectTransform.offsetMax = new Vector2(0f, rectTransform.offsetMax.y);

        _textTimeFrom.text = _timeSecondsFrom.ToString("0.0");
        _textTimeTo.text = _timeSecondsTo.ToString("0.0");
    }

    RectTransform rectTransform => GetComponent<RectTransform>();

    //Fields
    [SerializeField] private float _timeSecondsFrom = 0f;
    [SerializeField] private float _timeSecondsTo = 1f;

    [SerializeField] private UnityEngine.UI.Text _textTimeFrom = null;
    [SerializeField] private UnityEngine.UI.Text _textTimeTo = null;
}
