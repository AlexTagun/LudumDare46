using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnimatedFloatingImageUIEffect : UIEffect
{
    public void restart(float inStartAlpha, float inEndAlpha, float inFadeOutStartTime, float inFadeOutEndTime)
    {
        if (null != _currentAnimation)
            StopCoroutine(_currentAnimation);

        _currentAnimation = fadeOutCoroutine(inStartAlpha, inEndAlpha, inFadeOutStartTime, inFadeOutEndTime);
        StartCoroutine(_currentAnimation);
    }

    private IEnumerator fadeOutCoroutine(float inStartAlpha, float inEndAlpha, float inFadeOutStartTime, float inFadeOutEndTime) {
        setImageAlpha(inStartAlpha);

        yield return new WaitForSeconds(inFadeOutStartTime);

        float theTotalFadeOutDelta = inEndAlpha - inStartAlpha;
        float theTotalTimeToFadeOut = inFadeOutEndTime - inFadeOutStartTime;
        float theTimeToFadeOut = theTotalTimeToFadeOut;
        while (theTimeToFadeOut > 0f) {
            float theCurrentFadeOutTimeProgressRatio = (1f - theTimeToFadeOut / theTotalTimeToFadeOut);
            float theCurrentAlpha = inStartAlpha + theTotalFadeOutDelta * theCurrentFadeOutTimeProgressRatio;
            setImageAlpha(theCurrentAlpha);

            theTimeToFadeOut -= Time.fixedDeltaTime;
            yield return null;
        }

        setImageAlpha(inEndAlpha);
    }

    private void Awake() {
        setImageAlpha(0f);
    }

    private void setImageAlpha(float inAlpha) {
        Color theCurrentColor = _image.color;
        theCurrentColor.a = inAlpha;
        _image.color = theCurrentColor;
    }

    //Fields
    [SerializeField] private Image _image = null;
    [SerializeField] private IEnumerator _currentAnimation = null;
}
