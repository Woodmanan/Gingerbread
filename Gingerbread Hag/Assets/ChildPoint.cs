using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildPoint : MonoBehaviour
{
    private GameObject held;

    private bool waiting;

    private PickupOverride over;
    // Start is called before the first frame update
    void Start()
    {
        waiting = false;
        over = GetComponent<PickupOverride>();
    }

    // Update is called once per frame
    void Update()
    {
        if (waiting && !held)
        {
            print("Child point has finished waiting, cleaning everything.");
            over.Clear();
            EndAttract();
        }
    }

    public void BeginAttract(GameObject obj)
    {
        gameObject.tag = "RandomLocation";
        waiting = true;
        held = obj;
    }

    public void EndAttract()
    {
        gameObject.tag = "Untagged";
        waiting = false;
    }
}
