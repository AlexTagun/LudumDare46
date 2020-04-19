using UnityEngine;

public class PopularitySource : MonoBehaviour
{
    public PopularitySourceViewablePointObject[] viewablePoints => _viewablePoints;
    public WorldObjectAttachPointObject uiAttachPoint => _uiAttachPoint;

    public PopularityGainingLogic popularityGainingLogic => _popularityGainingLogic;

    private void Awake() {
        _viewablePoints = GetComponentsInChildren<PopularitySourceViewablePointObject>();

        registerInCollectors();
    }

    private void registerInCollectors() {
        PopularityCollector[] theCollectors = Object.FindObjectsOfType<PopularityCollector>();
        foreach (PopularityCollector theCollector in theCollectors)
            theCollector.registerPopularitySource(this);
    }

    internal void startGaining() { _popularityGainingLogic?.startGaining(); }
    internal void tickPopularityGaining(float inDeltaTime) { _popularityGainingLogic?.tickPopularityGaining(inDeltaTime); }
    internal void stopGaining() { _popularityGainingLogic?.stopGaining(); }

    //Fields

    [SerializeField] private WorldObjectAttachPointObject _uiAttachPoint = null;
    [SerializeField] private PopularityGainingLogic _popularityGainingLogic = null;

    private PopularitySourceViewablePointObject[] _viewablePoints = null;
}
