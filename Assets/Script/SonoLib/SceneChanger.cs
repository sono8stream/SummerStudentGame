using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    bool on;
    int sceneIndex;
    Waiter fadeWaiter, stateWaiter;
    Image[] fadeImages;

    private void Awake()
    {
        fadeWaiter = new Waiter(10);
        stateWaiter = new Waiter(3);
        fadeImages = transform.GetComponentsInChildren<Image>();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (on)
        {
            if (ChangeScene())
            {
                Debug.Log("Done");
                on = false;
                stateWaiter.Initialize();
                fadeWaiter.Initialize();
            }
            Debug.Log(stateWaiter.Count);
            Debug.Log(fadeWaiter.Count);
        }
    }

    public void OnChangeScene(int sceneIndex)
    {
        on = true;
        this.sceneIndex = sceneIndex;
        stateWaiter.Initialize();
        fadeWaiter.Initialize();
    }

    public bool ChangeScene()
    {
        switch (stateWaiter.Count)
        {
            case 0:
                if (Fade(false))
                {
                    stateWaiter.Wait();
                }
                break;
            case 1:
                SceneManager.LoadScene(sceneIndex);
                fadeWaiter.Initialize();
                stateWaiter.Wait();
                break;
            case 2:
                if (Fade(true))
                {
                    stateWaiter.Wait();
                }
                break;
        }
        return stateWaiter.Wait(false);
    }

    public bool Fade(bool off)
    {
        fadeWaiter.Wait();
        float alpha = off ? 1 - fadeWaiter.Count / (float)fadeWaiter.Limit
            : fadeWaiter.Count / (float)fadeWaiter.Limit;
        foreach(Image i in fadeImages)
        {
            i.color= new Color(1, 1, 1, alpha);
        }
        return fadeWaiter.Wait(false);
    }
}