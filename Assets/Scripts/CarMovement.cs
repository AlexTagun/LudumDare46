using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private int timeToChangePoint = 0;
    [SerializeField] private Transform[] pointsToMove = null;

    private NavMeshAgent _navMeshAgent = null;

    private void Awake() {
        if (gameObject.GetComponent<NavMeshAgent>() != null) {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        } else {
            gameObject.AddComponent<NavMeshAgent>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        };

        EventManager.OnSecondTick += GoToRandomPoint;
    }

    public void GoToRandomPoint (int sec) {
        if (sec % timeToChangePoint == 0) {
            int randomIndex = Random.Range(0, pointsToMove.Length);
            _navMeshAgent.SetDestination(pointsToMove[randomIndex].transform.position);
        }
    }
}
