using UnityEngine;

public class PopularitySource : MonoBehaviour
{
    public PopularitySourceViewablePointObject[] viewablePoints => _viewablePoints;
    public WorldObjectAttachPointObject uiAttachPoint => _uiAttachPoint;

    protected void Start() {
        _popularityManagerCache = Object.FindObjectOfType<PopularityManager>();

        collectViewablePoints();
        registerInCollectors();
    }

    private void collectViewablePoints() {
        _viewablePoints = GetComponentsInChildren<PopularitySourceViewablePointObject>();
    }

    private void registerInCollectors() {
        PopularityCollector[] theCollectors = Object.FindObjectsOfType<PopularityCollector>();
        foreach (PopularityCollector theCollector in theCollectors)
            theCollector.registerPopularitySource(this);
    }

    protected PopularityManager popularityManager => _popularityManagerCache;

    internal virtual void startGaining() { }
    internal virtual void tickPopularityGaining(float inDeltaTime) { }
    internal virtual void stopGaining() { }

    //Fields
    [SerializeField] private WorldObjectAttachPointObject _uiAttachPoint = null;

    private PopularityManager _popularityManagerCache = null;
    private PopularitySourceViewablePointObject[] _viewablePoints = null;
}
