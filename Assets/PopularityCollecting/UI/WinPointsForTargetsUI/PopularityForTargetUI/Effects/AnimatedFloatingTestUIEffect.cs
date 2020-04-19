using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AnimatedFloatingTestUIEffect : UIEffect
{
    public void setValueAndStart(string inText,
        Vector2 inStartPosition, Vector2 inMoveAnimDelta, float inMoveAnimTime,
        float inFadeOutStartTime, float inFadeOutEndTime)
    {
        _text.text = inText;

        rectTransform.anchoredPosition = inStartPosition;
        Vector2 theEndAnchorPos = rectTransform.anchoredPosition + inMoveAnimDelta;
        rectTransform.DOAnchorPos(theEndAnchorPos, 1f);

        float theTimeToFadeOut = inFadeOutEndTime - inFadeOutStartTime;
        Sequence theSequence = DOTween.Sequence();
        theSequence
            .AppendInterval(inFadeOutStartTime)
            .Append(_text.DOFade(0f, theTimeToFadeOut).OnComplete(() => {
                destroyEffect(1.5f);
            }));
    }

    [SerializeField] Text _text = null;
}
