using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupOverride : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
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
        while (!ObjectPlacement.instance.RegisterOverride(gameObject))
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

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(getLocation(), .3f);
    }
}
