using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyChildren : MonoBehaviour
{
    [SerializeField]
    private float destroyDistance;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Clean());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Child"))
        {
            Destroy(other);
        }
    }

    IEnumerator Clean()
    {
        while (true)
        {
            yield return new WaitForSeconds(.5f);
            print("Checking!");
            foreach (GameObject g in ChildControl.instance.getChildren())
            {
                if (Vector3.Distance(g.transform.position, transform.position) <= destroyDistance)
                {
                    Destroy(g);
                }
            }
        }
    }
    
}
