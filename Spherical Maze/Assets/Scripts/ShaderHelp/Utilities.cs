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
        //get mesh of model
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        if (PersistentInfo.Instance != null)
        {
            //getsubdivision number 
            for (int i = 0; i < PersistentInfo.Instance.m_Subdivision; i++)
            {
                //subdivide the mesh
                MeshSubdivider.Subdivide(mesh);
            }
        }
        else
        {
            //if PersistentInfo not present subdivide default ammount
            for (int i = 0; i < m_subdivisions; i++)
            {
                //subdivide the mesh
                MeshSubdivider.Subdivide(mesh);
            }
        }
        //set new mesh
        GetComponent<MeshFilter>().mesh = mesh;

        //get mesh again
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        //set bounds of the mesh to large to prevent culling when game thinks model is out of camera.
        //Unity doesnt let you prevent vertex culling in a nice way so setting bounds to all be in view is best way
        //this is so that curve is still seen if when bounds would technically be out of cam
        meshFilter.mesh.bounds = new Bounds(Vector3.zero, Vector3.one * m_boundsScale);
    }
}
