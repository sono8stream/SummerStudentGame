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

    RectTransform logRectT;
    const int LINE_LENGTH = 50;
    const int WIN_LINES = 13;
    int lineCnt;
    int lPerLine;
    int speed = 10;
    float maskHeight;

    // Use this for initialization
    void Awake()
    {
        log = "";
        logTxt.text = log;
        lineCnt = 0;
        Debug.Log("start");
        logRectT = logTxt.GetComponent<RectTransform>();
        lPerLine = (int)logRectT.sizeDelta.y / LINE_LENGTH;
        maskHeight
            = logTxt.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        Debug.Log("maskHeight:" + maskHeight.ToString());
        Debug.Log(maskHeight - lineCnt * lPerLine);
    }

    // Update is called once per frame
    void Update()
    {
        if (WIN_LINES < lineCnt)
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                logRectT.anchoredPosition += Vector2.down * speed;
                LimitScroll(true);
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                logRectT.anchoredPosition += Vector2.up * speed;
                LimitScroll(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            changer.prevT.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void AddText(string txt)
    {
        string nLiner = "\r\n";
        //lineCnt += Regex.Split(txt, nLiner).Length;
        lineCnt++;
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
        Debug.Log(lineCnt);
    }

    bool LimitScroll(bool up)
    {
        if (up && logRectT.anchoredPosition.y < maskHeight - lineCnt * lPerLine)
        {
            logRectT.anchoredPosition
                = Vector2.up * (maskHeight - lineCnt * lPerLine);
        }
        else if (0 < logRectT.anchoredPosition.y)
        {
            logRectT.anchoredPosition = Vector2.zero;
        }
        return true;
    }
}
