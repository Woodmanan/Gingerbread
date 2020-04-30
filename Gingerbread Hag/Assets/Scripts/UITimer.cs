using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UITimer : MonoBehaviour
{
    // Start is called before the first frame update
    public float maxTime = 500;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        maxTime -= Time.deltaTime;
        if (maxTime > 0)
        {
            transform.GetComponent<TextMeshProUGUI>().text = ((int)maxTime).ToString();
        }
    }
}
