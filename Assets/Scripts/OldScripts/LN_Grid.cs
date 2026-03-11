using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

namespace LNeves
{
    public class LN_Grid : MonoBehaviour
    {
        [Header("Waving Test")]
        public bool IsWavingTestEnabled = false;
        private Vector2 waveDirection;
        public float speed;

        [SerializeField] Chunk currentChunk;
        
        public List<EntityGroupTemplate> EntityGroupTemplates = new List<EntityGroupTemplate>();
        public List<Vector3> GridCoordinates = new List<Vector3>();
        public List<GameObject> CachedEntities = new List<GameObject>();
        public MeshCreation meshCreation;

        [Serializable] public struct Chunk
        {
            public int width;
            public int length;
            public int height;
            
            public float perlinScale;
            public float perlinXOffset;
            public float perlinZOffset;
        };
        [Serializable] public struct VoxelData
        {
            public Vector3 voxelCoordinates;
        };
        [Serializable] public struct EntityData
        {
            public GameObject entityGameObject;
            public Vector3 entityCoordinates;
        };
        [Serializable] public struct EntityGroupTemplate
        {
            public EntityData entityTemplate;
            public int entityGroupSize;
        };

        private void Awake()
        {
            GridCoordinates = GenerateGridCoordinates(currentChunk.width, currentChunk.length);

            // Setup grid mesh
            meshCreation.meshWrap.vertices = GridCoordinates.ToArray();
            SetupUVs();
            GenerateTriangles();

            // Spawn entities
            SpawnEntityGroups();
        }

        private void Update()
        {
            WaveTest();
        }

        // Grid Generation
        private List<Vector3> GenerateGridCoordinates(int width, int length)
        {
            List<Vector3> OutGridCoordinates = new List<Vector3>();
            
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < length; z++)
                {
                    float XCord = (float)(x + currentChunk.perlinXOffset) / (float)(width * currentChunk.perlinScale);
                    float ZCord = (float)(z + currentChunk.perlinZOffset) / (float)(width * currentChunk.perlinScale);
                    float YCord = Mathf.PerlinNoise(XCord, ZCord);
                    YCord *= currentChunk.height;
                    OutGridCoordinates.Add(new Vector3(x, (int)YCord, z));
                }
            }

            return OutGridCoordinates;
        }

        // Mesh Terrain Generation
        private void SetupUVs()
        {
            Vector2[] uvs = new Vector2[GridCoordinates.Count];

            for (int z = 0; z < currentChunk.length; z++)
            {
                for (int x = 0; x < currentChunk.width; x++)
                {
                    int i = z * currentChunk.width + x;

                    uvs[i] = new Vector2(
                        (float)x / (currentChunk.width - 1),
                        (float)z / (currentChunk.length - 1)
                    );
                }
            }
            meshCreation.meshWrap.uvs = uvs;
        }
        private void GenerateTriangles()
        {
            meshCreation.InitMesh();

            List<int> triangles = new List<int>();

            for (int i = 0; i < GridCoordinates.Count - currentChunk.width; i++)
            {
                // Quad mesh
                if ((i + 1) % currentChunk.width != 0)
                {
                    // First triangle
                    triangles.Add(i);
                    triangles.Add(i + 1);
                    triangles.Add(i + currentChunk.width);

                    // Second triangle
                    triangles.Add(i + currentChunk.width);
                    triangles.Add(i + 1);
                    triangles.Add(i + currentChunk.width + 1);
                }
            }

            meshCreation.meshWrap.triangles = triangles.ToArray();
            meshCreation.UpdateMesh(meshCreation.meshWrap);
        }

        // Entity System
        private void SpawnEntityGroups()
        {
            foreach (EntityGroupTemplate InEntityGroupTemplate in EntityGroupTemplates)
            {
                SpawnEntityGroup(InEntityGroupTemplate);
            }
        }
        private void SpawnEntityGroup(EntityGroupTemplate InEntityGroupTemplate)
        {
            for (int i = 0; i < InEntityGroupTemplate.entityGroupSize; i++)
            {
                int randomGridIndex = UnityEngine.Random.Range(0, GridCoordinates.Count - 1);
                GameObject NewEntity = SpawnEntity(InEntityGroupTemplate.entityTemplate, GridCoordinates[randomGridIndex]);
                CachedEntities.Add(NewEntity);
            }
        }
        private GameObject SpawnEntity(EntityData InEntityData, Vector3 InCoordinates)
        {
            return Instantiate(InEntityData.entityGameObject, InCoordinates, Quaternion.identity);
        }

#region In Editor Tests
        private void OnValidate()
        {
            meshCreation.UpdateMesh(meshCreation.meshWrap);
        }
        private void WaveTest()
        {
            float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");
            Vector2 input = new Vector2(x, y);

            if(input.magnitude != 0.0f && IsWavingTestEnabled)
            {
                waveDirection = input.normalized;
                currentChunk.perlinXOffset += waveDirection.x * Time.deltaTime * speed;
                currentChunk.perlinZOffset += waveDirection.y * Time.deltaTime * speed;
                meshCreation.UpdateMesh(meshCreation.meshWrap);
            }
            
            GridCoordinates = GenerateGridCoordinates(currentChunk.width, currentChunk.length);
            meshCreation.meshWrap.vertices = GridCoordinates.ToArray();
        }
    }
#endregion
}
