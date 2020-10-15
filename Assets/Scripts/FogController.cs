using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FogController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Scene scene = gameObject.scene;
        //Camera camera = Camera.main;
        //RenderSettings.fog = false;
        RenderSettings.fogColor = Color.red;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
