using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    //когда прицел находится на кнопке
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0)) //если нажата левая кнопка мыши
            transform.parent.GetComponent<TeleportRoomScript>().buttonPressed(); //оповестить комнату об этом
    }
}
