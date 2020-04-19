using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private int timeToChangePoint;

    [SerializeField] private Transform[] pointsToMove;

    private NavMeshAgent _navMeshAgent;
    // Start is called before the first frame update
    private void Awake()
    {
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        }
        else
        {
            gameObject.AddComponent<NavMeshAgent>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        };

        EventManager.OnSecondTick += GoToRandomPoint;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToRandomPoint (int sec)
    {
        if (sec % timeToChangePoint == 0)
        {
            int randomIndex = Random.Range(0, pointsToMove.Length);
            _navMeshAgent.SetDestination(pointsToMove[randomIndex].transform.position);
            // Debug.Log(pointsToMove[randomIndex].transform.position);
        }
    }

    private void OnCollisionEnter(Collision other) {
        Debug.Log(other.gameObject.name);
        if (other.gameObject.tag == "Player") {
            EventManager.HandleOnEndGame(EventManager.EndGameType.Die);
        }
    }
}
