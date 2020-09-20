using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycler : MonoBehaviour
{
    public float amplitude; //амплитуда движения света
    [Range(0f, 10f)] public float speed; //скорость движения света
    private float startZ; //изначальная координата света по Z
    private Vector3 target; //целевая позиция, к которой двигается свет
    // Start is called before the first frame update
    void Start()
    {
        target = transform.position;
        startZ = target.z;
        target.z += amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        //движение света
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

        //проверка на достижение света своей цели
        if (transform.position == target)
            target.z -= (transform.position.z - startZ) * 2;
    }
}
