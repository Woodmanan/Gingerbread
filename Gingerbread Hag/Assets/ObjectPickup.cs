using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPickup : MonoBehaviour
{
    [SerializeField] private GameObject held;

    [SerializeField] private KeyCode pickupKey;

    [SerializeField] private bool gridLogicActive;

    public bool holdingChild;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (holdingChild) //the "child" is a child of the player transform
        {
            if (Input.GetKeyDown(pickupKey));
            {

            }

        }
        else
        {
            if (Input.GetKeyDown(pickupKey) && gridLogicActive)
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
