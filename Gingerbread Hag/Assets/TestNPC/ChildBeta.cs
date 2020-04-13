using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ChildBeta : MonoBehaviour
{
    // Start is called before the first frame update
    Transform currentGoal;
    private int randNum;
    GameObject[] randomLocations;
    NavMeshAgent _navMeshAgent;

    private bool gettingCandy = false;
    private bool hasCandy = false;
    private int timeToEatCandy = 6;
    private double eatingCandyTimer;

    
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        randomLocations = GameObject.FindGameObjectsWithTag("RandomLocation");
        randNum = (int)Random.Range(0, randomLocations.Length-1);
        currentGoal = randomLocations[randNum].transform;

        if (_navMeshAgent == null)
        {
            Debug.LogError("Game object not attached: " + gameObject.name);
        }
        else
        {
            SetDestination();
        }


    }

    void Update()
    {

        if (gettingCandy)
        {
            if (currentGoal.gameObject.GetComponent<Candy>().visited == true && 
                gettingCandy == true)
            {
                toRandomDirection();
                gettingCandy = false;

            }
            else if (Vector3.Distance(transform.position, currentGoal.transform.position) < 1)
            {

                currentGoal.gameObject.GetComponent<Candy>().visited = true;
                gettingCandy = false;
                hasCandy = true;
            }

        }
        else if(hasCandy)
        {
            eatingCandyTimer += Time.deltaTime;

            if (eatingCandyTimer > timeToEatCandy)
            {
                eatingCandyTimer = 0;
                Destroy(currentGoal.gameObject);
                hasCandy = false;
                toRandomDirection();
            }


        }
        else
        {

            toRandomDirection();
            
        }


    }

    // Update is called once per frame
  

    private void SetDestination()
    {
        if (currentGoal != null)
        {
            Vector3 goalVector = currentGoal.transform.position;
            _navMeshAgent.SetDestination(goalVector);
        }


    }

    private void toRandomDirection()
    {
        if (Vector3.Distance(transform.position, currentGoal.position) < 1)
        {
            randNum += 1;

            if (randNum >= randomLocations.Length)
            {

                randNum = 0;

            }
            currentGoal = randomLocations[randNum].transform;
            SetDestination();
        }



    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Candy"))
        {
            if (!other.gameObject.GetComponent<Candy>().visited)
            {
                currentGoal = other.transform;
                gettingCandy = true;
                SetDestination();
            }


        }
    }
}
