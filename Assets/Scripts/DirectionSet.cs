using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//набор возможных направлений в ячейке помещения
public class DirectionSet
{
    public List<Vector3> dirs = new List<Vector3>(); //список направлений
    private int dirCall = -1; //индекс направления на вызов
    public float heightAdjustment; //регулировка высоты
    public bool callDone = false; //вызов выполнен

    //установить направление
    public void setCallDirection(int index)
    {
        dirCall = index;
    }

    //указано ли направление
    public bool isCallSpecified()
    {
        return dirCall != -1;
    }

    //получить направление
    public Vector3 getCallDirection()
    {
        return dirs[dirCall];
    }

    //получить случайное направление кроме заданного
    public Vector3 randomOtherSpecified(Vector3 specified)
    {
        Vector3 dir;
        do dir = dirs[Random.Range(0, dirs.Count)];
        while (dir == specified);
        return dir;
    }
}

//тип направления
public enum DirectionType { bottom, top, call }
