using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int vertexIndex = 0;
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();
    List<Vector2> uvs = new List<Vector2>();

    void Start()
    {
        AddVoxelToDataChunk();
        CreateMesh();
    }

    private void AddVoxelToDataChunk()
    {
        for (int d = 0; d < 6; d++)
        {
            for (int i = 0; i < 6; i++)
            {
                int triangleIndex = Voxel.triangles[d, i];
                vertices.Add(Voxel.vertices[triangleIndex]);
                triangles.Add(vertexIndex);
                uvs.Add(Voxel.uvs[i]);
                vertexIndex++;
            }
        }
    }

    private void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
