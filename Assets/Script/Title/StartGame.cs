using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour {

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            UserData.instance = UserData.Load();
            ChangeScene();
        }
        if (Input.GetKey(KeyCode.Z))
        {
            ChangeScene();
        }
    }

    public void ChangeScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(1);
    }
}
