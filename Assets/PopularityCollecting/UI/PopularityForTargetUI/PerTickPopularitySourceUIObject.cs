using UnityEngine;
using UnityEngine.UI;

public class PerTickPopularitySourceUIObject : MonoBehaviour
{
    public void init(RectTransform inPopularityForTargetPosRectTransform, PerTickPopularitySource inLogic) {
        _logic = inLogic;

        _logic.UI_onGainedPoints = (int inGainedPoints)=>{
            highlightPopularityIcon();
            spawnGainedPopularityEffect(inGainedPoints);
        };
    }

    private void spawnGainedPopularityEffect(int inGainedPoints) {
        float theFloatingAngle = 90f * Mathf.Deg2Rad;
        Vector2 theDirection = new Vector2(Mathf.Cos(theFloatingAngle), Mathf.Sin(theFloatingAngle));
        float theDistance = 50f;
        Vector2 theMoveDelta = theDirection * theDistance;

        var theNewEffect = Instantiate(_floatingTextEffectPrefab);
        theNewEffect.GetComponent<RectTransform>().SetParent(GetComponent<RectTransform>(), false);
        theNewEffect.setValueAndStart(inGainedPoints.ToString(),
                _textSpawningPosition.localPosition, theMoveDelta, 1f,
                1f, 2f);
    }

    private void highlightPopularityIcon() {
        _popularityIconImage.restart(0.9f, 0.1f, 0, 0.3f);
    }

    //Fields
    [SerializeField] private AnimatedFloatingImageUIEffect _popularityIconImage = null;
    [SerializeField] private AnimatedFloatingTextUIEffect _floatingTextEffectPrefab = null;
    [SerializeField] private RectTransform _textSpawningPosition = null;
    
    private PerTickPopularitySource _logic = null;
}
