using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneToggler : MonoBehaviour
{
    public bool isLoading; //будет ли загружать сцена
    public bool isUnloading; //будет ли выгружаться сцена
    public int sceneLoadIndex; //индекс загружаемой сцены
    public int sceneUnloadIndex; //индекс выгружаемой сцены

    public void changeScenes()
    {
        if (isLoading) SceneManager.LoadScene(sceneLoadIndex, LoadSceneMode.Additive);
        if (isUnloading) SceneManager.UnloadSceneAsync(sceneUnloadIndex);
    }

    public void OnTriggerEnter(Collider other)
    {
        changeScenes();
        Destroy(gameObject); //удаление триггера
    }
}
