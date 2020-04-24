using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChildControl : MonoBehaviour
{
    [SerializeField] private float updateDelay = 3f;
    [SerializeField] int maxChildren = 5;

    [SerializeField] private float spawnDelay = 5f;
    [SerializeField] private Vector3 spawnPosition;
    
    [SerializeField] private GameObject hanselPrefab;
    [SerializeField] private GameObject gretelPrefab;

    [SerializeField] private GameObject[] childrenToSpawn;

    //Are hansel and gretel active?
    private bool hgActive = false;
    private GameObject Hansel;
    private GameObject Gretel;

    
    
    //These variables generate a singleton for me
    private static ChildControl Instance;
    public static ChildControl instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject obj = GameObject.FindGameObjectWithTag("ChildController");
                if (obj)
                {
                    Instance = obj.GetComponent<ChildControl>();
                }
                else
                {
                    GameObject inst = new GameObject();
                    inst.AddComponent(typeof(ChildControl));
                    Instance = inst.GetComponent<ChildControl>();
                }
            }

            return Instance;
        }
        set => Instance = value;
    }
    
    private List<GameObject> children;
    
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
            Instance = GetComponent<ChildControl>();
        }
        
        children = new List<GameObject>();
        //Catch
        foreach (GameObject child in GameObject.FindGameObjectsWithTag("Child"))
        {
            children.Add(child);
        }

        StartCoroutine(UpdateChildren(updateDelay));
        StartCoroutine(SpawnChildren(spawnDelay));
    }

    IEnumerator UpdateChildren(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            GameObject[] allChildren = GameObject.FindGameObjectsWithTag("Child");
            foreach (GameObject g in allChildren)
            {
                if (!children.Contains(g))
                {
                    children.Add(g);
                }
            }
        }
    }

    IEnumerator SpawnChildren(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            if (children.Count < maxChildren)
            {
                SpawnChild();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (hgActive)
        {
            if (!Hansel && !Gretel)
            {
                hgActive = false;
            }
        }
    }

    public void SpawnChild()
    {
        int index = Random.Range(0, childrenToSpawn.Length + 1);
        if (hgActive)
        {

            while (index == childrenToSpawn.Length)
            {
                index = Random.Range(0, childrenToSpawn.Length);
            }
        }

        if (index != childrenToSpawn.Length)
        {
            GameObject childToSpawn = childrenToSpawn[index];
            GameObject child = Instantiate(childToSpawn, spawnPosition, Quaternion.identity);
            children.Add(child);
        }
        else
        {
            if (!GameMananger.instance.gretelCooked)
            {
                Gretel = Instantiate(gretelPrefab, spawnPosition, Quaternion.identity);
                Gretel.name = "Gretel";
                children.Add(Gretel);
            }

            if (!GameMananger.instance.hanselCooked)
            {
                Hansel = Instantiate(hanselPrefab, spawnPosition, Quaternion.identity);
                Hansel.name = "Hansel";
                children.Add(Hansel);
            }




            if (Hansel || Gretel)
            {
                hgActive = true;
            }

        }
    }

    public GameObject[] getChildren()
    {
        Clean();
        return children.ToArray();
    }
    

    void Clean()
    {
        for (int i = children.Count - 1; i >= 0; i--)
        {
            //Remove null or picked up children
            if (!children[i] || !children[i].CompareTag("Child"))
            {
                children.RemoveAt(i);
            }
        }
    }
}
