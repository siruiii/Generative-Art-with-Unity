using UnityEngine;

public class Generation : MonoBehaviour
{
    public int width = 150;
    public int height = 80;
    public GameObject cellPrefab;

    private int[,] board;
    private int[,] next;
    private GameObject[,] cellObjects;

    private float updateInterval = 0.1f; // controls the speed of generations
    private float timer = 0f;

    public float interactionInterval = 0.1f;
    private float interactionTimer = 0f;


    void Start()
    {
        board = new int[width, height];
        next = new int[width, height];
        cellObjects = new GameObject[width, height];
        Init();
    }

    void Init()
    {
        for(int i =0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                if (i == 0 || j == 0 || i == width - 1 || j == height - 1)
                {
                    board[i, j] = 0;
                }
                else
                {
                    board[i, j] = Random.Range(0, 2);
                    next[i, j] = 0;
                }
                if (cellObjects[i, j] != null)
                {
                    Destroy(cellObjects[i, j]);
                }
                cellObjects[i, j] = Instantiate(cellPrefab, new Vector2(i, j), Quaternion.identity);
            }
        }
    }

    void Generate()
    {
        for(int x = 1; x < width - 1; x++)
        {
            for(int y = 1; y < height - 1; y++)
            {
                int neighbors = 0;
                for(int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        neighbors += board[x + i, y + j];
                    }
                }
                neighbors -= board[x, y];
                if (board[x,y]==1 && neighbors < 2)
                {
                    next[x, y] = 0;
                }else if(board[x, y] == 1 && neighbors > 3)
                {
                    next[x, y] = 0;
                }else if (board[x, y] == 0 && neighbors == 3)
                {
                    next[x, y] = 1;
                }
                else
                {
                    next[x, y] = board[x, y];
                }
            }
        }
        // show each generation
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                board[x, y] = next[x, y];
            }
        }
    }

    void DrawBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject cell = cellObjects[x, y];
                Renderer renderer = cell.GetComponent<Renderer>();
                if (board[x, y] == 1)
                {
                    renderer.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                }
                else
                {
                    renderer.material.color = Color.white;
                }
            }
        }
    }

    void Update()
    {

            timer += Time.deltaTime;
            if (timer >= updateInterval)
            {
                Generate();
                DrawBoard();
                timer = 0f;
            }
        

        interactionTimer += Time.deltaTime;
        if (interactionTimer >= interactionInterval)
        {
            mouseCell();
            interactionTimer = 0f;
        }
    }

    void mouseCell()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            GameObject obj = hit.collider.gameObject;
            Renderer renderer = obj.GetComponent<Renderer>();

            if (renderer != null)
            {
                int x = Mathf.RoundToInt(obj.transform.position.x);
                int y = Mathf.RoundToInt(obj.transform.position.y);

                toggleCell(renderer,x, y);
            }
        }
    }

    void toggleCell(Renderer renderer, int x, int y)
    {
        board[x, y] = 1;
        renderer.material.color = Color.black;
    }
}
