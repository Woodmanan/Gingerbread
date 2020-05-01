using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private GameObject held;

    [SerializeField] private KeyCode pickupKey;

    [SerializeField] private float pickupDistance;
    private Vector3 inFront;
    
    [SerializeField] private bool gridLogicActive;

    [SerializeField] private Vector3 HoldingOffset;
    private GameObject holdDisplay;
    private MeshFilter displayMesh;
    private MeshRenderer displayRender;
    private Animator anim;

    public AudioClip pickupSFX;

    public AudioClip placeSFX;

    public bool holdingChild;

    [SerializeField] float dropDist;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        inFront = transform.position + transform.forward * pickupDistance;
        ObjectPlacement.instance.SignalPosition(inFront);
        
        /*if (holdingChild) //the "child" is a child of the player transform
        {
            if (Input.GetKeyDown(pickupKey));
            {
                print("Location: " + inFront);
                //Attempt drop
                if (ObjectPlacement.instance.Drop(held, inFront))
                {
                    held.SetActive(true);
                    held = null;
                }
                else
                {
                    print("Hwat.");
                }

            }

        }
        else
        {*/
        
            if (Input.GetKeyDown(pickupKey))// && gridLogicActive)
            {
                if (held)
                {
                    print("Location: " + transform.position);
                    GetComponent<AudioSource>().PlayOneShot(placeSFX);
                    
                    ChildBeta child = held.GetComponent<ChildBeta>();
                    if (child)
                    {
                        //Holding a child! See if it needs to become free roaming
                        NavMeshHit hit;
                        if (NavMesh.SamplePosition(inFront, out hit, dropDist, NavMesh.AllAreas))
                        {
                            //Drop the child!
                            print("Child is getting dropped into Navmesh!");
                            held.transform.position = inFront;
                            held.SetActive(true);
                            held.GetComponent<NavMeshAgent>().enabled = true;
                            held.GetComponent<BoxCollider>().enabled = true;
                            child.enabled = true;
                            child.StopGrab();
                            held = null;
                            print("Child has finished being dropped!");
                            Destroy(holdDisplay);
                            anim.SetTrigger("Place");
                            anim.SetBool("Holding", false);
                            return;
                        }
                    }
                    
                    //Attempt drop
                    if (ObjectPlacement.instance.Drop(held, inFront))
                    {
                        held.SetActive(true);
                        held = null;
                        anim.SetTrigger("Place");
                        anim.SetBool("Holding", false);
                        Destroy(holdDisplay);
                    }
                    else
                    {
                        print("Hwat. How has this happened.");
                    }
                }
                else
                {
                    
                    GameObject child = ChildNearby();
                    //Check for child pickup
                    if (child)
                    {
                        GetComponent<AudioSource>().PlayOneShot(pickupSFX);
                        anim.SetTrigger("Grab");
                        //Halt, because the children are currently the ones who signal use
                        Pickup(child);

                        GetComponent<AudioSource>().PlayOneShot(pickupSFX);
                        //Child currently doesn't do anything once picked up, so we disable it
                        ChildBeta childComp = child.GetComponent<ChildBeta>();
                        childComp.GetGrabbed();
                        //TODO: Figure out if this is necessary?
                        //Could be fun to have the child struggle once you pick them up
                        childComp.enabled = false;
                        
                        child.GetComponent<BoxCollider>().enabled = false;
                        child.GetComponent<Rigidbody>().useGravity = false;
                        child.tag = "Untagged";
                        child.SetActive(false);
                        
                    }
                    else
                    {
                        //No child found, check pick up a game object instead
                        GameObject found = ObjectPlacement.instance.PickUp(inFront);
                        if (found)
                        {
                            anim.SetTrigger("PickUp");
                            Pickup(found);
                        }
                        
                        
                        if (held)
                        {
                            anim.SetBool("Holding", true);
                            GetComponent<AudioSource>().PlayOneShot(pickupSFX);
                            held.SetActive(false);
                        }
                    }
                }
            }
        
    }

    private GameObject ChildNearby()
    {
        //Childcontrol keeps a fast, well-updated list of children.
        //More efficient than Gameobject.Find
        GameObject[] children = ChildControl.instance.getChildren();
        foreach (GameObject g in children)
        {
            if (Vector3.Distance(g.transform.position, transform.position) < 2)
            {
                if (g.GetComponent<ChildBeta>().CanBeGrabbed())
                {
                    return g;
                }
            }
        }

        return null;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        inFront = transform.position + transform.forward * pickupDistance;
        Gizmos.DrawWireSphere(inFront, .3f);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + HoldingOffset, .1f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position + HoldingOffset, dropDist);
    }
    
    public KeyCode GetKey()
    {
        return pickupKey;
    }

    public void SetHoldingChild(bool holding)
    {
        holdingChild = holding;
    }
    
    public bool GetHoldingChild()
    {
        return holdingChild;
    }

    private void Pickup(GameObject obj)
    {
        held = obj;
        holdDisplay = Instantiate(obj);

        //Hardcode to kill things that must die
        //I am very sorry Chileshe, I tried the clean way and Unity did not like it
        ChildBeta c = holdDisplay.GetComponentInChildren<ChildBeta>();
        if (c)
        {
            c.enabled = false;
        }

        Collider collider = holdDisplay.GetComponentInChildren<Collider>();
        if (collider)
        {
            Destroy(collider);
        }


        Candy candy = holdDisplay.GetComponentInChildren<Candy>();
        if (candy)
        {
            Destroy(candy);
        }
        
        holdDisplay.transform.parent = transform;
        holdDisplay.transform.localPosition = HoldingOffset;


    }

    public void DropHeld()
    {

        print("Location: " + transform.position);
        GetComponent<AudioSource>().PlayOneShot(placeSFX);
        
        
        //Attempt drop
        if (ObjectPlacement.instance.Drop(held, inFront))
        {
            held.SetActive(true);
            held = null;
            Destroy(holdDisplay);
        }
        else
        {
            print("Hwat. How has this happened.");
        }


    }


    public GameObject GetHeld()
    {


        return held;
    }
}
