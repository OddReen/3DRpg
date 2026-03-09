using UnityEngine;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    int _vertexIndex = 0;
    List<Vector3> _vertices = new List<Vector3>();
    List<int> _triangles = new List<int>();
    List<Vector2> _uvs = new List<Vector2>();

    byte[,,] _voxelMap = new byte[VoxelData.ChunkWidth, VoxelData.ChunkHeight, VoxelData.ChunkWidth];

    World _world;

    void Start()
    {
        _world = GameObject.Find("World").GetComponent<World>();

        PopulateVoxelMap();
        CreateMeshData();
        CreateMesh();
    }

    void PopulateVoxelMap()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    _voxelMap[x, y, z] = 0;
                }
            }
        }
    }

    private void CreateMeshData()
    {
        for (int y = 0; y < VoxelData.ChunkHeight; y++)
        {
            for (int x = 0; x < VoxelData.ChunkWidth; x++)
            {
                for (int z = 0; z < VoxelData.ChunkWidth; z++)
                {
                    AddVoxelToDataChunk(new Vector3Int(x, y, z));
                }
            }
        }
    }

    private void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;

        mesh.vertices = _vertices.ToArray();
        mesh.triangles = _triangles.ToArray();
        mesh.uv = _uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

    // Check whether there is a voxel in the given coordinate
    bool CheckVoxel(Vector3Int InCoordinate)
    {
        bool XValid = InCoordinate.x >= 0 && InCoordinate.x <= VoxelData.ChunkWidth - 1;
        bool YValid = InCoordinate.y >= 0 && InCoordinate.y <= VoxelData.ChunkHeight - 1;
        bool ZValid = InCoordinate.z >= 0 && InCoordinate.z <= VoxelData.ChunkWidth - 1;
        if (XValid && YValid && ZValid)
        {
            return _world.blockTypes[_voxelMap[InCoordinate.x, InCoordinate.y, InCoordinate.z]].isSolid;
        }
        return false;
    }

    private void AddVoxelToDataChunk(Vector3Int InCoordinate)
    {
        // Voxel representation
        int faceNum = 6;
        for (int faceIndex = 0; faceIndex < faceNum; faceIndex++)
        {
            // If there isnt a voxel in given direction, draw face
            if (!CheckVoxel(InCoordinate + VoxelData.faceDirections[faceIndex]))
            {
                int verticesNum = 4;
                for (int verticeIndex = 0; verticeIndex < verticesNum; verticeIndex++)
                {
                    _vertices.Add(VoxelData.vertices[VoxelData.faces[faceIndex, verticeIndex]] + InCoordinate);
                    _uvs.Add(VoxelData.uvs[verticeIndex]);
                }

                for (int t = 0; t < VoxelData.triangles.Length; t++)
                {
                    _triangles.Add(_vertexIndex + VoxelData.triangles[t]);
                }
                _vertexIndex += 4;
            }
        }
    }
}
