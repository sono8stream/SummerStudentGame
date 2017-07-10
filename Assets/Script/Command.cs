using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Command : MonoBehaviour {

    [SerializeField]
    Transform commandsT;

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
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            selectIndex = lineCommands <= selectIndex
                ? selectIndex % lineCommands : selectIndex + lineCommands;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = 0 < selectIndex ? selectIndex - 1 : commandCount - 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (Transform t in commandsT)
            {
                t.gameObject.SetActive(false);
            }
            selectIndex = 0;
        }

        SelectChoice(true);
    }
    
    void SelectChoice(bool on)
    {
        Color c = on ? Color.red : Color.white;
        commandsT.GetChild(selectIndex).GetComponent<Image>().color = c;
    }
}
