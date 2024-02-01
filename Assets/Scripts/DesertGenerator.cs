using UnityEngine;

public class DesertDunesGenerator : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20.0f;
    public float duneHeight = 5.0f;
    public float transitionLength = 20.0f;

    private MeshCollider meshCollider;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        GenerateTerrain();
    }

    void GenerateTerrain()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        Vector3[] vertices = new Vector3[width * height];
        Vector2[] uv = new Vector2[width * height];
        int[] triangles = new int[(width - 1) * (height - 1) * 6];
        int triIndex = 0;

        for (int z = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                float xCoord = (float)x / width * scale;
                float zCoord = (float)z / height * scale;
                float y = Mathf.PerlinNoise(xCoord, zCoord) * duneHeight;

                // Smooth transition towards the last few vertices
                float transitionFactor = Mathf.Clamp01((float)(width - x) / transitionLength);
                y *= transitionFactor;

                vertices[z * width + x] = new Vector3(x, y, z);
                uv[z * width + x] = new Vector2((float)x / width, (float)z / height);

                if (x < width - 1 && z < height - 1)
                {
                    triangles[triIndex + 0] = z * width + x;
                    triangles[triIndex + 1] = (z + 1) * width + x;
                    triangles[triIndex + 2] = z * width + x + 1;

                    triangles[triIndex + 3] = (z + 1) * width + x;
                    triangles[triIndex + 4] = (z + 1) * width + x + 1;
                    triangles[triIndex + 5] = z * width + x + 1;

                    triIndex += 6;
                }
            }
        }

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        // Flip normals by multiplying by -1
        Vector3[] normals = mesh.normals;
        for (int i = 0; i < normals.Length; i++)
        {
            normals[i] = -normals[i];
        }
        mesh.normals = normals;

        // Update the Mesh Collider
        meshCollider.sharedMesh = mesh;
    }
}
