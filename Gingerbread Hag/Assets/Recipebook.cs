using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Recipebook : MonoBehaviour
{
    // Start is called before the first frame update

    Dictionary<string, Recipe> recipes;
    Dropdown dropdown;
    string currentRecipe;
    int currentRecipeTree = 1;

    public Texture  apple;
    public Texture orange;
    
    void Start()
    {
        recipes = new Dictionary<string, Recipe>();
        dropdown = transform.GetChild(0).GetComponent<Dropdown>();

        dropdown.onValueChanged.AddListener(delegate {
            DropDownValueChanged();
        });

        dropdown.value = 0;


        AddRecipe("Apple", "Apple", "Apple", "Apple");
        AddRecipe("Orange", "Orange", "Orange");

        currentRecipe = dropdown.options[0].text;


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRecipe(string firstIngredient, string secondIngredient, string process, string result)
    {
        recipes.Add(result, new Recipe(firstIngredient, secondIngredient, process, result));

        dropdown.ClearOptions();

        List<string> recipenames = new List<string>();

        foreach (string i in recipes.Keys)
        {
            recipenames.Add(i);
        }

        dropdown.AddOptions(recipenames);
    }

    public void AddRecipe(string firstIngredient, string process, string result)
    {

        recipes.Add(result, new Recipe(firstIngredient, "none", process, result));

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

        Recipe currentRecipe = recipes[recipeName];

        GameObject firstTree = transform.GetChild(1).gameObject;
        GameObject secondTree = transform.GetChild(2).gameObject;

        if (currentRecipe.secondIngredient == "none")
        {
            currentRecipeTree = 2;
            firstTree.SetActive(true);
            secondTree.SetActive(false);

            Image firstIngredient = secondTree.transform.GetChild(0).GetComponent<Image>();

            Image process = secondTree.transform.GetChild(1).GetComponent<Image>();

            Image result = secondTree.transform.GetChild(2).GetComponent<Image>();


        }
        else
        {
            currentRecipeTree = 1;
            firstTree.SetActive(false);
            secondTree.SetActive(true);

            RawImage firstIngredient = firstTree.transform.GetChild(0).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(currentRecipe.firstIngredient);

            RawImage secondIngredient = firstTree.transform.GetChild(1).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(currentRecipe.secondIngredient);


            RawImage process = firstTree.transform.GetChild(2).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(currentRecipe.process);


            RawImage result = firstTree.transform.GetChild(3).GetComponent<RawImage>();
            firstIngredient.texture = GetTexture(currentRecipe.result);

            Debug.Log(" I AM OVER HERE");

        }

    }

    private Texture GetTexture(string name)
    {

        if (name == "apple")
        {
            return apple;
        }
        else if(name == "orange")
        {
            return orange;
        }

        return null;


    }
    
    void DropDownValueChanged()
    {
        Debug.Log("Dropdown Value has Changed");
    }

}
