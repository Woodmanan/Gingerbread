using System;
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
        
        if (Input.GetKeyDown(pickupKey))
        if (holdingChild) //the "child" is a child of the player transform
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
                    print("Hwat the fuck.");
                }

            }

        }
        else
        {
            if (Input.GetKeyDown(pickupKey) && gridLogicActive)
            {
                held = ObjectPlacement.instance.PickUp(inFront);
                if (held)
                {
                    print("Location: " + transform.position);
                    //Attempt drop
                    if (ObjectPlacement.instance.Drop(held, transform.position))
                    {
                        held.SetActive(true);
                        held = null;
                    }
                    else
                    {
                        print("Hwat the fuck.");
                    }
                }
                else
                {
                    held = ObjectPlacement.instance.PickUp(transform.position);
                    if (held)
                    {
                        held.SetActive(false);
                    }
                }
            }
        }
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
