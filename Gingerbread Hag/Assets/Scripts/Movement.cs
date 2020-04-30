using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public AudioClip cackle;
    [Header("Input Variables")]
    [SerializeField] private string HorizontalAxis;
    [SerializeField] private string VerticalAxis; 
    [SerializeField] private KeyCode SprintKey;

    [Header("Control Variables")] [SerializeField]
    private float speed;

    [SerializeField] private float sprintModifier;

    private Rigidbody rig;
    private ParticleSystem.EmissionModule emis;
    private ParticleSystem.MainModule main;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody>();
        ParticleSystem particles = GetComponent<ParticleSystem>();
        emis = particles.emission;
        main = particles.main;
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
        if (Input.GetKeyDown(SprintKey))
        {
            if (!GetComponent<AudioSource>().isPlaying)
            {
                GetComponent<AudioSource>().PlayOneShot(cackle);
            }
        }
        if (Input.GetKey(SprintKey))
        {
            movement = movement * sprintModifier;
            emis.rateOverDistance = 10;
            main.startColor = Color.magenta;
        }
        else
        {
            emis.rateOverDistance = 5;
            main.startColor = Color.white;
        }
        rig.velocity = movement;
    }


}
