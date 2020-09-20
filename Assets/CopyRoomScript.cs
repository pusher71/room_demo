using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyRoomScript : MonoBehaviour
{
    private static int roomCopiesCount = 2; //количество копий комнат, идущих подряд
    private const int roomLength = 32; //длина комнаты вдоль оси размножения
    // Start is called before the first frame update
    void Start()
    {
        if (roomCopiesCount > 0)
        {
            //спаун новой комнаты - копия текущего объекта this
            Instantiate(this, transform.position + new Vector3(roomLength, 0, 0), Quaternion.identity);
            roomCopiesCount--;
        }
    }
}
