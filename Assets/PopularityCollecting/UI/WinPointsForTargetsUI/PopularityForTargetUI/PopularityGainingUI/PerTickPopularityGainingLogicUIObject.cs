using UnityEngine;

public class PerTickPopularityGainingLogicUIObject : MonoBehaviour
{
    public void init(RectTransform inPopularityForTargetPosRectTransform, PerTickPopularityGainingLogic inLogic) {
        _logic = inLogic;

        _logic.UI_onGainedPoints = (int inGainedPoints)=>{
            float theFloatingAngle = Random.Range(-180f, 180f);
            Vector2 theRandomDirection =
                    new Vector2(Mathf.Cos(theFloatingAngle), Mathf.Sin(theFloatingAngle));
            float theDistance = 50f;
            Vector2 theMoveDelta = theRandomDirection * theDistance;

            //Spawn effect
            var theNewEffect = Instantiate(_floatingTextEffectPrefab);

            theNewEffect.GetComponent<RectTransform>().
                    SetParent(GetComponent<RectTransform>(), false);

            theNewEffect.setValueAndStart(inGainedPoints.ToString(),
                    Vector2.zero, theMoveDelta, 1f,
                    1f, 2f);
        };
    }

    //Fields

    [SerializeField] AnimatedFloatingTestUIEffect _floatingTextEffectPrefab = null;

    private PerTickPopularityGainingLogic _logic = null;
}
