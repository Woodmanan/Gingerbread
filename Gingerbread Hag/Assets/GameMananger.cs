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
                ResetLevel();
            }
        }
    }

    public void cookGretel()
    {
        gretelCooked = true;
        FinishGame();
    }

    public void cookHansel()
    {
        hanselCooked = true;
        FinishGame();
    }

    private void FinishGame()
    {
        if (hanselCooked && gretelCooked)
        {
            //Move to next Level?
            print("End conditions met!");
            ResetLevel();
        }
    }

    private void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void MoveToNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
