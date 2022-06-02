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
        //find all ddons
        m_DDOLCount = FindObjectsOfType<DontDestroyOnLoad>().Length;

        //if there are more than 1 ddol set as not first
        if (m_DDOLCount == 1)
        {
            m_firstDDOL = true;
        }
        //destroy self if not first
        else if (!m_firstDDOL)
        {
            Destroy(this.gameObject);
        }
        //prevent destroy on scene change
        DontDestroyOnLoad(this.gameObject);
    }
}
