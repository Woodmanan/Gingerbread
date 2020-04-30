using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
public class Hunter : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject player;
    NavMeshAgent _navMeshAgent;

    public GameObject randomLocation_0;
    public GameObject randomLocation_1;
    public GameObject randomLocation_2;
    public GameObject randomLocation_3;
    public int distanceFromGoal = 3;
    private int currentGoal = 0;

    private int witchRange = 20;
    private bool attackWitch = false;

    void Start()
    {
        _navMeshAgent = this.GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = 6;

        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
       
        
        if (attackWitch == false)
        {
            if (currentGoal == 0)
            {
                _navMeshAgent.SetDestination(randomLocation_0.transform.position);
            }
            else if (currentGoal == 1)
            {
                _navMeshAgent.SetDestination(randomLocation_1.transform.position);
            }
            else if (currentGoal == 2)
            {
                _navMeshAgent.SetDestination(randomLocation_2.transform.position);
            }
            else if (currentGoal == 3)
            {
                _navMeshAgent.SetDestination(randomLocation_3.transform.position);
            }

            if (_navMeshAgent.remainingDistance < distanceFromGoal)
            {
                int lastGoal = (int)currentGoal;

                while (currentGoal == lastGoal)
                {
                    currentGoal = Random.Range(0, 4);
                }
            }
        }
        else
        {
            _navMeshAgent.SetDestination(player.transform.position);
            if (Vector3.Distance(transform.position, player.transform.position) < 2)
            {
                player.GetComponent<ObjectPickup>().DropHeld();
                attackWitch = false;
            }
        }


        if (player.GetComponent<ObjectPickup>().GetHeld() != null)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < 10)
            {


                Debug.Log("I SHOULD BRUTULIZE THE WITCH" + " " + player.GetComponent<ObjectPickup>().GetHeld().tag);
                attackWitch = true;
            }
            
            
        }
        else
        {
            attackWitch = false;
        }
            

            

       


    }
}
