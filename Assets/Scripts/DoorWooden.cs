using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWooden : MonoBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fc; //игровой управленец
    //public float speedNormal; //обычная скорость
    //public float speedSlow; //медленная скорость

    private bool over = false; //прицел на двери
    private bool opened = false; //открыта ли?
    private float proc = 0; //процесс открытия
    private float baseProc = 0; //относительное открытие

    private bool isSlow; //режим медленного перетаскивания
    private float rootPos; //позиция нажатой ПКМ

    // Update is called once per frame
    void Update()
    {
        //привести дверь в движение
        transform.localRotation = Quaternion.Slerp(transform.localRotation, Quaternion.Euler(0, proc, 0), Time.deltaTime);
        opened = proc > 0;

        if (over) //мышка захвачена?
        {
            if (Input.GetMouseButtonDown(0)) //обычное открытие
            {
                proc = opened ? 0 : 90;
                baseProc = proc;
            }
            if (Input.GetMouseButtonDown(1)) //медленное открытие
            {
                rootPos = Camera.main.transform.rotation.eulerAngles.x;
                isSlow = true;
            }
        }

        //движение фазы
        if (isSlow)
            proc = Mathf.Clamp(baseProc + Camera.main.transform.rotation.eulerAngles.x - rootPos, 0, 90);

        if (Input.GetMouseButtonUp(1)) //медленное прикрытие
        {
            isSlow = false;
            baseProc = proc;
        }
    }

    private void OnMouseOver()
    {
        over = true;
    }

    private void OnMouseExit()
    {
        over = false;
    }
}
