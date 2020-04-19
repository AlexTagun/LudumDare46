using UnityEngine;

public class PerTickPopularityGainingLogic : PopularityGainingLogic
{
    //For UI{
    public System.Action<int> UI_onGainedPoints = null;
    //}

    internal override void tickPopularityGaining(float inDeltaTime) {
        _tickingTime += inDeltaTime;
        if (_tickingTime > _timeBetweenGainings) {
            _tickingTime = 0f;
            gainPopularity();
        }
    }

    internal override void stopGaining() {
        _tickingTime = 0f;
    }

    private void gainPopularity() {
        popularityManager.gainPopularity(_popularityPerGaining);

        notifyPopularityGained(_popularityPerGaining);
    }

    private void notifyPopularityGained(int inPopularityGained) {
        if (null != UI_onGainedPoints)
            UI_onGainedPoints(inPopularityGained);
    }

    // Fields

    float _tickingTime = 0f;

    [SerializeField] float _timeBetweenGainings = 1f;
    [SerializeField] int _popularityPerGaining = 10;
}
