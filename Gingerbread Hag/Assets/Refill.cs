using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refill : MonoBehaviour
{
    private GameObject held;
    [SerializeField] private GameObject toSpawn;
    [SerializeField] private float respawnDelay;
    private PickupOverride over;
    
    // Start is called before the first frame update
    void Start()
    {
        over = GetComponent<PickupOverride>();
        StartCoroutine(WaitToStart(1));
    }

    IEnumerator WaitToStart(float timer)
    {
        yield return new WaitForSeconds(timer);
        GameObject temp = Instantiate(toSpawn);
        ObjectPlacement.instance.Drop(temp, over.getLocation());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PickedUp(GameObject obj)
    {
        held = obj;
    }

    public void Dropped()
    {
        held = null;
        StartCoroutine(Refresh(respawnDelay));
    }

    IEnumerator Refresh(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (!held)
        {
            GameObject temp = Instantiate(toSpawn);
            ObjectPlacement.instance.Drop(temp, over.getLocation());
        }
    }
}
