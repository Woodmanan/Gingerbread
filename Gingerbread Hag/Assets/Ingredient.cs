using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public Cooking.cooktype requires;

    public float durationOfCooking;

    [SerializeField] private GameObject becomes;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject FinishCooking()
    {
        GameObject next = Instantiate(becomes, transform.position, transform.rotation);
        return next;
    }
}
