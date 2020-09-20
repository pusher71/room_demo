using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRoomScript : MonoBehaviour
{
    private static bool copyCreated = false; //копия комнаты была создана
    private const int roomLength = 32; //длина комнаты вдоль оси размножения
    public TeleportRoomScript roomPartner; //соседняя комната
    public GameObject door; //дверь в комнате
    public DoorScript ds; //её скрипт
    private bool closeTriggerActivated = true; //активирован триггер на перемещение

    // Start is called before the first frame update
    void Start()
    {
        ds = door.GetComponent<DoorScript>();
        if (!copyCreated)
        {
            //создание копии комнаты
            roomPartner = Instantiate(this, transform.position + new Vector3(roomLength, 0, 0), Quaternion.identity);
            roomPartner.roomPartner = this;

            //деактивация входного триггера происходит только в первой комнате
            closeTriggerActivated = false;

            copyCreated = true;
        }
    }

    public void changeRoom()
    {
        if (closeTriggerActivated) //если триггер активирован
        {
            //деактивировать триггер
            closeTriggerActivated = false;

            //переместить заднюю комнату вперёд
            Vector3 offset = (transform.position - roomPartner.transform.position) * 2; //вектор смещения
            roomPartner.transform.position += offset;

            //сбросить её
            roomPartner.closeTriggerActivated = true; //активация закрывающего триггера
            roomPartner.ds.updateBorders(offset); //обновление границ двери
            roomPartner.ds.closeDoor(); //закрытие двери
        }
    }
}
