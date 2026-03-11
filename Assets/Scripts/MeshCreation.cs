using NUnit.Framework.Internal;
using System;
using UnityEditor;
using UnityEngine;
using static LNeves.MeshCreation;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

namespace LNeves
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class MeshCreation : MonoBehaviour
    {
        private MeshFilter meshFilter;
        private Mesh mesh;

        public MeshWrap meshWrap;

        [Serializable]
        public struct MeshWrap
        {
            public Vector3[] vertices;
            public int[] triangles;
            public Vector2[] uvs;
        }

        public void InitMesh()
        {
            mesh?.Clear();
            mesh = new Mesh();
            meshFilter = GetComponent<MeshFilter>();
            meshFilter.mesh = mesh;
        }

        public void UpdateMesh(MeshWrap InMeshWrap)
        {
            if (InMeshWrap.triangles.Length % 3.0f == 0)
            {
                mesh.Clear();
                mesh.vertices = InMeshWrap.vertices;
                mesh.triangles = InMeshWrap.triangles;
                mesh.uv = InMeshWrap.uvs;
                mesh.RecalculateNormals();
            }
        }
    }
}