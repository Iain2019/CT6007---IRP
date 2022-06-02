using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController2D : MonoBehaviour
{
    //Serialized Fields
        //Control
    [SerializeField]
    float m_movementSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        //basic wsad controls
        if (Input.GetKey(KeyCode.W))
        {
            //translate based on input
            transform.Translate(Vector3.forward * m_movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            //translate based on input
            transform.Translate(Vector3.back * m_movementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //translate based on input
            transform.Translate(Vector3.right * m_movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //translate based on input
            transform.Translate(Vector3.left * m_movementSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //alt unlocks cursor for ui interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            //lock controls back
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
