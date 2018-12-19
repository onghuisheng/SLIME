using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour {

    //THANKS ADRIAN TT
    //ALSO: please change this if it has problems :,>

    [Header("Grid Sizing")]
    [SerializeField] int rowCount;              // Number of Rows
    [SerializeField] int colCount;              // Number of Columns

    [Header("Spawn Frequency (in Hz)")]
    [SerializeField] float rowFrequency;        // Frequency at which each grid item spawns per row
    [SerializeField] float colFrequency;        // Frequency at which each grid item spawns per column

    [Header("Spawn Direction")] 
    [SerializeField] bool spawnByRow;           // To spawn row by or or collumn by collumn 

    [Header("Prefab to populate grid")]
    [SerializeField] GameObject spawnPrefab;    // Prefab to spawn

    private List<GameObject> instances;         // Store all spawned prefabs

    private float outerLoop;                        // The max value of the outer forloop
    private float innerLoop;                        // The max value of the inner forloop
    [Header("Inner/Outer loop time")]
    [SerializeField] private float outerLoopTime;   //Time between each forloop in the outer loop (in sec)
    [SerializeField] private float innerLoopTime;   //Time between each forloop in the inner loop (in sec)

    private float prefabWidth;                      // Width of the prefab
    private float prefabLength;                     // Length of the prefab

    private void Awake()
    {
        //Get length of prefab before start runs
        GameObject temp = Instantiate(spawnPrefab, transform);
        prefabWidth = temp.GetComponent<MeshRenderer>().bounds.size.x;
        prefabLength = temp.GetComponent<MeshRenderer>().bounds.size.z;
        //Destroy temp
        Destroy(temp);

        //if spawnning row by row
        if (spawnByRow)
        {
            outerLoop = rowCount;
            innerLoop = colCount;

            //If frequency is 0, it treats it like there's no time between the spawn
            if (rowFrequency > 0)
                outerLoopTime = 1f / rowFrequency;
            else
                outerLoopTime = 0f;

            if (colFrequency > 0)
                innerLoopTime = 1f / colFrequency;
            else
                innerLoopTime = 0f;
        }
        //else, spawn by collumn
        else
        {
            outerLoop = colCount;
            innerLoop = rowCount;

            if (colFrequency > 0)
                outerLoopTime = 1f / colFrequency;
            else
                outerLoopTime = 0f;

            if (rowFrequency > 0)
                innerLoopTime = 1f / rowFrequency;
            else
                innerLoopTime = 0f;
        }
    }

    // Use this for initialization
    private void Start () {
        InitializeGrid();
	}

    private IEnumerator SpawnGrid()
    {
        instances = new List<GameObject>();

        //rxr: outerloop is rowCount, innerloop is colCount
        //cxc: outerloop is colCount, innerloop is rowCount

        Vector3 spawnPosition = transform.position;
        for (int y = 0; y < outerLoop; y++)
        {
            if (spawnByRow)                                 //reset spawnPosition.x at every new row if rxr
                spawnPosition.x = transform.position.x;
            else                                            //else, reset z pos
                spawnPosition.z = transform.position.z;

            for (int x = 0; x < innerLoop; x++)
            {
                GameObject spawn = Instantiate(spawnPrefab, transform);
                spawn.transform.position = spawnPosition;
                
                if (spawnByRow)                             //move spawnPosition.x over by width of prefab
                    spawnPosition.x += prefabWidth;
                else                                        //else move z by length
                    spawnPosition.z += prefabLength;

                //wait for innerloop
                yield return new WaitForSeconds(innerLoopTime);
            }

            if (spawnByRow)                                //move spawnPosition to next column, if rxr
                spawnPosition.z += prefabLength;
            else                                           //else move to next row
                spawnPosition.x += prefabWidth;

            //wait for outerloop
            yield return new WaitForSeconds(outerLoopTime);
        }
    }

    public void InitializeGrid()
    {
        // Clear all spawned prefabs
        ClearGrid();

        // Start the coroutine to spawn the grid
        StartCoroutine("SpawnGrid");
    }

    public void ClearGrid()
    {
        //if list does not exist/no prefabs spawnned, return 
        if (instances == null || instances.Count == 0)
            return;

        //Start from the back of the list
        for (int i = instances.Count - 1; i >= 0; i--)
        {
            Destroy(instances[i]);
        }
        instances.Clear();
    }


    // Update is called once per frame
    void Update () {
		
	}
}
