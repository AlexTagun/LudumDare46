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
                    var theNewUI = createPopularitySourceUI(inAddedSource);
                    _worldObjectsAttachedUIManger.attach(theNewUI.gameObject, inAddedSource.uiAttachPoint);
                },
                (PopularitySource inRemovedSource) => {
                    _worldObjectsAttachedUIManger.destroyUIAttachedForPoint(inRemovedSource.uiAttachPoint);
                });
    }

    MonoBehaviour createPopularitySourceUI(PopularitySource inPopularitySource) {
        switch (inPopularitySource) {
            case PerTickPopularitySource thePerTickPopularitySource:
                PerTickPopularitySourceUIObject thePopularitySourceUI = Instantiate(_perTickPopularitySourceUIPrefab);
                thePopularitySourceUI.init(GetComponent<RectTransform>(), thePerTickPopularitySource);
                return thePopularitySourceUI;
            default:
                throw(new System.Exception("Incorrect logic type"));
        }
    }

    //Field
    [SerializeField] private WorldObjectsAttachedUIManger _worldObjectsAttachedUIManger = null;
    [SerializeField] private PerTickPopularitySourceUIObject _perTickPopularitySourceUIPrefab = null;

    private PopularityCollector _popularityCollector = null;
    private PopularitySource[] _previousCollectedPopularitySourcesArray = new PopularitySource[4];
    private int _actualPreviousCollectedPopularitySourcesArrayNum = 0;
}
