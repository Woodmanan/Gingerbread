using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HGFood : MonoBehaviour
{

    public bool hansel;
    public bool gretel;
    
    // Start is called before the first frame update
    void Start()
    {
        if (hansel)
        {
            GameMananger.instance.cookHansel();
        }
        else if (gretel)
        {
            GameMananger.instance.cookGretel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
