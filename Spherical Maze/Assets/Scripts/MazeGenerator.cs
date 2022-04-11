using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //Serialized Fields
        //Maze
    [SerializeField]
    private int m_mazeWidth = 0;
    [SerializeField]
    private int m_mazeHeight = 0;
        //Prefabs
    [SerializeField]
    GameObject m_tilePrefab;
    [SerializeField]
    GameObject m_wallPrefab;

    //Private Variables
        //Maze
    Cell[,] m_maze;
    int m_visitedCount = 0;
    Stack<Vector2> m_path = new Stack<Vector2>();
        //Maze Visuals
    public CellVisuals[,] m_mazeVisuals;

    // Start is called before the first frame update
    void Start()
    {
        m_maze = new Cell[m_mazeWidth, m_mazeHeight];
        m_mazeVisuals = new CellVisuals[m_mazeWidth, m_mazeHeight];
        for (int x = 0; x < m_mazeWidth; x++)
        {
            for (int y = 0; y < m_mazeHeight; y++)
            {
                m_maze[x, y] = new Cell();
                m_mazeVisuals[x, y] = new CellVisuals();
            }
        }
        AjustCamera(0, m_mazeWidth, 0, m_mazeHeight);

        m_path.Push(new Vector2(0, 0));
        m_maze[0, 0].m_visited = true;
        m_visitedCount++;

        CalculateMaze();
        for (int x = 0; x < m_mazeWidth; x++)
        {
            for (int y = 0; y < m_mazeHeight; y++)
            {
                UpdateVisuals(x, y);
            }
        }
        m_mazeVisuals[0, 0].m_tile.GetComponent<Renderer>().material.color = Color.green;
        m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.GetComponent<Renderer>().material.color = Color.red;
    }

    //private void Update()
    //{
    //    CalculateMaze();
    //    UpdateVisuals((int)m_path.Peek().x, (int)m_path.Peek().y);
    //    UpdateVisuals((int)m_path.Peek().x + 0, (int)m_path.Peek().y + 1);
    //    UpdateVisuals((int)m_path.Peek().x + 0, (int)m_path.Peek().y + -1);
    //    UpdateVisuals((int)m_path.Peek().x + 1, (int)m_path.Peek().y + 0);
    //    UpdateVisuals((int)m_path.Peek().x + -1, (int)m_path.Peek().y + 0);

    //    m_mazeVisuals[0, 0].m_tile.GetComponent<Renderer>().material.color = Color.green;
    //    m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.GetComponent<Renderer>().material.color = Color.red;
    //}

    void CalculateMaze()
    { 
        //if (m_visitedCount<m_mazeWidth* m_mazeHeight)
        while (m_visitedCount < m_mazeWidth * m_mazeHeight)
        {
            List<char> neighbours = new List<char>();

            //Check Northern Neighbour
            if (m_path.Peek().y < m_mazeHeight - 1)
            {
                if (!m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + 1].m_visited)
                {
                    neighbours.Add('N');
                }
            }
            //Check Eastern Neighbour
            if (m_path.Peek().x < m_mazeWidth - 1)
            {
                if(!m_maze[(int)m_path.Peek().x + 1, (int)m_path.Peek().y + 0].m_visited)
                {
                    neighbours.Add('E');
                }
            }
            //Check Southern Neighbour
            if (m_path.Peek().y > 0)
            {
                if (!m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + -1].m_visited)
                {
                    neighbours.Add('S');
                }
            }
            //Check Western Neighbour
            if (m_path.Peek().x > 0)
            {
                if (!m_maze[(int)m_path.Peek().x + -1, (int)m_path.Peek().y + 0].m_visited)
                {
                    neighbours.Add('W');
                }
            }

            //No non-visited Neightbours
            if (neighbours.Count != 0)
            {
                char dir = neighbours[Random.Range(0, neighbours.Count)];
                switch (dir)
                {
                    case 'N':
                        m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_northPath = true;
                        m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + 1].m_southPath = true;
                        m_path.Push(new Vector2((int)m_path.Peek().x + 0, (int)m_path.Peek().y + 1));
                        break;
                    case 'E':
                        m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_eastPath = true;
                        m_maze[(int)m_path.Peek().x + 1, (int)m_path.Peek().y + 0].m_westPath = true;
                        m_path.Push(new Vector2((int)m_path.Peek().x + 1, (int)m_path.Peek().y + 0));
                        break;
                    case 'S':
                        m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_southPath = true;
                        m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + -1].m_northPath = true;
                        m_path.Push(new Vector2((int)m_path.Peek().x + 0, (int)m_path.Peek().y + -1));
                        break;
                    case 'W':
                        m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_westPath = true;
                        m_maze[(int)m_path.Peek().x + -1, (int)m_path.Peek().y + 0].m_eastPath = true;
                        m_path.Push(new Vector2((int)m_path.Peek().x + -1, (int)m_path.Peek().y + 0));
                        break;
                    default:
                        Debug.LogError("Unknown Direction");
                        break;
                }

                m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_visited = true;
                m_visitedCount++;
            }
            else
            {
                m_path.Pop();
            }
        }
    }

    void UpdateVisuals(int a_x, int a_y)
    {
        if (a_x >= 0 && a_x < m_mazeWidth && a_y >= 0 && a_y < m_mazeHeight)
        {
            Destroy(m_mazeVisuals[a_x, a_y].m_tile);
            m_mazeVisuals[a_x, a_y].m_tile = null;

            for (int i = 0; i < m_mazeVisuals[a_x, a_y].m_walls.Length; i++)
            {
                Destroy(m_mazeVisuals[a_x, a_y].m_walls[i]);
                m_mazeVisuals[a_x, a_y].m_walls[i] = null;
            }

            m_mazeVisuals[a_x, a_y].m_tile = Instantiate(m_tilePrefab, new Vector3(a_x, 0, a_y), Quaternion.identity);
            m_mazeVisuals[a_x, a_y].m_tile.transform.parent = this.gameObject.transform;
            if (m_maze[a_x, a_y].m_visited)
            {
                m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.color = Color.white;
            }
            else
            {
                m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.color = Color.blue;
            }
            if (!m_maze[a_x, a_y].m_northPath || a_y == m_mazeHeight)
            {
                m_mazeVisuals[a_x, a_y].m_walls[0] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[0].transform.Rotate(new Vector3(0, 0, 0));
                m_mazeVisuals[a_x, a_y].m_walls[0].GetComponentInChildren<Renderer>().material.color = Color.black;
                m_mazeVisuals[a_x, a_y].m_walls[0].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;

            }
            if (!m_maze[a_x, a_y].m_eastPath || a_x == m_mazeWidth)
            {
                m_mazeVisuals[a_x, a_y].m_walls[1] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[1].transform.Rotate(new Vector3(0, 90, 0));
                m_mazeVisuals[a_x, a_y].m_walls[1].GetComponentInChildren<Renderer>().material.color = Color.black;
                m_mazeVisuals[a_x, a_y].m_walls[1].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
            if (!m_maze[a_x, a_y].m_southPath || a_y == 0)
            {
                m_mazeVisuals[a_x, a_y].m_walls[2] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[2].transform.Rotate(new Vector3(0, 180, 0));
                m_mazeVisuals[a_x, a_y].m_walls[2].GetComponentInChildren<Renderer>().material.color = Color.black;
                m_mazeVisuals[a_x, a_y].m_walls[2].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
            if (!m_maze[a_x, a_y].m_westPath || a_x == 0)
            {
                m_mazeVisuals[a_x, a_y].m_walls[3] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[3].transform.Rotate(new Vector3(0, 270, 0));
                m_mazeVisuals[a_x, a_y].m_walls[3].GetComponentInChildren<Renderer>().material.color = Color.black;
                m_mazeVisuals[a_x, a_y].m_walls[3].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
        }
    }

    void AjustCamera(int a_lowestX, int a_highestX, int a_lowestY, int a_highestY)
    {
        int ensureBorder = 2;

        Camera.main.transform.position = new Vector3(m_mazeWidth / 2, 5, m_mazeHeight / 2);

        float lowestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_lowestX - ensureBorder, 0, 0)).x;
        float highestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_highestX + ensureBorder, 0, 0)).x;
        float lowestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_lowestY - ensureBorder)).y;
        float highestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_highestY + ensureBorder)).y;

        // While the highest & lowest x and y viewport point is not between 0 and 1, increase orthographicSize
        while (highestXViewportPoint > 1 || lowestXViewportPoint < 0 || highestYViewportPoint > 1 || lowestYViewportPoint < 0)
        {
            Camera.main.orthographicSize += 1;
            lowestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_lowestX - ensureBorder, 0, 0)).x;
            highestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_highestX + ensureBorder, 0, 0)).x;
            lowestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_lowestY - ensureBorder)).y;
            highestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_highestY + ensureBorder)).y;
        }
    }
}

public class Cell : MonoBehaviour
{
    //Public Variables
    public bool m_visited = false;
    public bool m_northPath = false;
    public bool m_eastPath = false;
    public bool m_southPath = false;
    public bool m_westPath = false;
}
public class CellVisuals
{
    //Public Variables
    public GameObject m_tile = null;
    public GameObject[] m_walls = new GameObject[4];
}