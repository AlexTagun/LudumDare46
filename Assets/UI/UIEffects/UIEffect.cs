using UnityEngine;

public class UIEffect : MonoBehaviour
{
    private void Awake() {
        _rectTransform = GetComponent<RectTransform>();
    }

    protected void destroyEffect(float inDelay = 0f) {
        Destroy(gameObject, inDelay);
    }

    protected RectTransform rectTransform => _rectTransform;

    RectTransform _rectTransform = null;
}
