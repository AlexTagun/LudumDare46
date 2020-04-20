﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ReporterMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    public LineRenderer CurrentPath;
    public bool _canRunning = false;

    private Vector3 nextWayPoint;
    private int curIndexWayPoint = 0;
    private void Awake()
    {
    }

    private void Start()
    {
    }

    void Update()
    {
        if (_canRunning)
        {
            MoveOnRoad();
        }
    }

    public void MoveOnRoad ()
    {

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            if (curIndexWayPoint < CurrentPath.positionCount - 1)
            {
                curIndexWayPoint++;
                nextWayPoint = CurrentPath.GetPosition(curIndexWayPoint);
                _navMeshAgent.SetDestination(nextWayPoint);
            }
            else
            {
                _canRunning = false;
                curIndexWayPoint = 0;
                //anim Idle
            }
        }
    }
}
