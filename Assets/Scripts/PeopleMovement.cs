using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class PeopleMovement : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;

    [SerializeField] private float radiusWalking;
    [SerializeField] private float chanceToGo;
    [SerializeField] private NPCAnimator _npcAnimator;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToRandomPosition (int sec)
    { 
        int i = Random.Range(0, 101);
        if (i <= chanceToGo)
        {
            Vector3 curPos = transform.position;
            Vector3 randomPosition = new Vector3(Random.Range(curPos.x - radiusWalking, curPos.x + radiusWalking), curPos.y, Random.Range(curPos.z - radiusWalking, curPos.z + radiusWalking));
            _navMeshAgent.SetDestination(randomPosition);
            _npcAnimator.Run();
        }
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player") {
            _npcAnimator.Die((() => Destroy(gameObject)));
        }
    }
}
