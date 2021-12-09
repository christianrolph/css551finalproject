using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    const int NUMBER_OF_VERTICIES = 26;
    const int NUMBER_OF_TRIANGLES = 24;

    Vector3 origin;
    Vector3[] verticies;
    Triangle[] triangleList;
    Vector3[] normals;
    public Vector2[] uv;
    int[] triangles = new int[NUMBER_OF_TRIANGLES * 3];
    GameObject[] spheres;

    public float r1 = 0.2f;
    public float r2 = 0.275f;
    public float y1 = 0.6f;
    public float y2 = 0.8f;
    public float y3 = 1.0f;

    float a, b, c, d, e, f; //line lengths

    float x1, x2, z1, z2;

    // Start is called before the first frame update
    void Start()
    {
        x1 = Mathf.Cos(30 * Mathf.Deg2Rad) * r1;
        x2 = Mathf.Cos(60 * Mathf.Deg2Rad) * r2;
        z1 = Mathf.Tan(30 * Mathf.Deg2Rad) * x1;
        z2 = Mathf.Sin(60 * Mathf.Deg2Rad) * r2;
        drawGem();
        computeNormals();
        displayNormals();
        displayVerties();
    }

    // Update is called once per frame
    void Update()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        //computeNormals(verticies, normals);
        theMesh.vertices = verticies;
        theMesh.normals = normals;
        theMesh.uv = uv;
        theMesh.triangles = triangles;
        x1 = Mathf.Cos(30 * Mathf.Deg2Rad) * r1;
        x2 = Mathf.Cos(60 * Mathf.Deg2Rad) * r2;
        z1 = Mathf.Tan(30 * Mathf.Deg2Rad) * x1;
        z2 = Mathf.Sin(60 * Mathf.Deg2Rad) * r2;
    }

    void generateVerticies()
    {
        verticies = new Vector3[NUMBER_OF_VERTICIES];
        spheres = new GameObject[NUMBER_OF_VERTICIES];

        //verticies[0] = new Vector3(0, 0, 0);
        //verticies[1] = new Vector3(-x1, y1, -z1);
        //verticies[2] = new Vector3(0, y1, -r1);
        //verticies[3] = new Vector3(x1, y1, -z1);
        //verticies[4] = new Vector3(x1, y1, z1);
        //verticies[5] = new Vector3(0, y1, r1);
        //verticies[6] = new Vector3(-x1, y1, z1);
        //verticies[7] = new Vector3(-r2, y2, 0);
        //verticies[8] = new Vector3(-x2, y2, -z2);
        //verticies[9] = new Vector3(x2, y2, -z2);
        //verticies[10] = new Vector3(r2, y2, 0);
        //verticies[11] = new Vector3(x2, y2, z2);
        //verticies[12] = new Vector3(-x2, y2, z2);
        //verticies[13] = new Vector3(0, y3, 0);

        verticies[0] = new Vector3(0, 0, 0);//0
        verticies[1] = new Vector3(-x1, y1, -z1);//1-1
        verticies[2] = new Vector3(-x1, y1, -z1);//1-2
        verticies[3] = new Vector3(0, y1, -r1);//2-1
        verticies[4] = new Vector3(0, y1, -r1);//2-2
        verticies[5] = new Vector3(x1, y1, -z1);//3-1
        verticies[6] = new Vector3(x1, y1, -z1);//3-2
        verticies[7] = new Vector3(x1, y1, z1);//4-1
        verticies[8] = new Vector3(x1, y1, z1);//4-2
        verticies[9] = new Vector3(0, y1, r1);//5-1
        verticies[10] = new Vector3(0, y1, r1);//5-2
        verticies[11] = new Vector3(-x1, y1, z1);//6-1
        verticies[12] = new Vector3(-x1, y1, z1);//6-2
        verticies[13] = new Vector3(-r2, y2, 0);//7-1
        verticies[14] = new Vector3(-r2, y2, 0);//7-2
        verticies[15] = new Vector3(-r2, y2, 0);//7-3
        verticies[16] = new Vector3(-x2, y2, -z2);//8
        verticies[17] = new Vector3(x2, y2, -z2);//9-1
        verticies[18] = new Vector3(x2, y2, -z2);//9-2
        verticies[19] = new Vector3(x2, y2, -z2);//9-3
        verticies[20] = new Vector3(r2, y2, 0);//10
        verticies[21] = new Vector3(x2, y2, z2);//11-1
        verticies[22] = new Vector3(x2, y2, z2);//11-2
        verticies[23] = new Vector3(x2, y2, z2);//11-3
        verticies[24] = new Vector3(-x2, y2, z2);//12
        verticies[25] = new Vector3(0, y3, 0);//13
    }

    void drawGem()
    {
        Mesh theMesh = GetComponent<MeshFilter>().mesh;
        theMesh.Clear();
        verticies = new Vector3[NUMBER_OF_VERTICIES];
        triangleList = new Triangle[NUMBER_OF_TRIANGLES];
        normals = new Vector3[verticies.Length];
        uv = new Vector2[verticies.Length];

        generateVerticies();

        //normals
        for (int i = 0; i < NUMBER_OF_VERTICIES; i++)
        {
            normals[i] = new Vector3(0, 1, 0);
        }

        //uv

        //triangles
        //triangleList[0] = new Triangle(0, 2, 0, 1);
        //triangleList[1] = new Triangle(1, 3, 0, 2);
        //triangleList[2] = new Triangle(2, 4, 0, 3);
        //triangleList[3] = new Triangle(3, 5, 0, 4);
        //triangleList[4] = new Triangle(4, 6, 0, 5);
        //triangleList[5] = new Triangle(5, 1, 0, 6);
        //triangleList[6] = new Triangle(6, 8, 1, 7);
        //triangleList[7] = new Triangle(7, 8, 2, 1);
        //triangleList[8] = new Triangle(8, 9, 2, 8);
        //triangleList[9] = new Triangle(9, 9, 3, 2);
        //triangleList[10] = new Triangle(10, 10, 3, 9);
        //triangleList[11] = new Triangle(11, 10, 4, 3);
        //triangleList[12] = new Triangle(12, 11, 4, 10);
        //triangleList[13] = new Triangle(13, 11, 5, 4);
        //triangleList[14] = new Triangle(14, 12, 5, 11);
        //triangleList[15] = new Triangle(15, 12, 6, 5);
        //triangleList[16] = new Triangle(16, 7, 6, 12);
        //triangleList[17] = new Triangle(17, 7, 1, 6);
        //triangleList[18] = new Triangle(18, 13, 8, 7);
        //triangleList[19] = new Triangle(19, 13, 9, 8);
        //triangleList[20] = new Triangle(20, 13, 10, 9);
        //triangleList[21] = new Triangle(21, 13, 11, 10);
        //triangleList[22] = new Triangle(22, 13, 12, 11);
        //triangleList[23] = new Triangle(23, 13, 7, 12);

        //T4s
        triangleList[0] = new Triangle(0, 3, 0, 1);
        triangleList[1] = new Triangle(1, 6, 0, 3);
        triangleList[2] = new Triangle(2, 7, 0, 5);
        triangleList[3] = new Triangle(3, 10, 0, 7);
        triangleList[4] = new Triangle(4, 11, 0, 9);
        triangleList[5] = new Triangle(5, 2, 0, 11);
        
        //T3s
        triangleList[6] = new Triangle(6, 17, 6, 3);
        triangleList[7] = new Triangle(7, 20, 8, 6);
        triangleList[8] = new Triangle(8, 21, 10, 7);
        triangleList[9] = new Triangle(9, 24, 12, 10);
        triangleList[10] = new Triangle(10, 13, 2, 11);
        triangleList[11] = new Triangle(11, 16, 4, 2);
        
        //T2s
        triangleList[12] = new Triangle(12, 17, 20, 6);
        triangleList[13] = new Triangle(13, 22, 8, 20); //11 second, 4 second, 10
        triangleList[14] = new Triangle(14, 21, 24, 10);
        triangleList[15] = new Triangle(15, 14, 12, 24);
        triangleList[16] = new Triangle(16, 13, 16, 2);
        triangleList[17] = new Triangle(17, 18, 4, 16);
        
        //T1s
        triangleList[18] = new Triangle(18, 25, 20, 17);
        triangleList[19] = new Triangle(19, 23, 20, 25);
        triangleList[20] = new Triangle(20, 25, 24, 21);
        triangleList[21] = new Triangle(21, 15, 24, 25);
        triangleList[22] = new Triangle(22, 25, 16, 13);
        triangleList[23] = new Triangle(23, 19, 16, 25);

        int index = 0;
        foreach (Triangle tri in triangleList)
        {
            triangles[index] = tri.TriangleVerticies[0];
            triangles[index + 1] = tri.TriangleVerticies[1];
            triangles[index + 2] = tri.TriangleVerticies[2];
            index += 3;
        }

        //computeNormals(verticies, normals);

        theMesh.vertices = verticies;
        //theMesh.normals = normals;
        //theMesh.uv = uv;
        theMesh.triangles = triangles;

    }

    void getLineLengths()
    {
        a = (verticies[6] - verticies[5]).magnitude;
        b = (verticies[7] - verticies[6]).magnitude;
        c = (verticies[12] - verticies[7]).magnitude;
        d = (verticies[13] - verticies[7]).magnitude;
        e = (verticies[1] - verticies[0]).magnitude;
        f = (verticies[2] - verticies[0]).magnitude;
    }

    void computeNormals()
    {
        Vector3[] triangleNormals = new Vector3[NUMBER_OF_TRIANGLES];
        for (int i = 0; i < NUMBER_OF_TRIANGLES; i++)
        {
            triangleNormals[i] = faceNormal(triangleList[i].TriangleVerticies[0],
                triangleList[i].TriangleVerticies[1],
                triangleList[i].TriangleVerticies[2]);
        }

        for (int vertexNumber = 0; vertexNumber < NUMBER_OF_VERTICIES; vertexNumber++)
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
            if (vertexNumber != 0 & vertexNumber < 13)
            {
                foreach (Triangle tri in triangleList)
                {
                    if (vertexNumber % 2 == 0)
                    {
                        //primary vertex
                        if (tri.ContainsVertex(vertexNumber - 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                    }
                    else
                    {
                        //secondary verted
                        if (tri.ContainsVertex(vertexNumber + 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                    }
                }
            }
            else if (vertexNumber != 16 && vertexNumber != 20 && vertexNumber != 24 && vertexNumber != 25)
            {
                foreach (Triangle tri in triangleList)
                {
                    if (vertexNumber == 13 || vertexNumber == 17 || vertexNumber == 21)
                    {
                        //primary vertex
                        if (tri.ContainsVertex(vertexNumber + 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                        if (tri.ContainsVertex(vertexNumber + 2))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                    }
                    else if (vertexNumber == 14 || vertexNumber == 18 || vertexNumber == 22)
                    {
                        //secondary verted
                        if (tri.ContainsVertex(vertexNumber + 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                        if (tri.ContainsVertex(vertexNumber - 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                    }
                    else
                    {
                        //tertiary vertex
                        if (tri.ContainsVertex(vertexNumber - 1))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                        if (tri.ContainsVertex(vertexNumber - 2))
                        {
                            trianglesWithThisVertex.Add(tri);
                        }
                    }
                }
            }

            // add normals together
            Vector3 combinedVectors = Vector3.zero;
            foreach (Triangle tri in trianglesWithThisVertex)
            {
                combinedVectors += triangleNormals[tri.TriangleNumber];
            }

            // normalize them and store
            normals[vertexNumber] = combinedVectors.normalized;
        }




    }

    Vector3 faceNormal(int v1, int v2, int v3)
    {
        Vector3 a = verticies[v2] - verticies[v1];
        Vector3 b = verticies[v3] - verticies[v1];
        return Vector3.Cross(a, b).normalized;
    }

    void displayNormals()
    {
        for (int i = 0; i < NUMBER_OF_VERTICIES; i++)
        {
            GameObject line = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            Vector3 v = (verticies[i] + 0.2f * normals[i]) - verticies[i];
            line.transform.localPosition = verticies[i] + v.normalized * v.magnitude;
            line.transform.localScale = new Vector3(0.02f, v.magnitude, 0.02f);
            line.transform.up = v.normalized;
            line.GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    void displayVerties()
    {
        for (int i = 0; i < NUMBER_OF_VERTICIES; i++)
        {
            spheres[i] = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            spheres[i].transform.localPosition = verticies[i];
            spheres[i].transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);
            spheres[i].GetComponent<MeshRenderer>().material.color = Color.black;
            spheres[i].name = "Vertex " + i;
        }
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
