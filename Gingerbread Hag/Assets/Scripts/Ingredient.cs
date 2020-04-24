using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    public AudioClip bakingSFX;
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
        GameObject next = Instantiate(becomes);
        next.transform.position = transform.position;
        if (requires == Cooking.cooktype.Baking)
        {
            next.GetComponent<AudioSource>().PlayOneShot(bakingSFX);
        }
        return next;
    }
}
