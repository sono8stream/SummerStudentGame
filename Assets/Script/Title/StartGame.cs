using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    SceneChanger changer;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            UserData.instance = UserData.Load();
            changer.OnChangeScene(1);
        }
        if (Input.GetKey(KeyCode.Z))
        {
            changer.OnChangeScene(1);
        }
    }
}