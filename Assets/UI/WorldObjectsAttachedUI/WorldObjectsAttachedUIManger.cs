using UnityEngine;
using System.Collections.Generic;

public class WorldObjectsAttachedUIManger : MonoBehaviour
{
    //Methods
    //-API
    public int destroyUIAttachedForPoint(WorldObjectAttachPointObject inAttachPoint) {
        return _uiAttaches_toWorldObjectAttachPoint.RemoveAll((UIAttach_ToWorldObjectAttachPoint inAttach) => {
            if (inAttachPoint == inAttach.attachPoint) {
                if (inAttach.UITransform)
                    Destroy(inAttach.UITransform.gameObject);
                return true;
            } else {
                return false;
            }
        });
    }

    public void attach(
        GameObject inUI, WorldObjectAttachPointObject inAttachPoint, bool inDestroyUIWithObject = true)
    {
        if (!inUI)
            throw (new System.Exception("Incorrect input"));
        if (!inAttachPoint)
            throw (new System.Exception("Incorrect input"));

        var theTranform = inUI.GetComponent<RectTransform>();
        theTranform.SetParent(_worldAttachTransform, false);

        var theNewAttach = new UIAttach_ToWorldObjectAttachPoint(
            inUI.GetComponent<RectTransform>(), inAttachPoint, inDestroyUIWithObject);
        _uiAttaches_toWorldObjectAttachPoint.Add(theNewAttach);
    }

    public void attach(
        GameObject inUI, GameObject inGameObject, bool inDestroyUIWithObject = true)
    {
        var theAttachPoint = inGameObject.GetComponent<WorldObjectAttachPointObject>();
        attach(inUI, theAttachPoint, inDestroyUIWithObject);
    }

    //-Implementation
    void Update() {
        _uiAttaches_toWorldObjectAttachPoint.RemoveAll((UIAttach_ToWorldObjectAttachPoint inAttach) =>{
            if (!inAttach.isValid()) {
                if (!!inAttach.UITransform) {
                    Destroy(inAttach.UITransform.gameObject);
                }
                return true;
            }

            updateToWorldObjectAttachPoint(ref inAttach);
            return false;
        });
    }

    void updateToWorldObjectAttachPoint(ref UIAttach_ToWorldObjectAttachPoint inAttach) {
        Vector2 theViewportNormalizedPosition = getViewportNormalizedPositionForWorldPosition(inAttach.attachPoint.attachPoint);

        inAttach.UITransform.anchorMax = theViewportNormalizedPosition;
        inAttach.UITransform.anchorMin = theViewportNormalizedPosition;
    }


    private Vector2 getViewportNormalizedPositionForWorldPosition(Vector3 inWorldPosition) {
        if (!Camera.main) return new Vector2();
        return Camera.main.WorldToViewportPoint(inWorldPosition);
    }

    private struct UIAttach_ToWorldObjectAttachPoint
    {
        public UIAttach_ToWorldObjectAttachPoint(
            RectTransform inUITransform, WorldObjectAttachPointObject inAttachPoint, bool inDestroyUIWithObject)
        {
            UITransform = inUITransform;
            attachPoint = inAttachPoint;
            destroyUIWithObject = inDestroyUIWithObject;
        }

        public bool isValid() {
            return (!!UITransform) && (!!attachPoint);
        }

        public RectTransform UITransform;
        public WorldObjectAttachPointObject attachPoint;
        public bool destroyUIWithObject;
    }

    List<UIAttach_ToWorldObjectAttachPoint> _uiAttaches_toWorldObjectAttachPoint = new List<UIAttach_ToWorldObjectAttachPoint>();

    [SerializeField] RectTransform _worldAttachTransform = null;
}