using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickupOverride : MonoBehaviour
{
    [SerializeField] private Vector3 offset;

    [SerializeField] private ObjectEvent OnPickup;

    [SerializeField] private UnityEvent OnDrop;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Register());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Register()
    {
        while (!ObjectPlacement.instance.RegisterOverride(this))
        {
            print("Attempting Register");
            yield return new WaitForSeconds(.1f);
        }

        print(gameObject.name + " registered");
    }

    public Vector3 getLocation()
    {
        return transform.position + offset;
    }

    public void PassObject(GameObject obj)
    {
        OnPickup.Invoke(obj);
    }

    public void GiveObject()
    {
        OnDrop.Invoke();
    }

    public void Clear()
    {
        print("Pickup Override is clearing point: " + getLocation());
        ObjectPlacement.instance.ClearPoint(getLocation());
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(getLocation(), .3f);
    }
}
