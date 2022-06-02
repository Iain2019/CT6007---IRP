using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    //Serialized Fields
        //Maze
    [SerializeField]
    int m_mazeWidth = 0;
    [SerializeField]
    int m_mazeHeight = 0;
        //Prefabs
    [SerializeField]
    GameObject m_tilePrefab;
    [SerializeField]
    GameObject m_wallPrefab;
    [SerializeField]
    GameObject m_playerPrefab;
    [SerializeField]
    bool m_3D = true;
    [SerializeField]
    GameObject m_infoCanvas;

    //Private Variables
        //Maze
    private Cell[,] m_maze;
    private int m_visitedCount = 0;
    private Stack<Vector2> m_path = new Stack<Vector2>();
        //Maze Visuals
    private CellVisuals[,] m_mazeVisuals;

    // Start is called before the first frame update
    void Awake()
    {
        //check pi
        if (GameObject.Find("PersistentInfo") != null)
        {
            //set maze size
            m_mazeWidth = PersistentInfo.Instance.m_MazeWidth;
            m_mazeHeight = PersistentInfo.Instance.m_MazeHeight;
        }
        //set logic and visuals arrays
        m_maze = new Cell[m_mazeWidth, m_mazeHeight];
        m_mazeVisuals = new CellVisuals[m_mazeWidth, m_mazeHeight];
        for (int x = 0; x < m_mazeWidth; x++)
        {
            for (int y = 0; y < m_mazeHeight; y++)
            {
                //loop through cells and set as defaults
                m_maze[x, y] = new Cell();
                m_mazeVisuals[x, y] = new CellVisuals();
            }
        }
        ////Ajust camera for debugging 
        //AjustCamera(0, m_mazeWidth, 0, m_mazeHeight);

        //push start point to stack
        m_path.Push(new Vector2(0, 0));
        //set first as visited
        m_maze[0, 0].m_visited = true;
        //up visted count
        m_visitedCount++;

        //calc maze
        CalculateMaze();
        //move maze to account for size (0, 0) centre
        this.transform.position = new Vector3(-((float)m_mazeWidth / 2), transform.position.y, -((float)m_mazeHeight / 2));
        for (int x = 0; x < m_mazeWidth; x++)
        {
            for (int y = 0; y < m_mazeHeight; y++)
            {
                //update the visuals for the maze
                UpdateVisuals(x, y);
            }
        }
        //set specific tiles shader info
        m_mazeVisuals[0, 0].m_tile.GetComponent<Renderer>().material.SetColor("_BaseColour", Color.green);
        GameObject player = Instantiate(m_playerPrefab, m_mazeVisuals[0, 0].m_tile.transform.position + new Vector3(0, 1, 0), Quaternion.identity);
        m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.GetComponent<Renderer>().material.SetColor("_BaseColour", Color.red);
        //show end point UI
        m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.transform.GetChild(0).gameObject.SetActive(true);
        if (m_3D)
        {
            //set player in end point
            m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.transform.GetChild(0).gameObject.GetComponent<EndPoint3D>().m_player = player;
        }
        else
        {
            //set player & info canvas for endpoint
            m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.transform.GetChild(0).gameObject.GetComponent<EndPoint2D>().m_player = player;
            m_mazeVisuals[m_mazeWidth - 1, m_mazeHeight - 1].m_tile.transform.GetChild(0).gameObject.GetComponent<EndPoint2D>().m_InfoCanvas = m_infoCanvas;
        }
    }

    //private void Update()
    //{
    //    //calculating maze in update for debugging to show sep by step building of maze  
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
        ////for updated veiwing
        //if (m_visitedCount<m_mazeWidth* m_mazeHeight)
        //if count is within array size
        while (m_visitedCount < m_mazeWidth * m_mazeHeight)
        {
            //neightbours list for cells arround current one
            List<char> neighbours = new List<char>();

            //Check Northern Neighbour
            //peek neightbours in path to check neighbours in array
            if (m_path.Peek().y < m_mazeHeight - 1)
            {
                if (!m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + 1].m_visited)
                {
                    //add unvisited neighbour to list
                    neighbours.Add('N');
                }
            }
            //Check Eastern Neighbour
            if (m_path.Peek().x < m_mazeWidth - 1)
            {
                if(!m_maze[(int)m_path.Peek().x + 1, (int)m_path.Peek().y + 0].m_visited)
                {
                    //add unvisited neighbour to list
                    neighbours.Add('E');
                }
            }
            //Check Southern Neighbour
            if (m_path.Peek().y > 0)
            {
                if (!m_maze[(int)m_path.Peek().x + 0, (int)m_path.Peek().y + -1].m_visited)
                {
                    //add unvisited neighbour to list
                    neighbours.Add('S');
                }
            }
            //Check Western Neighbour
            if (m_path.Peek().x > 0)
            {
                if (!m_maze[(int)m_path.Peek().x + -1, (int)m_path.Peek().y + 0].m_visited)
                {
                    //add unvisited neighbour to list
                    neighbours.Add('W');
                }
            }

            //No non-visited Neightbours
            if (neighbours.Count != 0)
            {
                //get rnd neighbour
                char dir = neighbours[Random.Range(0, neighbours.Count)];
                switch (dir)
                {
                    //for rnd direction
                    //set value of this and neightbour to not show wall
                    //push new position (visited neighbour) to stack
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
                //set new position to true
                m_maze[(int)m_path.Peek().x, (int)m_path.Peek().y].m_visited = true;
                //up visited count
                m_visitedCount++;
            }
            else
            {
                //no more neightbours, pop the stack until has new neighbour 
                m_path.Pop();
            }
        }
    }

    void UpdateVisuals(int a_x, int a_y)
    {
        //with maze
        if (a_x >= 0 && a_x < m_mazeWidth && a_y >= 0 && a_y < m_mazeHeight)
        {
            //destroy existing tile
            Destroy(m_mazeVisuals[a_x, a_y].m_tile);
            //set array value to null
            m_mazeVisuals[a_x, a_y].m_tile = null;

            //for each wall
            for (int i = 0; i < m_mazeVisuals[a_x, a_y].m_walls.Length; i++)
            {
                //destroy existing tile
                Destroy(m_mazeVisuals[a_x, a_y].m_walls[i]);
                //set array value to null
                m_mazeVisuals[a_x, a_y].m_walls[i] = null;
            }

            //instantiate new gameobject
            m_mazeVisuals[a_x, a_y].m_tile = Instantiate(m_tilePrefab, new Vector3(a_x, 0, a_y), Quaternion.identity);
            //assign sahder values
            m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.SetFloat("_Width", m_mazeWidth);
            m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.SetFloat("_Height", m_mazeHeight);
            //set parent
            m_mazeVisuals[a_x, a_y].m_tile.transform.parent = this.gameObject.transform;
            //set colour (mainly for update/ debugging)
            if (m_maze[a_x, a_y].m_visited)
            {
                m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.SetColor("_BaseColour", Color.white);
            }
            else
            {
                m_mazeVisuals[a_x, a_y].m_tile.GetComponent<Renderer>().material.SetColor("_BaseColour", Color.blue);
            }

            //check what neightbours the tile has
            //add walls to tiles that do no have a path going there
            //also check if not on maze boarder
            //assign values for wall's shaders
            if (!m_maze[a_x, a_y].m_northPath || a_y == m_mazeHeight)
            {
                m_mazeVisuals[a_x, a_y].m_walls[0] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[0].transform.Rotate(new Vector3(0, 0, 0));
                m_mazeVisuals[a_x, a_y].m_walls[0].GetComponentInChildren<Renderer>().material.SetFloat("_Width", m_mazeWidth);
                m_mazeVisuals[a_x, a_y].m_walls[0].GetComponentInChildren<Renderer>().material.SetFloat("_Height", m_mazeHeight);
                m_mazeVisuals[a_x, a_y].m_walls[0].GetComponentInChildren<Renderer>().material.SetColor("_BaseColour", Color.grey);
                m_mazeVisuals[a_x, a_y].m_walls[0].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;

            }
            if (!m_maze[a_x, a_y].m_eastPath || a_x == m_mazeWidth)
            {
                m_mazeVisuals[a_x, a_y].m_walls[1] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[1].transform.Rotate(new Vector3(0, 90, 0));
                m_mazeVisuals[a_x, a_y].m_walls[1].GetComponentInChildren<Renderer>().material.SetFloat("_Width", m_mazeWidth);
                m_mazeVisuals[a_x, a_y].m_walls[1].GetComponentInChildren<Renderer>().material.SetFloat("_Height", m_mazeHeight);
                m_mazeVisuals[a_x, a_y].m_walls[1].GetComponentInChildren<Renderer>().material.SetColor("_BaseColour", Color.grey);
                m_mazeVisuals[a_x, a_y].m_walls[1].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
            if (!m_maze[a_x, a_y].m_southPath || a_y == 0)
            {
                m_mazeVisuals[a_x, a_y].m_walls[2] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[2].transform.Rotate(new Vector3(0, 180, 0));
                m_mazeVisuals[a_x, a_y].m_walls[2].GetComponentInChildren<Renderer>().material.SetFloat("_Width", m_mazeWidth);
                m_mazeVisuals[a_x, a_y].m_walls[2].GetComponentInChildren<Renderer>().material.SetFloat("_Height", m_mazeHeight);
                m_mazeVisuals[a_x, a_y].m_walls[2].GetComponentInChildren<Renderer>().material.SetColor("_BaseColour", Color.grey);
                m_mazeVisuals[a_x, a_y].m_walls[2].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
            if (!m_maze[a_x, a_y].m_westPath || a_x == 0)
            {
                m_mazeVisuals[a_x, a_y].m_walls[3] = Instantiate(m_wallPrefab, new Vector3(a_x, 0.5f, a_y), Quaternion.identity);
                m_mazeVisuals[a_x, a_y].m_walls[3].transform.Rotate(new Vector3(0, 270, 0));
                m_mazeVisuals[a_x, a_y].m_walls[3].GetComponentInChildren<Renderer>().material.SetFloat("_Width", m_mazeWidth);
                m_mazeVisuals[a_x, a_y].m_walls[3].GetComponentInChildren<Renderer>().material.SetFloat("_Height", m_mazeHeight);
                m_mazeVisuals[a_x, a_y].m_walls[3].GetComponentInChildren<Renderer>().material.SetColor("_BaseColour", Color.grey);
                m_mazeVisuals[a_x, a_y].m_walls[3].transform.parent = m_mazeVisuals[a_x, a_y].m_tile.transform;
            }
        }
    }

    void AjustCamera(int a_lowestX, int a_highestX, int a_lowestY, int a_highestY)
    {
        //adds a bit of border
        int ensureBorder = 2;
        //centre camera
        Camera.main.transform.position = new Vector3(m_mazeWidth / 2, 5, m_mazeHeight / 2);

        //set points the the camera needs to encapulate
        float lowestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_lowestX - ensureBorder, 0, 0)).x;
        float highestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_highestX + ensureBorder, 0, 0)).x;
        float lowestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_lowestY - ensureBorder)).y;
        float highestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_highestY + ensureBorder)).y;

        // While the highest & lowest x and y viewport point is not between 0 and 1, increase orthographicSize
        while (highestXViewportPoint > 1 || lowestXViewportPoint < 0 || highestYViewportPoint > 1 || lowestYViewportPoint < 0)
        {
            //up orthographic viewing untill all points are in camera view
            Camera.main.orthographicSize += 1;
            lowestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_lowestX - ensureBorder, 0, 0)).x;
            highestXViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(a_highestX + ensureBorder, 0, 0)).x;
            lowestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_lowestY - ensureBorder)).y;
            highestYViewportPoint = Camera.main.WorldToViewportPoint(new Vector3(0, 0, a_highestY + ensureBorder)).y;
        }
    }
}

//info for each cell
public class Cell
{
    //Public Variables
    public bool m_visited = false;
    public bool m_northPath = false;
    public bool m_eastPath = false;
    public bool m_southPath = false;
    public bool m_westPath = false;
}
//info for each cell's visuals
public class CellVisuals
{
    //Public Variables
    public GameObject m_tile = null;
    public GameObject[] m_walls = new GameObject[4];
}