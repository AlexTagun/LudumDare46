using UnityEngine;

public class PopularityForTargetUIObject : MonoBehaviour
{
    public void init(PopularitySource inPopularitySource) {
        MonoBehaviour theLogicUI = createLogicUI(inPopularitySource.popularityGainingLogic);
        
        var theTranform = theLogicUI.GetComponent<RectTransform>();
        theTranform.SetParent(GetComponent<RectTransform>(), false);
    }

    MonoBehaviour createLogicUI(PopularityGainingLogic inLogic) {
        switch (inLogic) {
            case PerTickPopularityGainingLogic thePerTickPopularityGainingLogic:
                PerTickPopularityGainingLogicUIObject theLogicUI = Instantiate(_perTickPopularityGainingLogicUIPrefab);
                theLogicUI.init(GetComponent<RectTransform>(), thePerTickPopularityGainingLogic);
                return theLogicUI;
            default:
                throw(new System.Exception("Incorrect logic type"));
        }
    }

    //Field

    [SerializeField] private PerTickPopularityGainingLogicUIObject _perTickPopularityGainingLogicUIPrefab;
}
