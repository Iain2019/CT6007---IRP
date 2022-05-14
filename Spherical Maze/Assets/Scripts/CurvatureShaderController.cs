using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvatureShaderController : MonoBehaviour
{
    [SerializeField, Range(-0.1f, 0.1f)]
    float m_curvature = 0.0f;
    [SerializeField]
    Color m_tileColour = Color.white;
    [SerializeField]
    Color m_wallColour = Color.black;

    GameObject[] m_tiles;
    GameObject[] m_walls;

    // Start is called before the first frame update
    void Start()
    {
        m_tiles = GameObject.FindGameObjectsWithTag("Tile");
        m_walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    private void OnValidate()
    {
        if (m_tiles != null)
        {
            foreach (GameObject tile in m_tiles)
            {
                tile.GetComponent<Renderer>().material.SetFloat("_Curvature", m_curvature);
                if (tile.GetComponent<Renderer>().material.GetColor("_BaseColour") != Color.green 
                    && tile.GetComponent<Renderer>().material.GetColor("_BaseColour") != Color.red)
                {
                    tile.GetComponent<Renderer>().material.SetColor("_BaseColour", m_tileColour);
                }
            }
        }

        if (m_walls != null)
        {
            foreach (GameObject wall in m_walls)
            {
                wall.GetComponentInChildren<Renderer>().material.SetFloat("_Curvature", m_curvature);
                wall.GetComponent<Renderer>().material.SetColor("_BaseColour", m_wallColour);

            }
        }
    }
}
