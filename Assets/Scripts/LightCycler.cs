using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCycler : MonoBehaviour
{
    public bool startOpposite; //начать движение в противоположную сторону
    public Vector3 amplitude; //амплитуда движения света
    [Range(0f, 10f)] public float speed; //скорость движения света
    private Vector3 start; //изначальная позиция света по
    private Vector3 target; //целевая позиция, к которой двигается свет
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
        target = start;
        if (startOpposite) target -= amplitude;
        else target += amplitude;
    }

    // Update is called once per frame
    void Update()
    {
        //движение света
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * speed);

        //проверка на достижение света своей цели
        if (transform.position == target)
            target -= (target - start) * 2;
    }
}
