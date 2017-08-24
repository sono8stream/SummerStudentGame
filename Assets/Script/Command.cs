using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour {

    [SerializeField]
    Transform commandsT;
    [SerializeField]
    TextLoader loader;
    [SerializeField]
    TextAsset[] eventAssets;
    [SerializeField]
    Text dscText;
    [SerializeField]
    string[] dscArray;//コマンドの説明文

    const int commandCount = 6;
    const int lineCommands = 3;

    int selectIndex;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MoveCommand();
    }

    void MoveCommand()
    {
        SelectChoice(false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = selectIndex < commandCount - 1 ? selectIndex + 1 : 0;
            dscText.text = dscArray[selectIndex];
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex = lineCommands <= selectIndex
                ? selectIndex % lineCommands : selectIndex + lineCommands;
            dscText.text = dscArray[selectIndex];
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = 0 < selectIndex ? selectIndex - 1 : commandCount - 1;
            dscText.text = dscArray[selectIndex];
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            /*foreach (Transform t in commandsT)
            {
                t.gameObject.SetActive(false);
            }*/
            CommandClick(selectIndex);
            selectIndex = 0;
            gameObject.SetActive(false);
        }

        SelectChoice(true);
    }
    
    void SelectChoice(bool on)
    {
        Color c = on ? Color.red : Color.white;
        commandsT.GetChild(selectIndex).GetComponent<Image>().color = c;
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
