using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private GameObject held;

    [SerializeField] private KeyCode pickupKey;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pickupKey))
        {
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
    
    public KeyCode GetKey()
    {
        return pickupKey;
    }
    
}
