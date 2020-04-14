using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Input Variables")]
    [SerializeField] private string HorizontalAxis;
    [SerializeField] private string VerticalAxis; 
    [SerializeField] private KeyCode SprintKey;

    [Header("Control Variables")] [SerializeField]
    private float speed;

    [SerializeField] private float sprintModifier;

    private Rigidbody rig;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputDir = new Vector2(Input.GetAxis(HorizontalAxis), Input.GetAxis(VerticalAxis));
        if (inputDir.magnitude > 1)
        {
            inputDir = inputDir.normalized;
        }

        if (inputDir.magnitude > .1)
        {
            float angle = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
        
        
        Vector3 movement = new Vector3(inputDir.x, rig.velocity.y, inputDir.y) * speed;
        if (Input.GetKey(SprintKey))
        {
            movement = movement * sprintModifier;
        }
        rig.velocity = movement;
    }
}
