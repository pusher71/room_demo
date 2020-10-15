using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Vector3 moveDirection; //дистанция открытия двери по-вертикали
    [Range(0f, 10f)] public float speed; //скорость открытия двери

    private Vector3 openPosition; //положение при открытой двери
    private Vector3 closePosition; //положение при закрыой двери
    private bool doOpening; //нужно ли открывтаься
    private bool doClosing; //нужно ли закрываться
    private bool fullyOpened; //дверь полностью открыта
    private bool fullyClosed; //дверь полностью закрыта

    // Start is called before the first frame update
    void Start()
    {
        openPosition = transform.position + moveDirection;
        closePosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //возможное открытие двери
        if (doOpening && !fullyOpened)
            transform.position = Vector3.MoveTowards(transform.position, openPosition, Time.deltaTime * speed);

        //возможное закрытие двери
        if (doClosing && !fullyClosed)
            transform.position = Vector3.MoveTowards(transform.position, closePosition, Time.deltaTime * speed);

        //проверка на нахождение двери в крайних позициях
        if (fullyOpened = transform.position == openPosition)
            doOpening = false;
        if (fullyClosed = transform.position == closePosition)
            doClosing = false;
    }

    //открыть дверь
    public void openDoor()
    {
        doOpening = true;
    }

    //закрыть дверь
    public void closeDoor()
    {
        doClosing = true;
    }

    //сдвинуть границы двери
    public void updateBorders(Vector3 offset)
    {
        openPosition += offset;
        closePosition += offset;
    }
}
