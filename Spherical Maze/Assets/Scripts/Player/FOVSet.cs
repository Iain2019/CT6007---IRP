using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOVSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PersistentInfo.Instance != null)
        {
            //get fox from pi and set
            Camera.main.fieldOfView = PersistentInfo.Instance.m_FOV;
        }
    }
}
