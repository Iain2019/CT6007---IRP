using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController3D : MonoBehaviour
{
    //Serialized Fields
        //Control
    [SerializeField]
    float m_movementSpeed;
    [SerializeField]
    float m_rotationSpeed;
    [SerializeField]
    float m_maxYRot;
        //Cameras
    [SerializeField]
    GameObject m_camera;

    //Private Variables
    //Look locking
    bool m_lockLook = false;

    // Start is called before the first frame update
    void Start()
    {
        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        m_lockLook = false;
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
            transform.Translate(Vector3.left * m_movementSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            //translate based on input
            transform.Translate(Vector3.right * m_movementSpeed * Time.deltaTime);
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            //alt unlocks cursor for ui interaction
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            m_lockLook = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            //lock controls back
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            m_lockLook = false;

        }

        if (!m_lockLook)
        {
            //get axis input of the mouse
            float xRot = Input.GetAxis("Mouse X");
            float yRot = Input.GetAxis("Mouse Y");

            //roatate the body of the player
            transform.RotateAround(transform.position, Vector3.up, xRot * m_rotationSpeed * Time.deltaTime);
            //float angle = Quaternion.Angle(new Quaternion(0, 0, 0, 1), m_camera.transform.localRotation);
            //if (angle + Mathf.Abs(yRot) < m_maxYRot)
            //{
            //    m_camera.transform.RotateAround(m_camera.transform.position, transform.right, -yRot * m_rotationSpeed * Time.deltaTime);
            //}
            //else
            //{
            //    if (m_camera.transform.localRotation.x > 0)
            //    {
            //        m_camera.transform.RotateAround(m_camera.transform.position, transform.right, -0.1f);
            //    }
            //    else
            //    {
            //        m_camera.transform.RotateAround(m_camera.transform.position, transform.right, 0.1f);
            //    }
            //}

            //ratote camera if the input doesnt go over max roations
            if (m_camera.transform.localEulerAngles.x < 180.0f)
            {
                //if rotation isnt over the max angle plus the inputed rotation
                if (m_camera.transform.localEulerAngles.x + (-yRot * m_rotationSpeed * Time.deltaTime) < m_maxYRot)
                {
                    m_camera.transform.RotateAround(m_camera.transform.position, transform.right, -yRot * m_rotationSpeed * Time.deltaTime);
                }
            }
            else
            {
                if (m_camera.transform.localEulerAngles.x - 360 + (-yRot * m_rotationSpeed * Time.deltaTime) > -m_maxYRot)
                {
                    m_camera.transform.RotateAround(m_camera.transform.position, transform.right, -yRot * m_rotationSpeed * Time.deltaTime);
                }
            }
        }
    }
}
