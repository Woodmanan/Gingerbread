using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private int timeToEatCandy = 10;
    private double eatingCandyTimer;

    private bool isHeld = false;
    private bool isMoving = true;

    private int witchKidnapRange = 2;

    private GameObject theWitch;

    private KeyCode pickUpKey;

    [SerializeField] private Candy.CandyColor[] RecognizedColors;

    private ParticleSystem particles;

    //The delay between when the gameojbect attempts to get new targets
    [SerializeField] private float SearchingDelay = 2f;
    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();

        randomLocations = GameObject.FindGameObjectsWithTag("RandomLocation");
        toRandomDirection();

        StartCoroutine(SearchForTargets(SearchingDelay));

        if (theWitch == null)
        {
            theWitch = GameObject.FindGameObjectWithTag("Player");

            if (theWitch != null)
            {
                pickUpKey = theWitch.GetComponent<ObjectPickup>().GetKey();
            }

            
        }

        if (_navMeshAgent == null)
        {
            Debug.LogError("Game object not attached: " + gameObject.name);
        }
        else
        {
            SetDestination();
        }

        particles = GetComponent<ParticleSystem>();

    }

    IEnumerator SearchForTargets(float delay)
    {
        //Random wait, to offset all actors. Helps keep the framerate up.
        yield return new WaitForSeconds(Random.Range(0, delay));
        while (true)
        {
            yield return new WaitForSeconds(delay);
            randomLocations = GameObject.FindGameObjectsWithTag("RandomLocation");
            if (randomLocations.Length == 0)
            {
                //Stop moving if our target disappears!
                currentGoal = transform;
                SetDestination();
            }

            /*
            print("Targets: ");
            foreach (GameObject g in randomLocations)
            {
                print(g.name);
            }*/
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
            else if (currentGoal == null || !currentGoal.CompareTag("RandomLocation"))
            {
                toRandomDirection();


            }
            else if (Vector3.Distance(transform.position, currentGoal.transform.position) < 2)
            {

                toRandomDirection();

            }
        }


    }

    // Update is called once per frame
  

    private void SetDestination()
    {
        if (currentGoal)
        {
            Vector3 goalVector = currentGoal.position;
            _navMeshAgent.SetDestination(goalVector);
        }


    }

    private void toRandomDirection()
    {
        if (randomLocations.Length == 0)
        {
            currentGoal = transform;
            return;
        }

        //Slightly different math, guarantees a uniquely different choice without
        //making all children follow the same path
        randNum += Random.Range(0, randomLocations.Length - 2) + 1;
        
        while (randNum >= randomLocations.Length)
        {
            randNum -= randomLocations.Length;
            if (randNum < 0)
            {
                randNum = 0;
            }
        }
        currentGoal = randomLocations[randNum].transform;
        SetDestination();
        



    }
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Candy"))
        {
            Candy candy = other.GetComponent<Candy>();
            if (!candy.visited && InterestedIn(candy.color))
            {
                currentGoal = other.transform;
                gettingCandy = true;
                SetDestination();
            }


        }
    }

    private bool InterestedIn(Candy.CandyColor color)
    {
        return RecognizedColors.Contains(color);
    }

    private void EatingCandy()
    {
        if (!currentGoal.gameObject.active)
        {
            currentGoal = null;

            GetComponent<NavMeshAgent>().isStopped = false;
            particles.Stop();
            hasCandy = false;
            return;
        }
        eatingCandyTimer += Time.deltaTime;

        GetComponent<NavMeshAgent>().isStopped = true;
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if (eatingCandyTimer > timeToEatCandy)
        {

            eatingCandyTimer = 0;
            if (currentGoal)
            {
                if (currentGoal.CompareTag("Candy"))
                {
                    //GameObject obj = ObjectPlacement.instance.PickUp(transform.position);
                    print("Child ate " + currentGoal.gameObject);
                    Destroy(currentGoal.gameObject);
                    particles.Stop();
                }
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

                    if (theWitch.GetComponent<ObjectPickup>().GetHoldingChild())
                    {

                    }
                    else
                    {
                        //Code from when child picked themselves up
                        //Keeping around because it could be good for making struggling child!
                        /*
                        transform.parent = theWitch.transform;
                        transform.position = theWitch.transform.position + new Vector3(0, 2, 0);
                        Destroy(transform.GetComponent<NavMeshAgent>());
                        theWitch.GetComponent<ObjectPickup>().SetHoldingChild(true);
                        Destroy(currentGoal.gameObject);
                        isHeld = true;
                        */
                    }
                    Debug.Log("successful kidnapping");
                    


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
            particles.Stop();

        }
        else if (Vector3.Distance(transform.position, currentGoal.transform.position) < 2)
        {

            currentGoal.gameObject.GetComponent<Candy>().visited = true;
            gettingCandy = false;
            hasCandy = true;
            particles.Play();
        }

    }


    private void Kidnapped()
    {
        if (Input.GetKeyDown(pickUpKey))
        {

            Destroy(transform.gameObject);
            theWitch.GetComponent<ObjectPickup>().SetHoldingChild(false);



        }

    }

    //Function that ties up loose ends within the child
    public void GetGrabbed()
    {
        Destroy(transform.GetComponent<NavMeshAgent>());
        Candy currentCandy = currentGoal.GetComponent<Candy>();
        if (currentCandy)
        {
            currentCandy.visited = false;
        }
        particles.Stop();
    }

    //Should we be allowed to grab this child?
    public bool CanBeGrabbed()
    {
        return hasCandy;
    }
}
