using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Assertions;

public class ObjectPlacement : MonoBehaviour
{
    //These variables generate a singleton for me
    private static ObjectPlacement Instance;
    public static ObjectPlacement instance
    {
        get
        {
            if (Instance == null)
            {
                GameObject inst = new GameObject();
                inst.AddComponent(typeof(ObjectPlacement));
                Instance = inst.GetComponent<ObjectPlacement>();
            }

            return Instance;
        }
        set => Instance = value;
    }

    [SerializeField] private Vector2Int size;
    private Dictionary<Vector2, GameObject> objects;
    private Grid grid;

    private GameObject player;
    
    // Start is called before the first frame update
    void Start()
    {
        //Singleton code
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = gameObject.GetComponent<ObjectPlacement>();
        }
        objects = new Dictionary<Vector2, GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        grid = GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Vector2 gridPosition(Vector3 position)
    {
        Vector3 location = grid.WorldToCell(position);
        return new Vector2(location.x, location.z);
    }

    private GameObject OnFloor(Vector3 position)
    {
        Vector2 location = gridPosition(position);
        print("X: " + location.x + ", Y: " + location.y);
        GameObject gameobj;
        objects.TryGetValue(location, out gameobj);
        return gameobj;
    }

    public GameObject PickUp(Vector3 position)
    {
        Vector2 location = gridPosition(position);
        GameObject toReturn;
        if (objects.TryGetValue(location, out toReturn))
        {
            print("Pickup found an object!");
            objects.Remove(location);
        }
        return toReturn;
    }

    public bool Drop(GameObject obj, Vector3 position)
    {
        if (CanPlace(position))
        {
            //Place code
            Place(obj, position);
            Assert.IsTrue(OnFloor(position) == obj);
            return true;
        }
        return false;
    }

    public bool CanPlace(Vector3 position)
    {
        return (!OnFloor(position));
    }

    public void Place(GameObject obj, Vector3 position)
    {
        Vector3 location = gridPosition(position);
        objects.Add(location, obj);
        obj.transform.position = GetCellLocation(location); //new Vector3(location.x, 0, location.y);
    }

    public Vector3 GetCellLocation(Vector2 location)
    {
        return grid.GetCellCenterWorld(new Vector3Int((int) location.x, 0, (int) location.y));
    }

    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.red;
            Vector2 spot = gridPosition(player.transform.position);
            Gizmos.DrawWireCube(GetCellLocation(spot), grid.cellSize);
        }

        if (objects != null)
        {
            foreach (KeyValuePair<Vector2, GameObject> kvp in objects)
            {
                Gizmos.color = Color.blue;
                Vector2 spot = kvp.Key;
                Gizmos.DrawWireCube(GetCellLocation(spot), grid.cellSize);
            }
        }
    }
}
