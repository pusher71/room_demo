using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject door; //дверь, которая открывается этой кнопкой

    //когда прицел находится на кнопке
    private void OnMouseOver()
    {
        //если нажата левая кнопка мыши
        if (Input.GetMouseButtonDown(0))
        {
            //открыть дверь
            door.GetComponent<DoorScript>().openDoor();
        }
    }
}
