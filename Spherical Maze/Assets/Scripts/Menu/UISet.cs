using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISet : MonoBehaviour
{
    public enum TYPE
    {
        SOUND,
        SOUND_TOGGLE,
        MUSIC,
        MUSIC_TOGGLE,
        FOV,
        WIDTH,
        HEIGHT,
        SUBDIVISION
    }

    [SerializeField]
    TYPE m_type;
    [SerializeField]
    GameObject m_text;
    // Start is called before the first frame update
    void Start()
    {
        switch(m_type)
        {
            //switch on type of ui
            //Set assigned values for different in menu UI elements 
            case TYPE.SOUND:
                GetComponent<UnityEngine.UI.Slider>().value = PersistentInfo.Instance.m_Sound;
                break;
            case TYPE.SOUND_TOGGLE:
                GetComponent<UnityEngine.UI.Toggle>().isOn = PersistentInfo.Instance.m_soundToggle;
                break;
            case TYPE.MUSIC:
                GetComponent<UnityEngine.UI.Slider>().value = PersistentInfo.Instance.m_Music;
                break;
            case TYPE.MUSIC_TOGGLE:
                GetComponent<UnityEngine.UI.Toggle>().isOn = PersistentInfo.Instance.m_musicToggle;
                break;
            case TYPE.FOV:
                GetComponent<UnityEngine.UI.Slider>().value = PersistentInfo.Instance.m_FOV;
                m_text.GetComponent<UnityEngine.UI.Text>().text = PersistentInfo.Instance.m_FOV.ToString();
                break;
            case TYPE.WIDTH:
                GetComponent<UnityEngine.UI.Text>().text = PersistentInfo.Instance.m_MazeWidth.ToString();
                break;
            case TYPE.HEIGHT:
                GetComponent<UnityEngine.UI.Text>().text = PersistentInfo.Instance.m_MazeHeight.ToString();
                break;
            case TYPE.SUBDIVISION:
                GetComponent<UnityEngine.UI.Text>().text = PersistentInfo.Instance.m_Subdivision.ToString();
                break;
        }
    }
}
