using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipebook : MonoBehaviour
{
    // Start is called before the first frame update

    Dictionary<string, List<string>> recipes;
    Dropdown dropdown;
    string currentRecipe;
   

    public Texture  apple;
    public Texture orange;
    public Texture pot;
    public Texture stove;
    
    void Start()
    {
        recipes = new Dictionary<string, List<string>>();
        dropdown = transform.GetChild(0).GetComponent<Dropdown>();

        dropdown.onValueChanged.AddListener(delegate {
            DropDownValueChanged();
        });

        dropdown.value = 0;

        //  AddRecipe(string firstIngredient, string secondIngredient, string process, string result)

        AddRecipe("Apple", "Apple", "Stove", "Apple"); //two ingredients with mixing instructions and result
        AddRecipe("Orange", "none", "Pot", "Orange"); // 1 ingredient with cooking instructions and result

        currentRecipe = "Apple";

        DisplayRecipe(currentRecipe);


    }

    public void AddRecipe(string firstIngredient, string secondIngredient, string process, string result)
    {
        recipes[result] = new List<string>();

        recipes[result].Add(firstIngredient);
        recipes[result].Add(secondIngredient);
        recipes[result].Add(process);
        recipes[result].Add(result);
        
        dropdown.ClearOptions();

        List<string> recipenames = new List<string>();


        foreach (string i in recipes.Keys)
        {
            recipenames.Add(i);
        }

        dropdown.AddOptions(recipenames);
    }

    

    private void DisplayRecipe(string recipeName)
    {

        List<string> currentRecipe = recipes[recipeName];

        GameObject firstTree = transform.GetChild(1).gameObject;
        GameObject secondTree = transform.GetChild(2).gameObject;

        if (currentRecipe[1] == "none")
        {
           
            firstTree.SetActive(false);
            secondTree.SetActive(true);

            RawImage firstIngredient = secondTree.transform.GetChild(0).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(recipes[recipeName][0]);


            RawImage process = secondTree.transform.GetChild(1).GetComponent<RawImage>();
            process.texture = GetTexture(recipes[recipeName][2]);


            RawImage result = secondTree.transform.GetChild(2).GetComponent<RawImage>();
            result.texture = GetTexture(recipes[recipeName][3]);



        }
        else
        {
            
            firstTree.SetActive(true);
            secondTree.SetActive(false);

            RawImage firstIngredient = firstTree.transform.GetChild(0).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(recipes[recipeName][0]);

            RawImage secondIngredient = firstTree.transform.GetChild(1).GetComponent<RawImage>();
            secondIngredient.texture = GetTexture(recipes[recipeName][1]);


            RawImage process = firstTree.transform.GetChild(2).GetComponent<RawImage>();
            process.texture = GetTexture(recipes[recipeName][2]);


            RawImage result = firstTree.transform.GetChild(3).GetComponent<RawImage>();
            result.texture = GetTexture(recipes[recipeName][3]);

            Debug.Log(recipes[recipeName][3]);

        }

    }

    private Texture GetTexture(string name)
    {

        if (name == "Apple")
        {
            return apple;
        }
        else if(name == "Orange")
        {
            return orange;
        }
        else if(name == "Pot")
        {
            return pot;
        }
        else if(name == "Stove")
        {
            return stove;
        }

        return null;


    }
    
    void DropDownValueChanged()
    {

        currentRecipe = dropdown.options[dropdown.value].text;
        DisplayRecipe(currentRecipe);
        


    }

}
