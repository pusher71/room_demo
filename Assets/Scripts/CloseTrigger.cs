using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTrigger : MonoBehaviour
{
    //когда игрок входит в комнату
    private void OnTriggerEnter(Collider other)
    {
        //сменить комнаты при задевании входного триггера
        transform.parent.gameObject.GetComponent<TeleportRoomScript>().changeRoom();
    }
}
