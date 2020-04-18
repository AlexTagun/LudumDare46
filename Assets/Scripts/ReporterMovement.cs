using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReporterMovement : MonoBehaviour
{
    //[SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private LineRenderer _lineRenderer;

    [SerializeField] private float _speed;

    private Vector3 nextWayPoint;
    private Vector3 curNextWayPoint;
    private int curIndexWayPoint = 0;
    private void Awake()
    {
        //_navMeshAgent = GetComponent<NavMeshAgent>();
    }


    void Update()
    {

        MoveOnRoad ();
    }

    public void MoveOnRoad ()
    {
        if (transform.position == nextWayPoint)
        {
            //_navMeshAgent.destination =_lineRenderer.GetPosition(i);
            if (curIndexWayPoint < _lineRenderer.positionCount-1)
            curIndexWayPoint++;
            nextWayPoint = _lineRenderer.GetPosition(curIndexWayPoint);
            transform.LookAt(nextWayPoint);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, nextWayPoint, Time.deltaTime * _speed);
        }
    }


}
