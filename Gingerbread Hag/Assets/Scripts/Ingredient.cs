using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : MonoBehaviour
{
    [Serializable] public struct SingleRecipe
    {
        public Cooking.cooktype requires;

        public float durationOfCooking;

        public GameObject becomes;
    }

    public enum CombinesInto
    {
        Flour,
        Sugar
    }

    [Serializable] public struct CombineRecipe
    {
        public CombinesInto mixedWith;
        public GameObject creates;
    }
    
    public SingleRecipe[] myRecipes;
    public CombineRecipe[] combinedRecipes;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float getCooktime(Cooking.cooktype type)
    {
        foreach (SingleRecipe s in myRecipes)
        {
            if (s.requires == type)
            {
                return s.durationOfCooking;
            }
        }

        return -1;
    }

    public GameObject combinesWith(string tag)
    {
        CombinesInto type;
        switch (tag)
        {
            case "Flour":
                type = CombinesInto.Flour;
                break;
            case "Sugar":
                type = CombinesInto.Sugar;
                break;
            default:
                return null;
        }
        
        //Search matches
        foreach (CombineRecipe c in combinedRecipes)
        {
            if (c.mixedWith == type)
            {
                return c.creates;
            }
        }

        return null;
    }

    public GameObject FinishCooking(Cooking.cooktype type)
    {
        foreach (SingleRecipe s in myRecipes)
        {
            if (s.requires == type)
            {
                GameObject next = Instantiate(s.becomes);
                next.transform.position = transform.position;
                return next;
            }
        }
        
        print("Cooking finished, but no recipe of correct type was found?");
        return null;
    }
}
