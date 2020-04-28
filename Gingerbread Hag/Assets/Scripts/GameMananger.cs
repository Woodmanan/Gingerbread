using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMananger : MonoBehaviour
{

    public bool hanselCooked;
    public bool gretelCooked;

    public float gameTimer;

    public TextMeshProUGUI text;

    [System.Serializable]
    public struct Colorscore
    {
        public Candy.CandyColor colorNeeded;
        public int amountNeeded;
        public int amountHad;
    }
    
    public Colorscore[] winConditions;

    private static GameMananger Instance;
    public static GameMananger instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("GameController");
                if (obj)
                {
                    Instance = obj.GetComponent<GameMananger>();
                }
                else
                {
                    GameObject inst = new GameObject();
                    inst.AddComponent(typeof(ChildControl));
                    Instance = inst.GetComponent<GameMananger>();
                }
            }

            return Instance;
        }
        set => Instance = value;
    }
    // Start is called before the first frame update
    void Start()
    {
        //Singleton code
        if (Instance != this && Instance)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = GetComponent<GameMananger>();
        }

        StartCoroutine(Countdown(gameTimer));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Countdown(float runtime)
    {
        while (true)
        {
            yield return null;
            runtime -= Time.deltaTime;
            text.text = "" + (int) runtime;
            if (runtime < 0)
            {
                //TODO: End game!
                MoveToNextLevel();
            }
        }
    }

    public void cookGretel()
    {
        gretelCooked = true;
        //FinishGame();
    }

    public void cookHansel()
    {
        hanselCooked = true;
        //FinishGame();
    }

    private void FinishGame()
    {
        print("End conditions met!");
        ReturnToMenu();
        //MoveToNextLevel();
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MoveToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void RegisterCandy(Candy.CandyColor color)
    {
        print("Registering candy of color " + color);
        //Add in the correct count
        for (int i = 0; i < winConditions.Length; i++)
        {
            Colorscore score = winConditions[i];
            if (score.colorNeeded == color)
            {
                score.amountHad = score.amountHad + 1;
                winConditions[i] = score;
            }
        }

        if (canWeWin())
        {
            FinishGame();
        }
    }

    private bool canWeWin()
    {
        print("Checking win conditions:");
        bool win = true;
        for (int i = 0; i < winConditions.Length; i++)
        {
            Colorscore score = winConditions[i];
            print("Checking item " + i + ": " + score.colorNeeded);
            if (score.amountHad < score.amountNeeded)
            {
                print("This one was not good enough.");
                win = false;
            }
            else
            {
                print("This one was good enough.");
            }
        }

        return win;
    }
}
