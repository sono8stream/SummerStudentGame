using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Logger : MonoBehaviour
{
    [SerializeField]
    PhaseChanger changer;
    [SerializeField]
    Text logTxt;
    [SerializeField]
    string log;
    const int LINE_LENGTH = 50;
    const int WIN_LINES = 13;
    int lineCnt;

    // Use this for initialization
    void Start()
    {
        log = "";
        logTxt.text = log;
        lineCnt = 0;
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            changer.prevT.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void AddText(string txt)
    {
        string nLiner = "\r\n";
        lineCnt += Regex.Split(txt, nLiner).Length;
        log += txt;
        if (LINE_LENGTH < lineCnt)
        {
            int delCnt = lineCnt - LINE_LENGTH;
            for (int i = 0; i < delCnt; i++)
            {
                log = log.Substring(nLiner.Length + log.IndexOf(nLiner));
            }
        }
        logTxt.text = log;
    }
}
