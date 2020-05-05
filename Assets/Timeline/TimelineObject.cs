using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class TimelineObject : MonoBehaviour
{
    void Awake() {
        _elementsContainerRectTransform.hideFlags =
                _elementsContainerRectTransform.hideFlags | HideFlags.HideInInspector;
    }

    void Update() {
        updateVisualizers();
        updateTimeScale();
    }

    void updateVisualizers() {
        _visualElements.RemoveAll((TimelineVisualElementObject inVisual)=>{
            bool theShouldBeRemoved =
                    (null == inVisual) || (null == inVisual.attachedElement) ||
                    (-1 == System.Array.IndexOf(_elements, inVisual.attachedElement));
            if (theShouldBeRemoved)
                DestroyImmediate(inVisual.gameObject);
            return theShouldBeRemoved;
        });

        foreach (TimelineElement theElement in _elements) {
            if (null == theElement) continue;
            bool theShouldBeAdded = (-1 == _visualElements.FindIndex(
                    (TimelineVisualElementObject inVisualElement) => { return theElement == inVisualElement.attachedElement; }));
            if (theShouldBeAdded)
                _visualElements.Add(createVisualForElement(theElement));
        }
    }

    private void updateTimeScale() {
        _elementsContainerRectTransform.anchorMin = new Vector2(0f, _elementsContainerRectTransform.anchorMin.y);
        _elementsContainerRectTransform.anchorMax = new Vector2(1 / _secondsPerScreen, _elementsContainerRectTransform.anchorMax.y);

        _elementsContainerRectTransform.anchoredPosition = Vector2.zero;
        _elementsContainerRectTransform.offsetMin = new Vector2(0f, _elementsContainerRectTransform.offsetMin.y);
        _elementsContainerRectTransform.offsetMax = new Vector2(0f, _elementsContainerRectTransform.offsetMax.y);
    }

    private TimelineVisualElementObject createVisualForElement(TimelineElement inElement) {
        switch (inElement) {
            case TimelineMomentElement theMomentElement:
                TimelineMomentVisualElementObject theMomentVisualElement = Instantiate(_momentElementVisualPrefab);
                theMomentVisualElement.rectTransform.SetParent(_elementsContainerRectTransform, false);
                theMomentVisualElement.init(theMomentElement);
                inElement._visual = theMomentVisualElement;
                return theMomentVisualElement;
            default:
                throw(new System.Exception("Unsupported timeline element"));
        }
    }

    //Fields
    [SerializeField, Range(1F, 10F)] float _secondsPerScreen = 1;
    [SerializeField] private TimelineElement[] _elements = null;
    [SerializeField, HideInInspector] private List<TimelineVisualElementObject> _visualElements = new List<TimelineVisualElementObject>();

    //TODO: Hide this fields from setup {
    [SerializeField] private RectTransform _elementsContainerRectTransform = null;
    [SerializeField] private TimelineMomentVisualElementObject _momentElementVisualPrefab = null;
    //}
}
