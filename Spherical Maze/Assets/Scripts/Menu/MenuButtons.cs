using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtons : MonoBehaviour
{
    //Serialized Fields
        //Main Menu Buttons
    [SerializeField]
    GameObject m_mainPanel;
    [SerializeField]
    GameObject m_stereographicPanel;
    [SerializeField]
    GameObject m_gnomonicPanel;
    [SerializeField]
    GameObject m_hyperbolicPanel;
    [SerializeField]
    GameObject m_curvedPanel;
    [SerializeField]
    GameObject m_optionsPanel;
        //Options Menu Buttons
    [SerializeField]
    GameObject m_FOVText;

    //Main Menu Buttons
    //Toggle menu panles based on buttons
    public void OnStereographicButton()
    {
        m_mainPanel.SetActive(false);
        m_stereographicPanel.SetActive(true);
    }
    public void OnGnomonicButton()
    {
        m_mainPanel.SetActive(false);
        m_gnomonicPanel.SetActive(true);
    }
    public void OnHyperbolicButton()
    {
        m_mainPanel.SetActive(false);
        m_hyperbolicPanel.SetActive(true);
    }
    public void OnCurvedButton()
    {
        m_mainPanel.SetActive(false);
        m_curvedPanel.SetActive(true);
    }
    public void OnOptionsButton()
    {
        m_mainPanel.SetActive(false);
        m_optionsPanel.SetActive(true);
    }
    public void OnExitButton()
    {
        Application.Quit();
    }

    //Spherical/Hyperbolic Menu Button
    public void OnWidthInput(UnityEngine.UI.InputField a_input)
    {
        //check inout is within allowed range
        if (int.Parse(a_input.text) > 100)
        {
            a_input.text = "100";
        }
        else if (int.Parse(a_input.text) < 1)
        {
            a_input.text = "1";
        }
        else
        {
            PersistentInfo.Instance.m_MazeWidth = int.Parse(a_input.text);
        }
    }
    public void OnHeightInput(UnityEngine.UI.InputField a_input)
    {
        //check inout is within allowed 
        if (int.Parse(a_input.text) > 100)
        {
            a_input.text = "100";
        }
        else if (int.Parse(a_input.text) < 1)
        {
            a_input.text = "1";
        }
        else
        {
            PersistentInfo.Instance.m_MazeHeight = int.Parse(a_input.text);
        }
    }
    public void OnSubdivisionInput(UnityEngine.UI.InputField a_input)
    {
        //check inout is within allowed range
        if (int.Parse(a_input.text) > 3)
        {
            a_input.text = "3";
        }
        else if (int.Parse(a_input.text) < 0)
        {
            a_input.text = "0";
        }
        else
        {
            PersistentInfo.Instance.m_Subdivision = int.Parse(a_input.text);
        }
    }
    public void OnStartButton(int a_sceneIndex)
    {
        SceneManager.LoadScene(a_sceneIndex);
    }
    public void OnStereographicBackButton()
    {
        m_stereographicPanel.SetActive(false);
        m_mainPanel.SetActive(true);
    }
    public void OnGnomonicBackButton()
    {
        m_gnomonicPanel.SetActive(false);
        m_mainPanel.SetActive(true);
    }
    public void OnHyperbolicBackButton()
    {
        m_hyperbolicPanel.SetActive(false);
        m_mainPanel.SetActive(true);
    }
    public void OnCurvedBackButton()
    {
        m_curvedPanel.SetActive(false);
        m_mainPanel.SetActive(true);
    }

    //Options Menu Buttons
    public void OnSoundToggle(UnityEngine.UI.Toggle a_soundToggle)
    {
        PersistentInfo.Instance.m_soundToggle = a_soundToggle.GetComponent<UnityEngine.UI.Toggle>().isOn;
    }
    public void OnSoundSlider(UnityEngine.UI.Slider a_soundSlider)
    {
        PersistentInfo.Instance.m_Sound = a_soundSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }
    public void OnMusicToggle(UnityEngine.UI.Toggle a_musicToggle)
    {
        PersistentInfo.Instance.m_musicToggle = a_musicToggle.GetComponent<UnityEngine.UI.Toggle>().isOn;
    }
    public void OnMusicSlider(UnityEngine.UI.Slider a_musicSlider)
    {
        PersistentInfo.Instance.m_Music = a_musicSlider.GetComponent<UnityEngine.UI.Slider>().value;
    }
    public void OnFOVSlider(UnityEngine.UI.Slider a_POVSlider)
    {
        PersistentInfo.Instance.m_FOV = a_POVSlider.GetComponent<UnityEngine.UI.Slider>().value;
        m_FOVText.GetComponent<UnityEngine.UI.Text>().text = a_POVSlider.GetComponent<UnityEngine.UI.Slider>().value.ToString();
    }
    public void OnOptionsBackButton()
    {
        m_optionsPanel.SetActive(false);
        m_mainPanel.SetActive(true);
    }
}
