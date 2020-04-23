﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private GameObject held;

    [SerializeField] private KeyCode pickupKey;

    [SerializeField] private float pickupDistance;
    private Vector3 inFront;
    
    [SerializeField] private bool gridLogicActive;

    public AudioClip pickupSFX;

    public AudioClip placeSFX;

    public bool holdingChild;
    // Start is called before the first frame update
    void Start()
    {
        
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
                    //Attempt drop
                    if (ObjectPlacement.instance.Drop(held, inFront))
                    {
                        held.SetActive(true);
                        held = null;
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
                        //Halt, because the children are currently the ones who signal use
                        held = child;
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
                        held = ObjectPlacement.instance.PickUp(inFront);
                        if (held)
                        {
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
}