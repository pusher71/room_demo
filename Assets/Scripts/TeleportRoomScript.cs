using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportRoomScript : MonoBehaviour
{
    private static bool copyCreated = false; //копия комнаты была создана
    private const int roomLength = 32; //длина комнаты вдоль оси размножения
    public TeleportRoomScript neighbor; //соседняя комната
    public GameObject doorFront; //передняя дверь
    private DoorScript doorFrontScript; //её скрипт
    public GameObject doorBack; //задняя дверь
    private DoorScript doorBackScript; //её скрипт
    private bool closeTriggerActivated = true; //активирован триггер на перемещение
    public float delayAfterClosing; //задержка между закрытием двери и перемещением комнаты

    // Start is called before the first frame update
    void Start()
    {
        doorFrontScript = doorFront.GetComponent<DoorScript>();
        doorBackScript = doorBack.GetComponent<DoorScript>();
        if (!copyCreated)
        {
            //создание копии комнаты
            neighbor = Instantiate(this, transform.position + new Vector3(roomLength, 0, 0), Quaternion.identity);
            neighbor.neighbor = this;

            //задняя дверь открыта изначально только в первой комнате
            doorBackScript.openDoor();

            //деактивация входного триггера происходит только в первой комнате
            closeTriggerActivated = false;

            copyCreated = true;
        }
    }

    //кнопка в комнате нажата
    public void buttonPressed()
    {
        //открыть переднюю дверь текущей комнаты
        doorFrontScript.openDoor();

        //открыть заднюю дверь следующей комнаты
        neighbor.doorBackScript.openDoor();
    }

    public void changeRoom()
    {
        if (closeTriggerActivated) //если триггер активирован
        {
            //деактивировать триггер
            closeTriggerActivated = false;

            //закрыть заднюю дверь текущей комнаты
            doorBackScript.closeDoor();

            //закрыть переднюю дверь предыдущей комнаты
            neighbor.doorFrontScript.closeDoor();

            //подождать и переместить заднюю комнату вперёд
            StartCoroutine(ExampleCoroutine());
        }
    }

    IEnumerator ExampleCoroutine()
    {
        //задержка
        yield return new WaitForSeconds(delayAfterClosing);

        //переместить заднюю комнату вперёд
        Vector3 offset = (transform.position - neighbor.transform.position) * 2; //вектор смещения
        neighbor.transform.position += offset;

        //сбросить её
        neighbor.updateRoom(offset);
    }

    public void updateRoom(Vector3 offset)
    {
        doorFrontScript.updateBorders(offset); //обновление границ передней двери
        doorBackScript.updateBorders(offset); //обновление границ задней двери
        closeTriggerActivated = true; //активация закрывающего триггера
    }
}
