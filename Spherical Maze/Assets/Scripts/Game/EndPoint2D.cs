using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndPoint2D : MonoBehaviour
{
    //Public Variables
        //Player
    public GameObject m_player;
        //Info
    public GameObject m_InfoCanvas;

    //Serialized Fields
    //Visual 
    [SerializeField]
    float m_endDistance;

    // Update is called once per frame
    void Update()
    {
        if (m_InfoCanvas.activeInHierarchy)
        {
            //if interactable allow input for ending level
            if (Input.GetKeyDown(KeyCode.E))
            {
                //unlock cursor
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(0);
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
}
