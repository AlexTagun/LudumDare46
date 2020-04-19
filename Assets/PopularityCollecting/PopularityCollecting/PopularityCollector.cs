using UnityEngine;

public class PopularityCollector : MonoBehaviour
{
    public PopularitySource[] collectedPopularitySources => _collectedPopularitySourcesArray;
    public int actualCollectedPopularitySourcesNum => _actualCollectedPopularitySourcesNum;

    internal void registerPopularitySource(PopularitySource inPopularitySource) {
        ArrayUtils.addToArrayAdopting(ref _registeredPopularitySourcesArray, ref _actualRegisteredPopularitySourcesNum,
                inPopularitySource);
    }

    private void Awake() {
        GameMathUtils.calculateFrustumCenterTriangles(_XZFrustrumPlaneCorners, _XYFrustrumPlaneCorners, _FOV, _farPlane, _nearPlane);
    }

    private void FixedUpdate() {
        updateWorldFrusturmPlanesCorners();
        updateRegisteredPopularitySources();
        updateCollectedPopularitySources();
        updatePopularityGaining();
    }

    private void updateWorldFrusturmPlanesCorners() {
        System.Array.Copy(_XZFrustrumPlaneCorners, _WorldXZFrustrumPlaneCorners, 3);
        System.Array.Copy(_XYFrustrumPlaneCorners, _WorldXYFrustrumPlaneCorners, 3);

        Transform theTransform = cameraForCollecting.transform;
        for (int theCornerIndex = 0; theCornerIndex < 3; ++theCornerIndex) {
            _WorldXZFrustrumPlaneCorners[theCornerIndex] =
                    theTransform.TransformPoint(_WorldXZFrustrumPlaneCorners[theCornerIndex]);
            _WorldXYFrustrumPlaneCorners[theCornerIndex] =
                    theTransform.TransformPoint(_WorldXYFrustrumPlaneCorners[theCornerIndex]);
        }
    }

    private void updateRegisteredPopularitySources() {
        ArrayUtils.removeFromArraySwapping(_registeredPopularitySourcesArray, ref _actualRegisteredPopularitySourcesNum,
                (PopularitySource inPopularitySource) => !inPopularitySource);
    }

    static private PopularitySource[] _register_collectedPopularitySourcesArray = new PopularitySource[4];
    static private int _refister_actualCollectedPopularitySourcesNum = 0;
    private void updateCollectedPopularitySources() {
        _refister_actualCollectedPopularitySourcesNum = 0;

        ArrayUtils.iterateArray(_registeredPopularitySourcesArray, _actualRegisteredPopularitySourcesNum,
            (PopularitySource theRegisteredPopularitySource) =>{
                if (isPopularitySourceShouldBeCollected(theRegisteredPopularitySource))
                {
                    ArrayUtils.addToArrayAdopting(
                            ref _register_collectedPopularitySourcesArray, ref _refister_actualCollectedPopularitySourcesNum,
                            theRegisteredPopularitySource);
                }
            });

        ArrayUtils.setFromArrayProcessingChanges(
            ref _collectedPopularitySourcesArray, ref _actualCollectedPopularitySourcesNum,
            _register_collectedPopularitySourcesArray, _refister_actualCollectedPopularitySourcesNum,
            (PopularitySource inAddedPopularitySource)=>inAddedPopularitySource.startGaining(),
            (PopularitySource inRemovedPopularitySource)=>inRemovedPopularitySource.stopGaining());
    }

    private void updatePopularityGaining() {
        ArrayUtils.iterateArray(_collectedPopularitySourcesArray, _actualCollectedPopularitySourcesNum,
            (PopularitySource inPopularitySource) =>{
                inPopularitySource.tickPopularityGaining(Time.fixedDeltaTime);
            });
    }

    private bool isPopularitySourceShouldBeCollected(PopularitySource inPopularitySource) {
        foreach (PopularitySourceViewablePointObject theViewablePoint in inPopularitySource.viewablePoints) {
            Vector3 thePoint = theViewablePoint.transform.position;
            if (isPointInsideFrustrum(thePoint))
                return isPointVisible(thePoint);
        }

        return false;
    }

