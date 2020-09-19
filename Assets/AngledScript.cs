using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngledScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Mesh mesh = GetComponent<Mesh>();
        MeshFilter mf = GetComponent<MeshFilter>();
        Mesh mesh = mf.mesh;

        //изменение массива...
        Vector3[] v = mesh.vertices;
        //удаление вертикальной поверхности
        for (int i = 0; i < 4; i++)
            v[i] = v[0];

        //наклон верхней поверхности к низу
        v[8].y = v[9].y = -0.5f;

        mesh.vertices = v;

        //вывод массива
        for (int i = 0; i < mesh.vertices.Length; i++)
            Debug.Log(mesh.vertices[i]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
