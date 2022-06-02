using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //ensure that the end of the level ui looks at player
        transform.LookAt(Camera.main.transform.position, Vector3.up);
        transform.rotation = Quaternion.LookRotation(-transform.forward, Vector3.up);
    }
}
