using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class TextLoader : MonoBehaviour
{
    public TextAsset textSet;

    [SerializeField]
    Text messageText, dateText, timeText, weatherText;
    [SerializeField]
    string message;
    [SerializeField]
    Transform choicesT, commandsT;

    int textLim = 2;//テキスト表示までのウェイト
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
    Dictionary<string, IntVariable> variableDict;
    Predicate<string> messagePred;

    UserData uData;

    // Use this for initialization
    void Start()
    {
        messageText.text = "";
        Debug.Log(textSet.text);
        InitializeLoadMessage();

        uData = new UserData();
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
                    case (int)RegularExpressions.イベントエンド:
                        messagePred = EndEvent;
                        break;
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
                    case (int)RegularExpressions.ジャンプ:
                        messagePred = JumpLabel;
                        break;
                    case (int)RegularExpressions.着地点:
                        messagePred = (string t) => { return true; };
                        break;
                    case (int)RegularExpressions.変数呼び出し:
                        messagePred = ChangeVariable;
                        break;
                    case (int)RegularExpressions.日付表示変更:
                        messagePred = UpdateDate;
                        break;
                }
            }
            else//普通にメッセージ表示
            {
                messagePred = WriteText;
            }
        }
    }

    public void InitializeLoadMessage()
    {
        messageCacheList = Regex.Split(textSet.text, "\r\n|\r|\n");
        InitializeRE();
        InitializeVD();
        messageText.transform.parent.gameObject.SetActive(true);
        CheckRegular(messageCacheList[messageIndex]);
    }

    void InitializeRE()
    {
        regularExpression = new Dictionary<string, int>();
        regularExpression.Add("[e]", -1);
        regularExpression.Add("[r]", 0);
        regularExpression.Add("[w]", 1);
        regularExpression.Add("[i]", 2);
        regularExpression.Add("[t]", 3);
        regularExpression.Add("[c]", 4);
        regularExpression.Add("[s]", 5);
        regularExpression.Add("[j]", 6);
        regularExpression.Add("[a]", 7);
        regularExpression.Add("[h]", 8);
        regularExpression.Add("[d]", 9);
    }

    void InitializeVD()//変数辞典,[h]変数名:の形で指定可能
    {
        variableDict = new Dictionary<string, IntVariable>();
        variableDict.Add("日数", new IntVariable(1));
        variableDict.Add("時間", new IntVariable(10));
        variableDict.Add("現在地", new IntVariable(0));
    }

    #region 文字処理メソッド
    bool EndEvent(string text)//[e]
    {
        messageText.transform.parent.gameObject.SetActive(false);
        commandsT.gameObject.SetActive(true);
        return true;
    }

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

        if (letterIndex == text.Length)
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

    bool WaitInitialize(string text)//[r]
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            messageText.text = "";
            return true;
        }
        return false;
    }

    bool Wait(string text)//[w]
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            return true;
        }
        return false;
    }

    bool Initialize(string text)//[i]
    {
        messageText.text = "";
        return true;
    }

    bool ChangeSpeed(string text)//[t]数値で文字速度変更, 大きいほど遅い(def=2)
    {
        string t = text.Substring(3);
        textLim = int.Parse(t);
        Debug.Log(textLim);
        return true;
    }

    bool AddChoice(string text)//[c]
    {
        if (6 <= choiceCount) { return true; }

        choiceCount++;
        Transform t = choicesT.GetChild(choiceCount - 1);
        t.gameObject.SetActive(true);
        t.FindChild("Text").GetComponent<Text>().text = text.Substring(3);
        return true;
    }

    bool SetChoices(string text)//[s]
    {
        SelectChoice(false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = selectIndex < choiceCount - 1 ? selectIndex + 1 : 0;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow)
            && selectIndex + lineChoices < choiceCount)
        {
            selectIndex = lineChoices <= selectIndex || choiceCount <= lineChoices
                ? selectIndex % lineChoices : selectIndex + lineChoices;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = 0 < selectIndex ? selectIndex - 1 : choiceCount - 1;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpIndex(choicesT.GetChild(selectIndex).GetChild(0)
                .GetComponent<Text>().text);

            foreach (Transform t in choicesT)
            {
                t.gameObject.SetActive(false);
            }
            selectIndex = 0;
            choiceCount = 0;
            return true;
        }

        SelectChoice(true);
        return false;
    }

    bool JumpLabel(string text)//[j]
    {
        JumpIndex(text.Substring(3));
        return true;
    }

    bool ChangeVariable(string text)//[h] 変数名:の形で指定可能
    {
        IntVariable var;
        string tex = text.Split(']')[1];
        string name = tex.Split(':')[0];

        if (variableDict.ContainsKey(name))
        {
            var = variableDict[name];
        }
        else
        {
            return true;
        }

        tex = tex.Split(':')[1];
        char itr = tex[0];
        int value = int.Parse(tex.Substring(1));

        switch (itr)
        {
            case '+':
                var.value += value;
                break;
            case '-':
                var.value -= value;
                break;
            case '*':
                var.value *= value;
                break;
            case '/':
                var.value /= value;
                break;
            case '=':
                var.value = value;
                break;
        }
        return true;
    }

    bool UpdateDate(string text)//[d]
    {
        dateText.text = variableDict["日数"].value.ToString() + "日目";
        timeText.text = variableDict["時間"].value.ToString() + ":00";
        return true;
    }
    #endregion

    void SelectChoice(bool on)
    {
        Color c = on ? Color.red : Color.white;
        choicesT.GetChild(selectIndex).GetComponent<Image>().color = c;
    }

    void JumpIndex(string text)//特定のラベルまでジャンプします,[j]
    {
        string t = "[a]" + text;
        for (int i = messageIndex; i < messageCacheList.Length; i++)
        {
            if (t.Equals(messageCacheList[i]))
            {
                messageIndex = i - 1;
                return;
            }
        }
    }
}

public enum RegularExpressions
{
    イベントエンド = -1,
    キー待ち文字初期化, キー待ち, 文字初期化, 速度変更, 選択肢追加, 選択肢待ち,
    ジャンプ, 着地点, 変数呼び出し, 日付表示変更
}

public enum VariableNames
{
    日数 = 0, 時間, 現在地
}
