using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvatureShaderController : MonoBehaviour
{
    [SerializeField]
    float m_curvature = 0.0f;
    [SerializeField]
    float m_radius = 1.0f;
    [SerializeField]
    Color m_tileColour = Color.white;
    [SerializeField]
    Color m_wallColour = Color.black;

    GameObject[] m_tiles;
    GameObject[] m_walls;

    // Start is called before the first frame update
    void Start()
    {
        //get all tiles and walls
        m_tiles = GameObject.FindGameObjectsWithTag("Tile");
        m_walls = GameObject.FindGameObjectsWithTag("Wall");
    }

    private void OnValidate()
    {
        //On volidate is called when the unity UI is updated or before start
        UpdateShader();
    }

    public void OnCurveSlider(UnityEngine.UI.Slider a_curveSlider)
    {
        //curve slider updated set value
        m_curvature = a_curveSlider.GetComponent<UnityEngine.UI.Slider>().value;
        UpdateShader();
    }

    public void OnRadiusSlider(UnityEngine.UI.Slider a_radiusSlider)
    {
        //radius slider updated set value
        m_radius = a_radiusSlider.GetComponent<UnityEngine.UI.Slider>().value;
        UpdateShader();
    }

    private void UpdateShader()
    {
        //send shader all values
        if (m_tiles != null)
        {
            //foreach tile send over the data
            foreach (GameObject tile in m_tiles)
            {
                tile.GetComponent<Renderer>().material.SetFloat("_Curvature", m_curvature);
                tile.GetComponent<Renderer>().material.SetFloat("_Radius", m_radius);

                //if colour isnt green or red set it it (green or red are start and stop points)
                if (tile.GetComponent<Renderer>().material.GetColor("_BaseColour") != Color.green
                    && tile.GetComponent<Renderer>().material.GetColor("_BaseColour") != Color.red)
                {
                    tile.GetComponent<Renderer>().material.SetColor("_BaseColour", m_tileColour);
                }
            }
        }

        if (m_walls != null)
        {
            //foreach tile send over the data
            foreach (GameObject wall in m_walls)
            {
                wall.GetComponentInChildren<Renderer>().material.SetFloat("_Curvature", m_curvature);
                wall.GetComponentInChildren<Renderer>().material.SetFloat("_Radius", m_radius);
                wall.GetComponent<Renderer>().material.SetColor("_BaseColour", m_wallColour);

            }
        }
    }
}
