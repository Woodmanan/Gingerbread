using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingBoard : MonoBehaviour
{
    [SerializeField] private KeyCode choppingKey;
    private AudioSource audio;
    [SerializeField] private float distance;

    private bool readyToChop;
    private bool chopping;
    private GameObject held;
    private Ingredient ingredient;
    private float timeLeft;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        timeLeft = 0;
        readyToChop = false;
        chopping = false;
        audio = GetComponent<AudioSource>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!chopping && readyToChop && Input.GetKeyDown(choppingKey))
        {
            print("Starting chop!");
            StartChop();
        }
        else if (chopping && Input.GetKey(choppingKey))
        {
            //Has player held key?
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0)
            {
                GameObject next = ingredient.FinishCooking(Cooking.cooktype.Chopping);
                //Move it to the next one!
                ObjectPlacement.instance.Replace(transform.position, next);
                readyToChop = false;
                chopping = false;
                Destroy(held);
                GetObject(next);
            }
        }
        else
        {
            StopChop();
        }
    }

    public void GetObject(GameObject obj)
    {
        print("Object got!");
        held = obj;
        ingredient = held.GetComponent<Ingredient>();
        if (ingredient)
        {
            timeLeft = ingredient.getCooktime(Cooking.cooktype.Chopping);
            readyToChop = (timeLeft > 0);
        }
        else
        {
            readyToChop = false;
        }
        
    }

    public void LoseObject()
    {
        held = null;
        ingredient = null;
        StopChop();
    }

    private void StartChop()
    {
        if (Vector3.Distance(transform.position, player.transform.position) < distance)
        {
            chopping = true;
            audio.Play();
        }
    }

    private void StopChop()
    {
        chopping = false;
        audio.Stop();
    }
}
