using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe : MonoBehaviour
{
    // Start is called before the first frame update
    public string firstIngredient;
    public string secondIngredient;
    public string process;
    public string result;

    void Start()
    {
        
    }

    // Update is called once per frame

    public Recipe(string firstIngredient, string secondIngredient, string process, string result)
    {
        this.firstIngredient = firstIngredient;
        this.secondIngredient = secondIngredient;
        this.process = process;
        this.result = result;

    }

    
}
