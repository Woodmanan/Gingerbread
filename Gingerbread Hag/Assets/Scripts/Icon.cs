using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Icon : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform target;
    public GameObject player;
    public float radius;
    public bool start = false;
    public double viewRadius;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            MoveArrow();
        }
    }

    void MoveArrow()
    {
        if (target != null)
        {
            Vector2 TargetLocation = new Vector2(target.position.x, target.position.z);
            Vector2 PlayerLocation = new Vector2(player.transform.position.x, player.transform.position.z);
            Vector2 dist = TargetLocation - PlayerLocation;

           

            transform.localPosition = dist.normalized * radius;
        }
        else
        {
            Debug.Log("Reset Icons");
            SendMessageUpwards("resetIcons");
            Destroy(transform.gameObject);


        }



    }



}