    private bool isPointInsideFrustrum(Vector3 inPoint) {
        bool theIsPointInsideXZFrustrumPlaneCorners =
                GameMathUtils.isPointInsideTriangle(inPoint, _WorldXZFrustrumPlaneCorners);
        bool theIsPointInsideXYFrustrumPlaneCorners =
                GameMathUtils.isPointInsideTriangle(inPoint, _WorldXYFrustrumPlaneCorners);
        return theIsPointInsideXZFrustrumPlaneCorners && theIsPointInsideXYFrustrumPlaneCorners;
    }

    private bool isPointVisible(Vector3 inPoint) {
        Vector3 theTraceStart = cameraForCollecting.transform.position;
        int theMask = ~0; //TODO: Exclude here triggers layer
        return !Physics.Linecast(theTraceStart, inPoint, theMask);
    }

    private void OnDrawGizmos() {
        if (_debug_drawGizmos && isCameraForCollectingCorrect) {
            Transform theTransform = cameraForCollecting.transform;

            Matrix4x4 theGizmosSavedMatrix = Gizmos.matrix;
            Color theGizmosSavedColor = Gizmos.color;
            Gizmos.matrix = Matrix4x4.TRS(theTransform.position, theTransform.rotation, Vector3.one);
            Gizmos.color = Color.blue;
            Gizmos.DrawFrustum(Vector3.zero, _FOV, _farPlane, _nearPlane, aspectRatio);
            Gizmos.matrix = theGizmosSavedMatrix;
            Gizmos.color = theGizmosSavedColor;
        }
    }

    //Fields

    Camera cameraForCollecting => _cameraForCollecting;
    bool isCameraForCollectingCorrect => cameraForCollecting && !cameraForCollecting.orthographic;
    float aspectRatio => _cameraForCollecting.aspect;

    [SerializeField] private Camera _cameraForCollecting = null;
    [SerializeField] private float _FOV = 60f;
    [SerializeField] private float _nearPlane = 0.3f;
    [SerializeField] private float _farPlane = 1000f;

    [SerializeField] private bool _debug_drawGizmos = true;

    private Vector3[] _XZFrustrumPlaneCorners = new Vector3[3];
    private Vector3[] _XYFrustrumPlaneCorners = new Vector3[3];
    private Vector3[] _WorldXZFrustrumPlaneCorners = new Vector3[3];
    private Vector3[] _WorldXYFrustrumPlaneCorners = new Vector3[3];

    private PopularitySource[] _collectedPopularitySourcesArray = new PopularitySource[4];
    private int _actualCollectedPopularitySourcesNum = 0;

    private PopularitySource[] _registeredPopularitySourcesArray = new PopularitySource[4];
    private int _actualRegisteredPopularitySourcesNum = 0;
}

// =========================================================================================================

static class GameMathUtils
{
    public static bool isPointInsideTriangle(Vector3 inPoint, Vector3[] inTriangle) {
        checkIsTriangleArray(inTriangle);

        //Based on implementation from here https://blackpawn.com/texts/pointinpoly/

        Vector3 v0 = inTriangle[2] - inTriangle[0];
        Vector3 v1 = inTriangle[1] - inTriangle[0];
        Vector3 v2 = inPoint - inTriangle[0];

        float dot00 = Vector3.Dot(v0, v0);
        float dot01 = Vector3.Dot(v0, v1);
        float dot02 = Vector3.Dot(v0, v2);
        float dot11 = Vector3.Dot(v1, v1);
        float dot12 = Vector3.Dot(v1, v2);

        float invDenom = 1 / (dot00 * dot11 - dot01 * dot01);
        float u = (dot11 * dot02 - dot01 * dot12) * invDenom;
        float v = (dot00 * dot12 - dot01 * dot02) * invDenom;
        return (u >= 0f) && (v >= 0f) && (u + v < 1f);
    }

