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
        {
            if (held)
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
            else
            {
                held = ObjectPlacement.instance.PickUp(inFront);
                if (held)
                {
                    held.SetActive(false);
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
}
