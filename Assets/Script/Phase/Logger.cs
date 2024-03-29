﻿using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class Logger : MonoBehaviour
{
    [SerializeField]
    PhaseChanger changer;
    [SerializeField]
    Text logTxt;
    [SerializeField]
    string log;
    [SerializeField]
    Text upMark, downMark;

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
            }
            else if (Input.GetKey(KeyCode.DownArrow))
            {
                logRectT.anchoredPosition += Vector2.up * speed;
            }
            LimitScroll();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            changer.prevT.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void AddText(string txt,int lines=1)
    {
        string nLiner = "\r\n";
        lineCnt += lines;
        log += txt;
        if (LINE_LENGTH < lineCnt)
        {
            int delCnt = lineCnt - LINE_LENGTH;
            //Debug.Log(delCnt);
            for (int i = 0; i < delCnt; i++)
            {
                log = log.Substring(nLiner.Length + log.IndexOf(nLiner));
            }
            lineCnt = LINE_LENGTH;
        }
        logTxt.text = log;
        //Debug.Log(lineCnt);
    }

    bool LimitScroll()
    {
        if (logRectT.anchoredPosition.y < maskHeight - lineCnt * lPerLine)
        {
            logRectT.anchoredPosition
                = Vector2.up * (maskHeight - lineCnt * lPerLine);
            upMark.gameObject.SetActive(false);
            return true;
        }
        else if (0 < logRectT.anchoredPosition.y)
        {
            logRectT.anchoredPosition = Vector2.zero;
            downMark.gameObject.SetActive(false);
            return true;
        }

        upMark.gameObject.SetActive(true);
        downMark.gameObject.SetActive(true);
        if (logRectT.anchoredPosition.y == maskHeight - lineCnt * lPerLine)
        {
            upMark.gameObject.SetActive(false);
        }
        else if (logRectT.anchoredPosition.y == 0)
        {
            downMark.gameObject.SetActive(false);
        }
        return false;
    }
}
