using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public enum CandyColor
    {
        Red,
        Orange,
        Yellow,
        Green,
        Cyan,
        Blue,
        Purple
    }

    public CandyColor color;
    
    
    // Start is called before the first frame update
    public bool visited = false;
    public bool canBeSeenByChildren = true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
}
