using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
    [SerializeField]
    float m_boundsScale = 1000.0f;

    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        MeshSubdivider.Subdivide(mesh);
        MeshSubdivider.Subdivide(mesh);
        MeshSubdivider.Subdivide(mesh);
        MeshSubdivider.Subdivide(mesh); 
        GetComponent<MeshFilter>().mesh = mesh;

        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * m_boundsScale);
    }
}
