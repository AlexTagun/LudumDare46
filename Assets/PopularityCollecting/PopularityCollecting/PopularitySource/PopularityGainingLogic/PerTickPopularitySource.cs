using UnityEngine;

public class PerTickPopularitySource : PopularitySource
{
    //For UI{
    public System.Action<int> UI_onGainedPoints = null;
    public System.Action UI_onStoppedGainingPoints = null;
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

        notifyPopularityStoppedGaining();
    }

    private void gainPopularity() {
        popularityManager.gainPopularity(_popularityPerGaining);

        notifyPopularityGained(_popularityPerGaining);
    }

    private void notifyPopularityGained(int inPopularityGained) {
        UI_onGainedPoints?.Invoke(inPopularityGained);
    }

    private void notifyPopularityStoppedGaining() {
        UI_onStoppedGainingPoints?.Invoke();
    }

    // Fields

    float _tickingTime = 0f;

    [SerializeField] float _timeBetweenGainings = 1f;
    [SerializeField] int _popularityPerGaining = 10;
}
