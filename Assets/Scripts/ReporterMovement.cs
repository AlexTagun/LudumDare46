﻿using UnityEngine;
using UnityEngine.AI;

public class ReporterMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent = null;

    public LineRenderer CurrentPath;
    public bool _canRunning = false;

    private Vector3 nextWayPoint;
    private int curIndexWayPoint = 0;

    void Update() {
        if (_canRunning)
            MoveOnRoad();
    }

    public void MoveOnRoad () {
        if (curIndexWayPoint == 0) {
            nextWayPoint = CurrentPath.GetPosition(curIndexWayPoint);
            _navMeshAgent.SetDestination(nextWayPoint);
            curIndexWayPoint++;
        } else {
            if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance) {
                if (curIndexWayPoint <= CurrentPath.positionCount - 1) {
                    nextWayPoint = CurrentPath.GetPosition(curIndexWayPoint);
                    _navMeshAgent.SetDestination(nextWayPoint);
                    curIndexWayPoint++;
                } else {
                    _canRunning = false;
                    curIndexWayPoint = 0;
                }
            }
        }
    }
}
