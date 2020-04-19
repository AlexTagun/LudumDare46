using UnityEngine;

public class PopularityGainingLogic : MonoBehaviour
{
    protected void Start() {
        _popularityManagerCache = Object.FindObjectOfType<PopularityManager>();
    }

    internal virtual void startGaining() { }
    internal virtual void tickPopularityGaining(float inDeltaTime) { }
    internal virtual void stopGaining() { }

    protected PopularityManager popularityManager => _popularityManagerCache;

    private PopularityManager _popularityManagerCache = null;
}
