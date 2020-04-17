using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Place());
    }

    IEnumerator Place()
    {
        yield return new WaitForSeconds(.1f);
        ObjectPlacement.instance.Drop(gameObject, transform.position);
        print(gameObject.name + " has dropped itself");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
