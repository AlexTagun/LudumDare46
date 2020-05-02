using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PeopleMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent = null;

    [SerializeField] private float radiusWalking = 0f;
    [SerializeField] private float chanceToGo = 0f;
    [SerializeField] private NPCAnimator _npcAnimator = null;
    private void Awake()
    {
        if(gameObject.GetComponent<NavMeshAgent>() != null) {
            _navMeshAgent = GetComponent<NavMeshAgent>();
        } else
        {
            gameObject.AddComponent<NavMeshAgent>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
        };

        EventManager.OnSecondTick += GoToRandomPosition;
    }

    public void GoToRandomPosition (int sec)
    { 
        int i = Random.Range(0, 101);
        if (i <= chanceToGo)
        {
            Vector3 curPos = transform.position;
            Vector3 randomPosition = new Vector3(Random.Range(curPos.x - radiusWalking, curPos.x + radiusWalking), curPos.y, Random.Range(curPos.z - radiusWalking, curPos.z + radiusWalking));
            if (!_navMeshAgent.SetDestination(randomPosition))
                Debug.Log("[Incorrect SetDestination for]-------> " + gameObject.name);
            _npcAnimator.Run();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            _npcAnimator.Die((() => Destroy(gameObject)));
        }
    }
}
