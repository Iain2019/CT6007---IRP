using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    [SerializeField]
    float m_boundsScale = 1000.0f;
    [SerializeField]
    int m_subdivisions = 3;

    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (PersistentInfo.Instance != null)
        {
            for (int i = 0; i < PersistentInfo.Instance.m_Subdivision; i++)
            {
                MeshSubdivider.Subdivide(mesh);
            }
        }
        else
        {
            for (int i = 0; i < m_subdivisions; i++)
            {
                MeshSubdivider.Subdivide(mesh);
            }
        }

        GetComponent<MeshFilter>().mesh = mesh;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * m_boundsScale);
    }
}
