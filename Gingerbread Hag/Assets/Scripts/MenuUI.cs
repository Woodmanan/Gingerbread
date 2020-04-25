using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public void LevelOne()
    {
        SceneManager.LoadScene(1);
    }

    public void Leave()
    {
        Debug.Log("exit game");
        Application.Quit();
    }
}
