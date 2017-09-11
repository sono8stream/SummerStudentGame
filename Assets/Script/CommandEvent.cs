using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandEvent : MonoBehaviour
{
    TextLoader loader;

    [SerializeField]
    TextAsset[] eventAssets;

    // Use this for initialization
    void Start()
    {
        loader = GameObject.Find("Canvas").GetComponent<TextLoader>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CommandClick(int no)
    {
        CallTextMessage(eventAssets[no]);
    }

    void CallTextMessage(TextAsset tAsset)
    {
        loader.textSet = tAsset;
        loader.InitializeLoadMessage();
    }
}