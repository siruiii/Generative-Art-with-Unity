using UnityEngine;
using System.Collections.Generic;

public class MouseAttractor : MonoBehaviour
{
    public AudioVisualization _audioVis;

    public GameObject prefabToInstantiate; 
    public float attractionSpeed = 5f; 
    public float minSize = 0.5f; 
    public float maxSize = 1f; 
    public float returnSpeed = 2f; 
    private bool isAttracting = false; 

    public List<Vector3> originalPositions = new List<Vector3>(); 
    Vector3 scaleMin = new Vector3(0.1f, 0.1f, 0.1f);
    Vector3 scaleMax = new Vector3(0.6f, 0.6f, 0.6f);
    public AudioSource audioSource;

    void Start()
    {
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Attract");
        foreach (GameObject cube in cubes)
        {
            originalPositions.Add(cube.transform.position);
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            InstantiatePrefab();
        }

        if (Input.GetMouseButton(0)) 
        {
            isAttracting = true;
        }
        else
        {
            isAttracting = false;
            ReturnCubesToOriginalPositions();
        }

        if (isAttracting)
        {
            AttractCubesToMouse();
        }

        ScaleCubes();
    }

    void InstantiatePrefab()
    {
        
        Vector3 mousePosition = Input.mousePosition;

        
        mousePosition.z = 25f; 

        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);

        
        GameObject newObj = Instantiate(prefabToInstantiate, worldPosition, Quaternion.identity);
        newObj.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.white);
        newObj.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.white);
        newObj.name = "user";

       
        originalPositions.Add(newObj.transform.position);
    }

    void AttractCubesToMouse()
    {
        Vector3 mousePosition = Input.mousePosition;
        //mousePosition.z = Camera.main.transform.position.z;
        //mousePosition.x = 25f;
        //mousePosition.y = 25f;
        mousePosition.z = 25f;

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //Vector3 targetPosition = new Vector3(25f,25f,25f);

        List<GameObject> cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Attract"));
        foreach (GameObject cube in cubes)
        {

            Vector3 direction = (targetPosition - cube.transform.position).normalized; 
            cube.transform.position += direction * attractionSpeed * Time.deltaTime; 
        }
    }

    void ReturnCubesToOriginalPositions()
    {
        
        List<GameObject> cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Attract"));
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.position = Vector3.Lerp(cubes[i].transform.position, originalPositions[i], returnSpeed * Time.deltaTime);
        }
    }
    void ScaleCubes()
    {
        // current time stamp
        float currentTime = Time.time;
        float audioLength = audioSource.clip.length;
        float remainingTime = audioLength - currentTime;

        List<GameObject> cubes = new List<GameObject>(GameObject.FindGameObjectsWithTag("Attract"));
        for (int i = 0; i < cubes.Count; i++)
        {
            if (cubes[i].name == "user")
            {
                cubes[i].transform.localScale = Vector3.Lerp(scaleMin, scaleMax, _audioVis._audioBand[i % 8]);
            }

            if (remainingTime < 6)
            {
                cubes[i].transform.localScale = new Vector3(0f, 0f, 0f);
            }
        }

    }
}
