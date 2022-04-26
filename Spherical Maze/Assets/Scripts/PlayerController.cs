using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject m_camera;
    [SerializeField]
    private float m_movementSpeed;
    [SerializeField]
    private float m_rotationSpeed;
    [SerializeField]
    private float m_minXRot;
    [SerializeField]
    private float m_maxXRot;
    [SerializeField]
    private float m_minYRot;
    [SerializeField]
    private float m_maxYRot;

    private Rigidbody m_rigidbody;
    private Quaternion m_originalRot;
    // Start is called before the first frame update
    void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_originalRot = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            m_rigidbody.AddForce(transform.forward * m_movementSpeed, ForceMode.Impulse);
            //transform.position = (transform.position + transform.forward) * m_movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            m_rigidbody.AddForce(-transform.forward * m_movementSpeed, ForceMode.Impulse);
            //transform.position = (transform.position - transform.forward) * m_movementSpeed * Time.deltaTime;

        }
        if (Input.GetKey(KeyCode.A))
        {
            m_rigidbody.AddForce(-transform.right * m_movementSpeed, ForceMode.Impulse);
            //transform.position = (transform.position - transform.right) * m_movementSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            m_rigidbody.AddForce(transform.right * m_movementSpeed, ForceMode.Impulse);
            //transform.position = (transform.position + transform.right) * m_movementSpeed * Time.deltaTime;
        }

        float xRot = Input.GetAxis("Mouse X");
        float yRot = Input.GetAxis("Mouse Y");

        if (transform.rotation.y + xRot > m_minXRot && transform.rotation.y + xRot < m_maxXRot)
        {
            transform.RotateAround(transform.position, Vector3.up, xRot * m_rotationSpeed);
        }
        if (m_camera.transform.rotation.x + yRot > m_minYRot && m_camera.transform.rotation.x + yRot < m_maxYRot)
        {
            m_camera.transform.RotateAround(m_camera.transform.position, transform.right, -yRot * m_rotationSpeed);
        }
    }
}
