using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Grid))]
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
                GameObject obj = GameObject.FindGameObjectWithTag("Grid");
                if (obj)
                {
                    Instance = obj.GetComponent<ObjectPlacement>();
                }
                else
                {
                    GameObject inst = new GameObject();
                    inst.AddComponent(typeof(ObjectPlacement));
                    Instance = inst.GetComponent<ObjectPlacement>();
                }
            }

            return Instance;
        }
        set => Instance = value;
    }

    [SerializeField] private float indicatorHeight;
    [SerializeField] private float dropHeight;
    private Dictionary<Vector2, GameObject> objects;
    private Dictionary<Vector2, PickupOverride> overrides;
    private Grid grid;

    private GameObject player;

    private GameObject signal;
    private Vector3 signaledPosition;

    [Header("Gizmos")] [SerializeField] private int dist;
    
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
            Instance = GetComponent<ObjectPlacement>();
        }
        objects = new Dictionary<Vector2, GameObject>();
        overrides = new Dictionary<Vector2, PickupOverride>();
        player = GameObject.FindGameObjectWithTag("Player");
        grid = GetComponent<Grid>();
        signal = transform.GetChild(0).gameObject;
        if (!signal)
        {
            Debug.LogError("Object grid must have signal object childed!");
        }
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

    //Call this to attempt to pickup an object at a position
    public GameObject PickUp(Vector3 position)
    {
        Vector2 location = gridPosition(position);
        GameObject toReturn;
        if (objects.TryGetValue(location, out toReturn))
        {
            print("Pickup found an object!");
            objects.Remove(location);
            PickupOverride over;
            if (overrides.TryGetValue(location, out over))
            {
                over.GiveObject();
            }
        }
        return toReturn;
    }

    //Call this to attempt to drop your object at position
    //Returns false if the drop failed
    public bool Drop(GameObject obj, Vector3 position)
    {
        if (CanPlace(position))
        {
            //Place code
            Place(obj, position);
            Assert.IsTrue(OnFloor(position) == obj);
            return true;
        }
        else
        {
            print("Performing Checks");
            //Check for collision
            GameObject other;
            Vector2 loc = gridPosition(position);
            if (objects.TryGetValue(loc, out other))
            {
                //Test first combination
                Ingredient ing = other.GetComponent<Ingredient>();
                GameObject final;
                if (ing && (final = ing.combinesWith(obj.tag)))
                {
                    final = Instantiate(final);
                    final.transform.position = other.transform.position;
                    Replace(position, final);
                    Destroy(obj);
                    Destroy(other);
                    return true;
                }

                //Test other combination
                ing = obj.GetComponent<Ingredient>();
                if (ing && (final = ing.combinesWith(other.tag)))
                {
                    final = Instantiate(final);
                    final.transform.position = other.transform.position;
                    Replace(position, final);
                    Destroy(obj);
                    Destroy(other);
                    return true;
                }
            }
        }
        return false;
    }

    private bool CanPlace(Vector3 position)
    {
        return (!OnFloor(position));
    }

    
    private void Place(GameObject obj, Vector3 position)
    {
        Vector3 location = gridPosition(position);
        objects.Add(location, obj);
        PickupOverride over;
        if (overrides.TryGetValue(location, out over))
        {
            over.PassObject(obj);
            obj.transform.position = over.getLocation();
        }
        else
        {
            obj.transform.position = GetCellLocation(location);
        }
    }
    
    //Return real world location of grid coord
    public Vector3 GetCellLocation(Vector2 location)
    {
        Vector3 vec = grid.GetCellCenterWorld(new Vector3Int((int) location.x, 0, (int) location.y));
        return new Vector3(vec.x, dropHeight, vec.z);
    }

    //CURRENTLY DEFUNCT, BUT MIGHT BE USEFUL LATER
    //Return the location of grid coord, factoring in overrides
    public Vector3 GetFinalLocation(Vector2 location)
    {
        PickupOverride over;
        if (overrides.TryGetValue(location, out over))
        {
            print("Found an override!");
            return over.getLocation();
        }
        else
        {
            return GetCellLocation(location);
        }
    }

    public void Replace(Vector3 position, GameObject obj)
    {
        Vector2 loc = gridPosition(position);
        GameObject outObj;
        if (objects.TryGetValue(loc, out outObj))
        {
            //We have an object, so replace it
            objects.Remove(loc);
            objects.Add(loc, obj);
        }
        else
        {
            Debug.LogError("Attempted to replace something at an empty spot!");
            //Add it? The caller seemed to want it here 
            objects.Add(loc, obj);
        }
    }

    public bool ClearPoint(Vector3 position)
    {
        Vector2 loc = gridPosition(position);
        return objects.Remove(loc);
    }

    public bool RegisterOverride(PickupOverride over)
    {
        if (grid)
        {
            Vector2 location = gridPosition(over.getLocation());
            overrides.Add(location, over);
            print("Override registered! " + location);
            GameObject objectHeld;
            if (objects.TryGetValue(location, out objectHeld))
            {
                objectHeld = PickUp(over.getLocation());
                Drop(objectHeld, over.getLocation());
            }
            return true;
        }

        return false;
    }

    public void SignalPosition(Vector3 position)
    {
        signaledPosition = position;
        Vector2 loc = gridPosition(position);
        Vector3 norm = GetCellLocation(loc);
        signal.transform.position = new Vector3(norm.x, indicatorHeight, norm.z);
    }

    private void OnDrawGizmos()
    {
        if (player)
        {
            Gizmos.color = Color.red;
            Vector2 spot = gridPosition(player.transform.position);
            Gizmos.DrawWireCube(GetCellLocation(spot), grid.cellSize);
            
            //Draw the other cube
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(signaledPosition, grid.cellSize);
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

        if (overrides != null)
        {
            foreach (KeyValuePair<Vector2, PickupOverride> kvp in overrides)
            {
                Gizmos.color = Color.magenta;
                Vector2 spot = kvp.Key;
                Gizmos.DrawWireCube(GetCellLocation(spot), grid.cellSize);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(GetCellLocation(spot), kvp.Value.getLocation());
            }
        }

        

        Gizmos.color = Color.green;
        Grid g = GetComponent<Grid>();
        for (float i = 0; i <= dist; i += g.cellSize.x)
        {
            Gizmos.DrawLine(new Vector3(i, 0, -dist), new Vector3(i, 0, dist));
            Gizmos.DrawLine(new Vector3(-i, 0, -dist), new Vector3(-i, 0, dist));
        }
        for (float j = 0; j <= dist; j += g.cellSize.z)
        {
            Gizmos.DrawLine(new Vector3(-dist, 0, j), new Vector3(dist, 0, j));
            Gizmos.DrawLine(new Vector3(-dist, 0, -j), new Vector3(dist, 0, -j));
        }
        
        
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector3 planeSize = new Vector3(.3f, .1f, .3f);
        Gizmos.DrawCube(new Vector3(0, dropHeight, 0), planeSize);

        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(0, indicatorHeight, 0), planeSize);
    }
}