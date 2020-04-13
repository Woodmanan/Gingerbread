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

    private bool isHeld = false;
    private bool isMoving = true;

    private int witchKidnapRange = 5;

    public GameObject theWitch;

    private KeyCode pickUpKey;
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        randomLocations = GameObject.FindGameObjectsWithTag("RandomLocation");
        randNum = (int)Random.Range(0, randomLocations.Length-1);
        currentGoal = randomLocations[randNum].transform;

        if (theWitch != null)
        {

            pickUpKey = theWitch.GetComponent<ObjectPickup>().GetKey();

            
        }

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
        if (isHeld)
        {
            Kidnapped();
        }
        else
        {
            if (gettingCandy)
            {

                GettingCandy();
            }
            else if (hasCandy)
            {


                EatingCandy();


            }
            else if (currentGoal == null)
            {
                toRandomDirection();


            }
            else if (Vector3.Distance(transform.position, currentGoal.transform.position) < 1)
            {

                toRandomDirection();

            }
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
        randNum += 1;

        if (randNum >= randomLocations.Length)
        {

            randNum = 0;

        }
        currentGoal = randomLocations[randNum].transform;
        SetDestination();
        



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

    private void EatingCandy()
    {
        eatingCandyTimer += Time.deltaTime;

        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if (eatingCandyTimer > timeToEatCandy)
        {

            eatingCandyTimer = 0;
            if (currentGoal != null)
            {
                Destroy(currentGoal.gameObject);
            }

            currentGoal = null;

            GetComponent<NavMeshAgent>().isStopped = false;

            hasCandy = false;


            toRandomDirection();
        }
        else if (theWitch != null)
        {
            if (Vector3.Distance(theWitch.transform.position, transform.position) < witchKidnapRange)
            {

                if (Input.GetKeyDown(pickUpKey))
                {

                    
                    transform.parent = theWitch.transform;
                    transform.localPosition = new Vector3(0, 5, 0);
                    


                }
            }


        }

    }

    private void GettingCandy()
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


    private void Kidnapped()
    {
        if (Input.GetKeyDown(pickUpKey))
        {

            Destroy(transform.gameObject);



        }

    }
}
