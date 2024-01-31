using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CustomTerrainGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshFilter meshFilter;
    Vector3[] vertices;
    int[] triangles;
    Vector2[] uv;

    // Set the size of the mesh base
    public int totalSize = 300; // Total size of the terrain
    public int flatSize = 100;  // Size of the flat center area
    public int desertSize = 100; // Size of the surrounding desert area
    public float strength = 0.3f;

    const float HeightMultiplier = 2f;

    void Start()
    {
        InitializeMesh();
        CreateShape();
        UpdateMesh();
    }

    void InitializeMesh()
    {
        mesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;
    }

    void CreateShape()
    {
        vertices = new Vector3[(totalSize + 1) * (totalSize + 1)];
        uv = new Vector2[vertices.Length];

        for (int i = 0, z = 0; z <= totalSize; z++)
        {
            for (int x = 0; x <= totalSize; x++)
            {
                float y = CalculateHeight(x, z);
                vertices[i] = new Vector3(x, y, z);
                uv[i] = new Vector2((float)x / totalSize, (float)z / totalSize);
                i++;
            }
        }

        CreateTriangles();
    }

    float CalculateHeight(int x, int z)
    {
        float flatCenterHeight = 0f;
        float desertHeight = Mathf.PerlinNoise(x * strength, z * strength) * HeightMultiplier;

        // Check if the point is inside the flat center area
        if (x >= (totalSize - flatSize) / 2 && x <= (totalSize + flatSize) / 2 &&
            z >= (totalSize - flatSize) / 2 && z <= (totalSize + flatSize) / 2)
        {
            return flatCenterHeight; // Flat center area
        }
        else
        {
            return desertHeight; // Surrounding desert area
        }
    }

    void CreateTriangles()
    {
        triangles = new int[totalSize * totalSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < totalSize; z++)
        {
            for (int x = 0; x < totalSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + totalSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + totalSize + 1;
                triangles[tris + 5] = vert + totalSize + 2;

                vert++;
                tris += 6;
            }
            vert++;
        }
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uv;
        mesh.RecalculateNormals();
    }
}
