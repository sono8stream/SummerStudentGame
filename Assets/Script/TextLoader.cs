﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    [SerializeField]
    TextAsset textSet;
    [SerializeField]
    Text messageText, dateText, weatherText;
    [SerializeField]
    string message;
    [SerializeField]
    Transform choicesT;

    int textLim = 3;//テキスト表示までのウェイト
    int tSetIndex;
    int messageIndex;
    int letterIndex;
    int textCount;

    int lineChoices = 3;
    int selectIndex;//選択中選択肢番号
    int choiceCount;

    [SerializeField]
    string[] messageCacheList;
    Dictionary<string, int> regularExpression;
    Predicate<string> messagePred;

    // Use this for initialization
    void Start()
    {
        messageText.text = "";
        Debug.Log(textSet.text);
        messageCacheList = Regex.Split(textSet.text, "\r\n|\r|\n");
        InitializeRE();
        CheckRegular(messageCacheList[messageIndex]);
    }

    // Update is called once per frame
    void Update()
    {
        if (messageCacheList.Length == messageIndex) { return; }

        if (messagePred(messageCacheList[messageIndex]))
        {
            messageIndex++;
            if (messageIndex < messageCacheList.Length)
            {
                CheckRegular(messageCacheList[messageIndex]);
            }
        }
    }

    string LoadTextLine()
    {
        string textLine = "";
        int endCount = 0;
        messageText.text = textSet.text;
        List<string> textList = new List<string>();
        //messageText.cachedTextGenerator.GetLines(textList);
        //textasset
        while (textSet.text[tSetIndex] != '\n' || endCount != 3)
        {
            //textLine=
        }
        return textLine;
    }

    void CheckRegular(string s)//正規表現のチェック
    {
        if (s.Length <= 2)
        {
            messagePred = WriteText;
            return;
        }
        else
        {
            string sTemp = s.Substring(0, 3);
            if (regularExpression.ContainsKey(sTemp))
            {
                switch (regularExpression[sTemp])
                {
                    case (int)RegularExpressions.キー待ち文字初期化:
                        messagePred = WaitInitialize;
                        break;
                    case (int)RegularExpressions.キー待ち:
                        messagePred = Wait;
                        break;
                    case (int)RegularExpressions.文字初期化:
                        messagePred = Initialize;
                        break;
                    case (int)RegularExpressions.速度変更:
                        messagePred = ChangeSpeed;
                        break;
                    case (int)RegularExpressions.選択肢追加:
                        messagePred = AddChoice;
                        break;
                    case (int)RegularExpressions.選択肢待ち:
                        messagePred = SetChoices;
                        break;
                    case (int)RegularExpressions.着地点:
                        messagePred = (string t) => { return true; };
                        break;
                }
            }
            else//普通にメッセージ表示
            {
                messagePred = WriteText;
            }
        }
    }

    void InitializeRE()
    {
        regularExpression = new Dictionary<string, int>();
        regularExpression.Add("[r]", 0);
        regularExpression.Add("[w]", 1);
        regularExpression.Add("[i]", 2);
        regularExpression.Add("[t]", 3);
        regularExpression.Add("[c]", 4);
        regularExpression.Add("[s]", 5);
        regularExpression.Add("[a]", 6);
    }

    #region 文字処理メソッド
    bool WriteText(string text)
    {
        if (textLim <= textCount)
        {
            messageText.text += text[letterIndex];
            letterIndex++;
            textCount = 0;
        }
        else
        {
            textCount++;
        }

        if(letterIndex == text.Length)
        {
            letterIndex = 0;
            messageText.text += "\r\n";
            return true;
        }
        else
        {
            return false;
        }
    }

    bool WaitInitialize(string text)
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            messageText.text = "";
            return true;
        }
        return false;
    }

    bool Wait(string text)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    bool Initialize(string text)
    {
        messageText.text = "";
        return true;
    }

    bool ChangeSpeed(string text)//文字速度変更
    {
        string t = text.Substring(3);
        textLim = int.Parse(t);
        Debug.Log(textLim);
        return true;
    }

    bool AddChoice(string text)
    {
        if (6 <= choiceCount) { return true; }

        choiceCount++;
        Transform t = choicesT.GetChild(choiceCount - 1);
        t.gameObject.SetActive(true);
        t.FindChild("Text").GetComponent<Text>().text = text.Substring(3);
        return true;
    }

    bool SetChoices(string text)
    {
        SelectChoice(false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = selectIndex < choiceCount - 1 ? selectIndex + 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            && selectIndex + lineChoices < choiceCount)
        {
            selectIndex = lineChoices <= selectIndex
                ? selectIndex - lineChoices : selectIndex + lineChoices;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = 0 < selectIndex ? selectIndex - 1 : choiceCount - 1;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            foreach(Transform t in choicesT)
            {
                t.gameObject.SetActive(false);
            }
            choiceCount = 0;
            return true;
        }

        SelectChoice(true);
        return false;
    }
    #endregion

    void SelectChoice(bool on)
    {
        Color c = on ? Color.red : Color.white;
        choicesT.GetChild(selectIndex).GetComponent<Image>().color = c;
    }
}

public enum RegularExpressions
{
    キー待ち文字初期化,キー待ち,文字初期化,速度変更,選択肢追加,選択肢待ち,
    着地点
}
