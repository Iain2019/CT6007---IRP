using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    int m_DDOLCount = 0;
    bool m_firstDDOL = false;

    //Don't destroy on load for moving to another scene
    private void Awake()
    {
        m_DDOLCount = FindObjectsOfType<DontDestroyOnLoad>().Length;

        if (m_DDOLCount == 1)
        {
            m_firstDDOL = true;
        }
        else if (!m_firstDDOL)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
