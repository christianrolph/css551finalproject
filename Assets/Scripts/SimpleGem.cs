using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGem : MonoBehaviour
{
    const int numberOfVertecies = 6;//14;
    const int numberOfTriangles = 4;//21;
    public Transform[] VertexPoints = new Transform[4];
    public Transform Origin;
    Vector3 origin;
    Vector3[] verticies;
    Triangle[] triangleList;
    Vector3[] normals;
    public Vector2[] uv;
    int[] triangles = new int[numberOfTriangles * 3];

    // Start is called before the first frame update
    void Start()
    {
        origin = Origin.localPosition;
        drawGem();
    }

    private void Update()
    {
        //drawGem();
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        computeNormals(verticies, normals);
        theMesh.vertices = verticies;
        theMesh.normals = normals;
        theMesh.uv = uv;
        theMesh.triangles = triangles;
    }

    void drawGem()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();
        verticies = new Vector3[numberOfVertecies];
        triangleList = new Triangle[numberOfTriangles];
        normals = new Vector3[verticies.Length];
        uv = new Vector2[verticies.Length];

        //verticies
        verticies[0] = VertexPoints[0].localPosition;
        verticies[1] = VertexPoints[1].localPosition;
        verticies[2] = VertexPoints[2].localPosition;
        verticies[3] = VertexPoints[3].localPosition;
        verticies[4] = VertexPoints[3].localPosition;
        verticies[5] = VertexPoints[3].localPosition;

        //normals
        normals[0] = (verticies[0] - origin);
        normals[1] = (verticies[1] - origin);
        normals[2] = (verticies[2] - origin);
        normals[3] = (verticies[3] - origin);
        normals[4] = (verticies[4] - origin);
        normals[5] = (verticies[5] - origin);

        //uv
        float x = 300;
        float y = 300;
        uv[0] = new Vector2(175f / x, 179f / y);
        uv[1] = new Vector2(175f / x, 179f / y);
        uv[2] = new Vector2(150f / x, 222f / y);
        uv[3] = new Vector2(150f / x, 25f / y);
        uv[4] = new Vector2(5f / x, 277f / y);
        uv[5] = new Vector2(294f / x, 277f / y);

        //triangles
        triangleList[0] = new Triangle(0, 3, 1, 0);
        triangleList[1] = new Triangle(1, 4, 0, 2);
        triangleList[2] = new Triangle(2, 5, 2, 1);
        triangleList[3] = new Triangle(3, 1, 2, 0);
        int index = 0;
        foreach (Triangle tri in triangleList)
        {
            triangles[index] = tri.TriangleVerticies[0];
            triangles[index + 1] = tri.TriangleVerticies[1];
            triangles[index + 2] = tri.TriangleVerticies[2];
            index += 3;
        }

        computeNormals(verticies, normals);

        theMesh.vertices = verticies;
        theMesh.normals = normals;
        theMesh.uv = uv;
        theMesh.triangles = triangles;
    }

    void computeNormals(Vector3[] verticiesArray, Vector3[] normalsArray)
    {
        Vector3[] triangleNormals = new Vector3[numberOfTriangles];
        for (int i = 0; i < numberOfTriangles; i++)
        {
            triangleNormals[i] = faceNormal(
                verticiesArray,
                triangleList[i].TriangleVerticies[0],
                triangleList[i].TriangleVerticies[1],
                triangleList[i].TriangleVerticies[2]);
        }

        for (int vertexNumber = 0; vertexNumber < numberOfVertecies; vertexNumber++)
        {
            // find all the triangles with that vertex
            List<Triangle> trianglesWithThisVertex = new List<Triangle>();
            foreach (Triangle tri in triangleList)
            {
                if (tri.ContainsVertex(vertexNumber))
                {
                    trianglesWithThisVertex.Add(tri);
                }
            }
            Vector3 combinedVectors = Vector3.zero;
            foreach (Triangle tri in trianglesWithThisVertex)
            {
                combinedVectors += triangleNormals[tri.TriangleNumber];
            }

            // normalize them and store
            normalsArray[vertexNumber] = combinedVectors.normalized;
        }

        Vector3 faceNormal(Vector3[] v, int i0, int i1, int i2)
        {
            Vector3 a = v[i1] - v[i0];
            Vector3 b = v[i2] - v[i0];
            return Vector3.Cross(a, b).normalized;
        }
    }
    public class Triangle
    {
        public int TriangleNumber;
        public List<int> TriangleVerticies;

        public Triangle(int triangleNum, int v1, int v2, int v3)
        {
            this.TriangleNumber = triangleNum;
            this.TriangleVerticies = new List<int> { v1, v2, v3 };
        }

        public bool ContainsVertex(int vertex)
        {
            if (this.TriangleVerticies.Contains(vertex))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
