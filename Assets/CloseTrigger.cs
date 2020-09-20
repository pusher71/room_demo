using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //сменить комнаты при задевании входного триггера
        transform.parent.gameObject.GetComponent<TeleportRoomScript>().changeRoom();
    }
}
