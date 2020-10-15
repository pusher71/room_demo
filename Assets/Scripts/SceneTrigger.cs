using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<SceneToggler>().changeScenes();
        Destroy(gameObject); //удаление триггера
    }
}
