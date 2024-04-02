using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generateGrid : MonoBehaviour
{
    public GameObject blockGameObject;
    private Color minHeightColor = Color.white;
    private Color maxHeightColor = Color.black;
    private int worldSizeX = 20;
    private int worldSizeZ = 20;
    private int noiseHeight = 10;
    private float gridOffset = 0.8f;
    private float waveSpeed = 1.0f;
    private float waveHeight = 0.5f;

    private List<GameObject> blocks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        GenerateInitialGrid();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateWaveMovement();

        // Check for mouse click
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            GenerateRandomColors();
            ApplyColorsToBlocks();
        }
    }

    private void GenerateInitialGrid()
    {
        GenerateRandomColors();
        ApplyColorsToBlocks();

        for (int x = 0; x < worldSizeX; x++)
        {
            for (int z = 0; z < worldSizeZ; z++)
            {
                float noise = generateNoise(x, z, 8f) * noiseHeight;
                Vector3 pos = new Vector3(x * gridOffset, noise, z * gridOffset);
                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity) as GameObject;
                block.transform.SetParent(this.transform);
                blocks.Add(block);
            }
        }
    }

    private void UpdateWaveMovement()
    {
        float time = Time.time * waveSpeed;
        for (int i = 0; i < blocks.Count; i++)
        {
            Vector3 pos = blocks[i].transform.position;
            float noise = generateNoise((int)pos.x, (int)pos.z, 8f) * noiseHeight;
            pos.y = noise + Mathf.Sin(time + pos.x + pos.z) * waveHeight;
            blocks[i].transform.position = pos;
        }
    }

    private float generateNoise(int x, int z, float detailScale)
    {
        float xNoise = (x + this.transform.position.x) / detailScale;
        float zNoise = (z + this.transform.position.y) / detailScale;
        return Mathf.PerlinNoise(xNoise, zNoise);
    }

    private void GenerateRandomColors()
    {
        // Generate random colors with significant differences in hue, saturation, and value
        minHeightColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
        maxHeightColor = Random.ColorHSV(0f, 1f, 0.5f, 1f, 0.5f, 1f);
    }

    private void ApplyColorsToBlocks()
    {
        for (int i = 0; i < blocks.Count; i++)
        {
            float noise = generateNoise((int)blocks[i].transform.position.x, (int)blocks[i].transform.position.z, 8f) * noiseHeight;
            Color color = Color.Lerp(minHeightColor, maxHeightColor, noise / noiseHeight);
            Renderer renderer = blocks[i].GetComponent<Renderer>();
            renderer.material.color = color;
        }
    }
}
