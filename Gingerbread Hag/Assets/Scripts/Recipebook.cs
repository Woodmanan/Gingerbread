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



    public Texture Candyapple;
    public Texture Cubes;
    public Texture Pot;
    public Texture Cauldron;
    public Texture Hanselplate;
    public Texture Gretelplate;
    public Texture Sweets;
    public Texture Tart;
    public Texture Pie;
    public Texture Flour;
    public Texture Sorbet;
    public Texture Sugarcane;
    
    void Start()
    {
        recipes = new Dictionary<string, List<string>>();
        dropdown = transform.GetChild(0).GetComponent<Dropdown>();
        dropdown.onValueChanged.AddListener(delegate {
            DropDownValueChanged();
        });
        dropdown.value = 0;
        //  AddRecipe(string firstIngredient, string secondIngredient, string process, string result)

        AddRecipe("Sugar", "Flour", "Pot", "Pie"); //two ingredients with mixing instructions and result
        AddRecipe("Sugarcane", "none", "Pot", "Sugar"); // 1 ingredient with cooking instructions and result

        currentRecipe = "Pie";

        DisplayRecipe(currentRecipe);


    }
    private Texture GetTexture(string textureName)
    {
        switch(textureName)
        {
            case "Candyapple":
                return Candyapple;
            case "Sugar":
                return Cubes;
            case "Sugarcane":
                return Sugarcane;
            case "Cauldron":
                return Cauldron;
            case "Sorbet":
                return Sorbet;
            case "Pot":
                return Pot;
            case "Hanselplate":
                return Hanselplate;
            case "Gretelplate":
                return Gretelplate;
            case "Flour":
                return Flour;
            case "Pie":
                return Pie;
            case "Tart":
                return Tart;
            case "Sweets":
                return Sweets;

        }

        return null;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            int len = dropdown.options.Count;
            
            if (dropdown.value < len - 1)
            {
                dropdown.value += 1;
            }
            else
            {
                dropdown.value = 0;
            }
        }
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

    
    void DropDownValueChanged()
    {

        currentRecipe = dropdown.options[dropdown.value].text;
        DisplayRecipe(currentRecipe);
        


    }

}
