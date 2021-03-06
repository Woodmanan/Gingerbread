﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrows : MonoBehaviour
{
    // Start is called before the first frame update

    GameObject player;
    private float radius = 70;
    private double viewRadius = 8;
    GameObject child;
    bool reset = true;
    public GameObject childIcon;
    private GameObject manager;

    void Start()
    {

        player = GameObject.FindGameObjectWithTag("Player");
        manager = GameObject.FindGameObjectWithTag("GameController");
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(Screen.width.ToString() + " " + Screen.height.ToString());
        if (reset)
        {
            resetIcons();
        }
       

    }

    void resetIcons()
    {
        int m = 0;
        foreach (Transform child in transform.GetChild(2))
        {
            if (m > 0)
            {
                if (child.gameObject != null)
                {
                    Destroy(child.gameObject);
                }
            }
            m += 1;
        }

        GameObject[] children = manager.GetComponent<ChildControl>().getChildren();

        foreach (GameObject i in children)
        {
            GameObject newCompass = Instantiate(childIcon, transform.GetChild(2));
            newCompass.SetActive(true);
            newCompass.GetComponent<Icon>().target = i.transform;
            newCompass.GetComponent<Icon>().radius = radius;
            newCompass.GetComponent<Icon>().start = true;
            newCompass.GetComponent<Icon>().viewRadius = viewRadius;
            
        }

        if (children.Length != 0)
        {
            reset = false;
        }
        

    }


}
