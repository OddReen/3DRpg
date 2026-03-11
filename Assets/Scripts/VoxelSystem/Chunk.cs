using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;

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
                    if (y > VoxelData.ChunkHeight * .75f)
                    {
                        _voxelMap[x, y, z] = 1;
                    }
                    else
                    {
                        _voxelMap[x, y, z] = 1;
                    }
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
                    CreateVoxelMeshData(new Vector3Int(x, y, z));
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
        Debug.Log(_uvs.Count);
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

    private void CreateVoxelMeshData(Vector3Int InVoxelPos)
    {
        // Voxel representation
        int faceNum = 6;
        for (int faceIndex = 0; faceIndex < faceNum; faceIndex++)
        {
            // If there isnt a voxel in given neighbour, draw face
            if (!CheckVoxel(InVoxelPos + VoxelData.faceDirections[faceIndex]))
            {

                int verticesNum = 4;
                for (int verticeIndex = 0; verticeIndex < verticesNum; verticeIndex++)
                {
                    _vertices.Add(VoxelData.vertices[VoxelData.faces[faceIndex, verticeIndex]] + InVoxelPos);
                }
                
                byte blockID = _voxelMap[InVoxelPos.x, InVoxelPos.y, InVoxelPos.z];
                AddTexture(_world.blockTypes[blockID].GetTextureID(faceNum));
                
                for (int t = 0; t < VoxelData.triangles.Length; t++)
                {
                    _triangles.Add(_vertexIndex + VoxelData.triangles[t]);
                }
                _vertexIndex += 4;
            }
        }
    }

    void AddTexture(int InTextureID)
    {
        float y = InTextureID / VoxelData.TextureAtlasSizeInBlocks;
        float x = InTextureID - (y * VoxelData.TextureAtlasSizeInBlocks);

        x *= VoxelData.NormalizedBlockTextureSize;
        y *= VoxelData.NormalizedBlockTextureSize;

        y = 1f - y - VoxelData.NormalizedBlockTextureSize;

        _uvs.Add(new Vector2(x, y));
        _uvs.Add(new Vector2(x, y + VoxelData.NormalizedBlockTextureSize));
        _uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y));
        _uvs.Add(new Vector2(x + VoxelData.NormalizedBlockTextureSize, y + VoxelData.NormalizedBlockTextureSize));
    }
}
