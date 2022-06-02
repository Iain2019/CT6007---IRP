using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameButtons : MonoBehaviour
{
    [SerializeField]
    GameObject m_mathsShown;
    [SerializeField]
    GameObject m_mathsHiden;

    public void OnMenuButton()
    {
        //unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        //to main menu
        SceneManager.LoadScene(0);
    }

    //maths panel toggle
    public void OnMathsShowButton()
    {
        m_mathsShown.SetActive(true);
        m_mathsHiden.SetActive(false);
    }
    public void OnMathsHideButton()
    {
        m_mathsShown.SetActive(false);
        m_mathsHiden.SetActive(true);
    }
}
