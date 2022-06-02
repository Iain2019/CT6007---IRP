using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint3D : MonoBehaviour
{
    //Public Variables
        //Player
    public GameObject m_player;

    //Serialized Fields
        //Visual 
    [SerializeField]
    float m_endDistance;
    [SerializeField]
    Color m_colorChanage;
        //Info
    [SerializeField]
    GameObject m_InfoCanvas;

    //Private Variables
    private bool m_mouseOver = false;

    // Update is called once per frame
    void Update()
    {
        if (m_InfoCanvas.activeInHierarchy)
        {
            //player is looking at
            if (m_mouseOver)
            {
                //if interactable allow input for ending level
                if (Input.GetKeyDown(KeyCode.E))
                {
                    //unlock cursor
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    SceneManager.LoadScene(0);
                }
            }

            if (Vector3.Distance(transform.position, m_player.transform.position) > m_endDistance)
            {
                //if player goes out of distance show end UI
                m_InfoCanvas.SetActive(false);
            }
        }
        else
        {
            //if player goes into distance show end UI
            if (Vector3.Distance(transform.position, m_player.transform.position) <= m_endDistance)
            {
                m_InfoCanvas.SetActive(true);
            }
        }
    }

    private void OnMouseEnter()
    {
        if (m_InfoCanvas.activeInHierarchy)
        {
            //set ui colour to show mouse over
            m_InfoCanvas.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color += m_colorChanage;
            m_mouseOver = true;
        }
    }

    private void OnMouseExit()
    {
        if (m_InfoCanvas.activeInHierarchy)
        {
            //set ui colour to show mouse unover
            m_InfoCanvas.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().color -= m_colorChanage;
            m_mouseOver = false;
        }
    }
}
