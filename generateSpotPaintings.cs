using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class generateSpotPaintings : MonoBehaviour
{
    public GameObject blockGameObject;
    public Camera mainCamera;

    private int worldSizeX = 5;
    private int worldSizeZ = 5;
    private int gridOffset = 2;

    Color[] colors = new Color[]
    {
        new Color(7f/255f, 14f/255f, 59f/255f),
        new Color(70f/255f, 125f/255f, 186f/255f),
        new Color(155f/255f, 178f/255f, 82f/255f),
        new Color(210f/255f, 161f/255f, 152f/255f),
        new Color(229f/255f, 193f/255f, 65f/255f),
        new Color(96f/255f, 58f/255f, 126f/255f),
        new Color(106f/255f, 130f/255f, 187f/255f),
        new Color(81f/255f, 17f/255f, 42f/255f),
        new Color(173f/255f, 45f/255f, 47f/255f),
        new Color(69f/255f, 120f/255f, 88f/255f),
        new Color(50f/255f, 115f/255f, 177f/255f),
        new Color(221f/255f, 100f/255f, 61f/255f),
        new Color(147f/255f, 178f/255f, 54f/255f),
        new Color(26f/255f, 62f/255f, 141f/255f),
        new Color(189f/255f, 86f/255f, 50f/255f),
        new Color(147f/255f, 31f/255f, 39f/255f),
        new Color(233f/255f, 166f/255f, 58f/255f),
        new Color(58f/255f, 126f/255f, 191f/255f),
        new Color(44f/255f, 101f/255f, 59f/255f),
        new Color(237f/255f, 187f/255f, 189f/255f),
        new Color(110f/255f, 114f/255f, 52f/255f),
        new Color(81f/255f, 54f/255f, 189f/255f),
        new Color(43f/255f, 100f/255f, 57f/255f),
        new Color(37f/255f, 53f/255f, 104f/255f)
    };

    void Start()
    {
        worldSizeX = Random.Range(2,10);
        worldSizeZ = Random.Range(2,10);
        Color lastColor = Color.white;

        for (int x = 0; x < worldSizeX; x++)
        {
            for(int z = 0; z < worldSizeZ; z++)
            {
                Vector3 pos = new Vector3(x * gridOffset, 0, z * gridOffset);
                GameObject block = Instantiate(blockGameObject, pos, Quaternion.identity);

                MeshRenderer renderer = block.GetComponent<MeshRenderer>();

                if (renderer != null)
                {
                    Color randomColor = GetUniqueColor(lastColor); // Get a unique color
                    renderer.material.color = randomColor;
                    lastColor = randomColor; // Update lastColor to the current color
                }
            }
        }
        // Center the camera according to the size of the grid
        mainCamera.transform.position = new Vector3((worldSizeX - 1) * gridOffset / 2f, 25f, (worldSizeZ - 1) * gridOffset / 2f);
    }

    Color GetUniqueColor(Color lastColor)
    {
        Color randomColor;
        do
        {
            randomColor = colors[Random.Range(0, colors.Length)];
        }
        while (randomColor == lastColor); // Keep generating random colors until it's different from the last one
        return randomColor;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
