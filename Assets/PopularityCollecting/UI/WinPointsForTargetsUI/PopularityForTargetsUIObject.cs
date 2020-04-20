using UnityEngine;

public class PopularityForTargetsUIObject : MonoBehaviour
{
    private void Start() {
        _popularityCollector = FindObjectOfType<PopularityCollector>();
    }

    private void FixedUpdate() {
        PopularitySource[] theCurrentPopularitySources = _popularityCollector.collectedPopularitySources;
        int theActualCollectedPopularitySourcesNum = _popularityCollector.actualCollectedPopularitySourcesNum;

        ArrayUtils.setFromArrayProcessingChanges(
                ref _previousCollectedPopularitySourcesArray, ref _actualPreviousCollectedPopularitySourcesArrayNum,
                theCurrentPopularitySources, theActualCollectedPopularitySourcesNum,
                (PopularitySource inAddedSource) =>{
                    var theNewUI = Instantiate(_popularityForTargetUIPrefab).GetComponent<PopularityForTargetUIObject>();
                    theNewUI.init(inAddedSource);
                    _worldObjectsAttachedUIManger.attach(theNewUI.gameObject, inAddedSource.uiAttachPoint);
                },
                (PopularitySource inRemovedSource) => {
                    _worldObjectsAttachedUIManger.destroyUIAttachedForPoint(inRemovedSource.uiAttachPoint);
                });
    }

    //Fields

    [SerializeField] private WorldObjectsAttachedUIManger _worldObjectsAttachedUIManger = null;
    [SerializeField] private PopularityForTargetUIObject _popularityForTargetUIPrefab = null;

    private PopularityCollector _popularityCollector = null;
    private PopularitySource[] _previousCollectedPopularitySourcesArray = new PopularitySource[4];
    private int _actualPreviousCollectedPopularitySourcesArrayNum = 0;
}