    public static void calculateFrustumCenterTriangles(Vector3[] inXZPlaneCornersToFill, Vector3[] inXYPlaneCornersToFill,
        float inFrustrumFOV, float inFarPlane, float inAspectRatio)
    {
        checkIsTriangleArray(inXZPlaneCornersToFill);
        checkIsTriangleArray(inXYPlaneCornersToFill);

        var theFrustumFarHalfWidth = inFarPlane * Mathf.Tan(inFrustrumFOV * 0.5f * Mathf.Deg2Rad) * 2f;
        var theFrustumFarHalfHeight = theFrustumFarHalfWidth * inAspectRatio;

        inXZPlaneCornersToFill[0] = new Vector3(0f, 0f, 0f);
        inXZPlaneCornersToFill[1] = new Vector3(-theFrustumFarHalfWidth, 0f, inFarPlane);
        inXZPlaneCornersToFill[2] = new Vector3(theFrustumFarHalfWidth, 0f, inFarPlane);

        inXYPlaneCornersToFill[0] = new Vector3(0f, 0f, 0f);
        inXYPlaneCornersToFill[1] = new Vector3(0f, -theFrustumFarHalfHeight, inFarPlane);
        inXYPlaneCornersToFill[2] = new Vector3(0f, theFrustumFarHalfHeight, inFarPlane);
    }

    public static void checkIsTriangleArray(Vector3[] inTriangleArrayToCheck) {
        if (null == inTriangleArrayToCheck || 3 != inTriangleArrayToCheck.Length)
            throw (new System.Exception("Is not triangle array"));
    }
}

// =========================================================================================================

static class ArrayUtils
{
    public static void addToArrayAdopting<ElementType>(ref ElementType[] inoutArray, ref int inoutActualSize,
            ElementType inElementToAdd, int inReallocationSize = 4)
    {
        if (inoutActualSize == inoutArray.Length) {
            var theReallocatedArray = new ElementType[inoutActualSize + inReallocationSize];
            System.Array.Copy(inoutArray, theReallocatedArray, inoutActualSize);
            inoutArray = theReallocatedArray;
        }

        inoutArray[inoutActualSize++] = inElementToAdd;
    }

    public static void swap<Type>(ref Type inElementA, ref Type inElementB) {
        Type theTMP = inElementA;
        inElementA = inElementB;
        inElementB = theTMP;
    }

    public static void removeFromArraySwapping<ElementType>(ElementType[] inoutArray, ref int inoutActualSize,
            System.Func<ElementType, bool> inPredicate)
    {
        int theIndex = 0;
        while (theIndex < inoutActualSize) {
            ref ElementType theElementRef = ref inoutArray[theIndex];
            bool theDoRemove = inPredicate(theElementRef);
            if (theDoRemove) {
                swap(ref inoutArray[--inoutActualSize], ref theElementRef);
            } else {
                ++theIndex;
            }
        }
    }

    public static void iterateArray<ElementType>(ElementType[] inArray, int inActualSize,
            System.Action<ElementType> inPredicate)
    {
        for (int theIndex = 0; theIndex < inActualSize; ++theIndex)
            inPredicate(inArray[theIndex]);
    }

    public static void setFromArrayProcessingChanges<ElementType>(
        ref ElementType[] inoutSetToArray, ref int inoutSetToActualSize,
        ElementType[] inSetFromArray, int inSetFromArrayActualSize,
        System.Action<ElementType> inAddingProcessing, System.Action<ElementType> inRemovingProcessing,
        int inReallocationSize = 4)
    {
        removeFromArraySwapping(inoutSetToArray, ref inoutSetToActualSize,
            (ElementType inSetToElement) =>{
                if (-1 == System.Array.IndexOf(inSetFromArray, inSetToElement, 0, inSetFromArrayActualSize)) {
                    inRemovingProcessing(inSetToElement);
                    return true;
                } else {
                    return false;
                }
            });

        for (int theIndex = 0; theIndex < inSetFromArrayActualSize; ++theIndex) {
            ElementType theSetFromElement = inSetFromArray[theIndex];
            if (-1 == System.Array.IndexOf(inoutSetToArray, theSetFromElement, 0, inoutSetToActualSize)) {
                addToArrayAdopting(ref inoutSetToArray, ref inoutSetToActualSize, theSetFromElement);
                inAddingProcessing(theSetFromElement);
            }
        }
    }
}
