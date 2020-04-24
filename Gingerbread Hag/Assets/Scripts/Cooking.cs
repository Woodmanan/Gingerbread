using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Cooking : MonoBehaviour
{
    //Our types of cooking 
    public enum cooktype
    {
        Baking,
        Boiling,
        Frying,
        Chopping
    }

    [SerializeField] private cooktype performedAction;
    private bool isCooking;

    private GameObject held;

    private Ingredient ingredient;

    private float cookTime;

    [SerializeField] private ObjectEvent onFinishCooking;
    // Start is called before the first frame update
    void Start()
    {
        isCooking = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (held && isCooking)
        {
            cookTime -= Time.deltaTime;
            print("Time left: " + cookTime);
            if (cookTime <= 0)
            {
                //Make the new one!
                GameObject next = ingredient.FinishCooking(performedAction);
                //Move it to the next one!
                ObjectPlacement.instance.Replace(transform.position, next);
                Destroy(held);
                onFinishCooking.Invoke(next);
                StartCooking(next);
            }
        }
    }
    
    public void StartCooking(GameObject obj)
    {
        print("Got an " + obj.name);
        held = obj;
        ingredient = held.GetComponent<Ingredient>();
        if (ingredient)
        {
            print("It has an ingredient in it!");
            //Fancy bool magic
            cookTime = ingredient.getCooktime(performedAction);
            isCooking = (cookTime > 0);
        }
        else
        {
            isCooking = false;
        }

        if (isCooking)
        {
            print("We can cook it!");
        }
    }

    public void StopCooking()
    {
        held = null;
        ingredient = null;
        isCooking = false;
    }
}
