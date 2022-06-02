using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshSubdivider
{
    static List<Vector3> m_verticies;
    static List<Vector3> m_normals;
    static List<int> m_indices;
     
    static Dictionary<uint, int> m_newVerticies;

    // Start is called before the first frame update
    static int GetNewVertex(int a_tri1, int a_tri2)
    {
        //get tris for face
        uint tri1 = ((uint)a_tri1 << 16) | (uint)a_tri2;
        uint tri2 = ((uint)a_tri2 << 16) | (uint)a_tri1;

        //if not in new vertices 
        if (m_newVerticies.ContainsKey(tri2))
        {
            return m_newVerticies[tri2];
        }
        if (m_newVerticies.ContainsKey(tri1))
        {
            return m_newVerticies[tri1];
        }

        int newIndex = m_verticies.Count;
        m_newVerticies.Add(tri1, newIndex);
        //add to new point in dic

        //add to verticies & normals
        m_verticies.Add((m_verticies[a_tri1] + m_verticies[a_tri2]) * 0.5f);
        m_normals.Add((m_normals[a_tri1] + m_normals[a_tri2]).normalized);

        return newIndex;
    }

    static public void Subdivide(Mesh a_mesh)
    {
        m_newVerticies = new Dictionary<uint, int>();

        m_verticies = new List<Vector3>(a_mesh.vertices);
        m_normals = new List<Vector3>(a_mesh.normals);

        m_indices = new List<int>();

        int[] triangles = a_mesh.triangles;

        //increment for 3 each time, subdividing mesh adds 3 new triangles
        for (int i = 0; i < triangles.Length; i += 3)
        {
            //get tris wither side in list
            int tri1 = triangles[i + 0];
            int tri2 = triangles[i + 1];
            int tri3 = triangles[i + 2];

            //get vertex between already existing
            int vert1 = GetNewVertex(tri1, tri2);
            int vert2 = GetNewVertex(tri2, tri3);
            int vert3 = GetNewVertex(tri3, tri1);

            //add info for each new tri
            m_indices.Add(tri1);
            m_indices.Add(vert1);
            m_indices.Add(vert3);

            m_indices.Add(tri2);
            m_indices.Add(vert2);
            m_indices.Add(vert1);

            m_indices.Add(tri3);
            m_indices.Add(vert3);
            m_indices.Add(vert2);

            m_indices.Add(vert1);
            m_indices.Add(vert2);
            m_indices.Add(vert3);
        }
        //add new verticies' info to array
        a_mesh.vertices = m_verticies.ToArray();
        a_mesh.normals = m_normals.ToArray();
        a_mesh.triangles = m_indices.ToArray();

        //reset (due to static function)
        m_newVerticies = null;
        m_verticies = null;
        m_newVerticies = null;
        m_indices = null;
    }
}
