using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorWooden : MonoBehaviour
{
    public UnityStandardAssets.Characters.FirstPerson.FirstPersonController fc; //игровой управленец
    public GameObject doorOrigin; //дверные петли
    public float speedNormal; //обычная скорость
    public float speedSlow; //медленная скорость

    private bool over = false; //прицел на двери
    private bool opened = false; //открыта ли?
    private float velocity = 0; //текущая скорость движения
    private float proc = 0; //процесс открытия

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //привести дверь в движение
        proc = Mathf.Clamp(proc + velocity, 0, 90);
        doorOrigin.transform.localRotation = Quaternion.Euler(0, proc, 0);

        if (over) //мышка захвачена?
        {
            if (Input.GetMouseButtonDown(0)) //обычное открытие
            {
                if (!opened) velocity = speedNormal;
                else velocity = -speedNormal;
                opened = !opened;
            }
            if (Input.GetMouseButtonDown(1)) //медленное открытие
            {
                fc.enabled = false;
                opened = true;
                velocity = speedSlow;
            }
            if (Input.GetMouseButtonUp(1)) //медленное прикрытие
            {
                fc.enabled = true;
                opened = false;
                velocity = -speedSlow;
            }
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
