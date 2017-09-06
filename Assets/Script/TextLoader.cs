﻿using System;
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
    TextAsset summit, ending;
    [SerializeField]
    TextAsset[] randomEvents;

    [SerializeField]
    Text messageText, stamText, reachText, dateText, timeText, weatherText;
    [SerializeField]
    string message;
    [SerializeField]
    Transform choicesT, commandsT;
    [SerializeField]
    Image back1, back2;//背景画像
    [SerializeField]
    Image leftChara, leftChara2;//キャラ画像1
    [SerializeField]
    Image rightChara, rightChara2;//キャラ画像2
    [SerializeField]
    Sprite[] backSprites, charaSprites;
    [SerializeField]
    AudioClip[] bgms;

    int textLim = 2;//テキスト表示までのウェイト
    int tSetIndex;
    int messageIndex;
    int letterIndex;
    int textCount;
    int fadeLim;
    int fadeCnt;
    Image charaTemp, charaTemp2;

    int lineChoices = 3;
    int selectIndex;//選択中選択肢番号
    int choiceCount;
    
    [SerializeField]
    string[] messageCacheList;
    Dictionary<string, Predicate<string>> regExp;
    Dictionary<string, IntVariable> variableDict;
    Dictionary<string, Sprite> backSpDict;
    Dictionary<string, Sprite> charaSpDict;
    Dictionary<string, AudioClip> bgmDict;
    Predicate<string> messagePred, subPred;

    int[] subVar;//予備変数

    // Use this for initialization
    void Start()
    {
        messageText.text = "";
        InitializeRE();
        InitializeVD();
        InitializeBD();
        InitializeCD();
        InitializeLoadMessage();

        fadeLim = 0;
        fadeCnt = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(messageIndex == messageCacheList.Length) { return; }

        HideWin();

        if (messageIndex < messageCacheList.Length
            && messagePred(messageCacheList[messageIndex]))
        {
            do
            {
                messageIndex++;
                if (messageIndex < messageCacheList.Length)
                {
                    subPred = CheckRegular(messageCacheList[messageIndex - 1]);
                    messagePred = CheckRegular(messageCacheList[messageIndex]);

                }
                else
                {
                    break;
                }
            }
            while (subPred == WriteText
            && messagePred == WriteText
            && messagePred(messageCacheList[messageIndex]));
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

    Predicate<string> CheckRegular(string s)//正規表現のチェック
    {
        Predicate<string> messagePred = WriteText;
        if (2 < s.Length)
        {
            string sTemp = s.Substring(0, 3);
            if (regExp.ContainsKey(sTemp))
            { messagePred = regExp[sTemp]; }
        }
        return messagePred;
    }

    public void InitializeLoadMessage(bool onTextSet=true)
    {
        subVar = new int[10];
        if (onTextSet)
        {
            messageCacheList = Regex.Split(textSet.text, "\r\n|\r|\n");
        }
        messageText.transform.parent.gameObject.SetActive(true);
        messageText.text = "";
        messageIndex = 0;
        messagePred = CheckRegular(messageCacheList[messageIndex]);
    }

    void InitializeRE()
    {
        regExp = new Dictionary<string, Predicate<string>>();
        regExp.Add("[e]", EndEvent);
        regExp.Add("[r]", WaitInitialize);
        regExp.Add("[w]", Wait);
        regExp.Add("[i]", Initialize);
        regExp.Add("[t]", ChangeSpeed);
        regExp.Add("[c]", AddChoice);
        regExp.Add("[s]", SetChoices);
        regExp.Add("[j]", JumpLabel);
        regExp.Add("[a]", (string t) => { return true; });
        regExp.Add("[h]", ChangeVariable);
        regExp.Add("[g]", GetVariable);
        regExp.Add("[d]", UpdateStatus);
        regExp.Add("[n]", GetRandom);
        regExp.Add("[l]", Highlight);
        regExp.Add("[f]", CheckFlag);
        regExp.Add("[b]", ChangeBackSprite);
        regExp.Add("[m]", ChangeCharaSprite);
        regExp.Add("[x]", SetItemChoices);
        regExp.Add("[u]", UseItem);
    }

    void InitializeVD()//変数辞典,[h]変数名:の形で指定可能
    {
        variableDict = new Dictionary<string, IntVariable>();
        variableDict.Add("体力", UserData.instance.hp);
        variableDict.Add("最大体力", UserData.instance.mHp);
        variableDict.Add("到達度", UserData.instance.reach);
        variableDict.Add("カルマ", UserData.instance.karman);
        variableDict.Add("カースト", UserData.instance.caste);
        variableDict.Add("日数", UserData.instance.day);
        variableDict.Add("時間", UserData.instance.hour);
    }

    void InitializeBD()//背景画像辞典
    {
        backSpDict = new Dictionary<string, Sprite>();
        backSpDict.Add("消去", backSprites[0]);
        backSpDict.Add("実家内", backSprites[1]);
        backSpDict.Add("村", backSprites[2]);
        backSpDict.Add("入口", backSprites[3]);
        backSpDict.Add("山中", backSprites[4]);
        backSpDict.Add("山頂", backSprites[5]);
        backSpDict.Add("宴", backSprites[6]);
        backSpDict.Add("寺院", backSprites[7]);
        backSpDict.Add("夜の森", backSprites[8]);
        backSpDict.Add("夜の川", backSprites[9]);
    }

    void InitializeCD()//キャラ画像辞典
    {
        charaSpDict = new Dictionary<string, Sprite>();
        charaSpDict.Add("消去", charaSprites[0]);
        charaSpDict.Add("主人公", charaSprites[1]);
        charaSpDict.Add("長", charaSprites[2]);
        charaSpDict.Add("妹", charaSprites[3]);
        charaSpDict.Add("山賊", charaSprites[4]);
        charaSpDict.Add("登山者", charaSprites[5]);
        charaSpDict.Add("ヒンドゥー教徒", charaSprites[6]);
        charaSpDict.Add("レモン教徒", charaSprites[7]);
        charaSpDict.Add("女神", charaSprites[8]);
        charaSpDict.Add("化け物", charaSprites[9]);
    }

    #region 文字処理メソッド
    bool EndEvent(string text)//[e]
    {
        Highlight("[l]f");
        messageText.transform.parent.gameObject.SetActive(false);
        commandsT.gameObject.SetActive(true);
        return true;
    }

    bool CallNextEvent()//[] ,コマンド後イベント呼び出し
    {
        GetEventSet();
        InitializeLoadMessage();
        return true;
    }

    bool WriteText(string text)
    {
        if (Input.GetKeyDown(KeyCode.Space)||Input.GetKey(KeyCode.X))
        {
            while (!WriteChar(text)) { }
            textCount = 0;
            return true;
        }
        else
        {
            if (textCount == textLim)
            {
                textCount = 0;
                if (!WriteVariable(text))//変数呼び出し
                {
                    messageText.text += text[letterIndex];
                    letterIndex++;
                }
                if (letterIndex == text.Length)
                {
                    letterIndex = 0;
                    messageText.text += "\r\n";
                    return true;
                }
            }
            else
            {
                textCount++;
            }
        }
        return false;
    }

    bool WaitInitialize(string text)//[r]
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            (5 <= textCount && Input.GetKey(KeyCode.X)))
        {
            messageText.text = "";
            textCount = 0;
            return true;
        }

        if (textCount == 0)
        {
            messageText.text
                = messageText.text.Substring(0, messageText.text.Length - 2);
            textCount++;
        }
        else if (textCount == 1)
        {
            messageText.text += "▼";
            textCount++;
        }
        else if (textCount == textLim * 25)
        {
            messageText.text
                = messageText.text.Substring(0, messageText.text.Length - 1);
            textCount++;
        }
        else if (textCount == textLim * 45)
        {
            messageText.text += "▼";
            textCount = textLim * 5;
        }
        else
        {
            textCount++;
        }
        return false;
    }

    bool Wait(string text)//[w]
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            (5 <= textCount && Input.GetKey(KeyCode.X)))
        {
            textCount = 0;
            if (messageText.text[messageText.text.Length - 1] == '▼')
            {
                messageText.text
                    = messageText.text.Substring(0, messageText.text.Length - 1);
            }
            messageText.text += "\r\n";
            return true;
        }

        if (textCount == 0)
        {
            messageText.text
                = messageText.text.Substring(0, messageText.text.Length - 2);
            textCount++;
        }
        else if (textCount == 1)
        {
            messageText.text += "▼";
            textCount++;
        }
        else if (textCount == textLim * 25)
        {
            messageText.text
                = messageText.text.Substring(0, messageText.text.Length - 1);
            textCount++;
        }
        else if (textCount == textLim * 45)
        {
            messageText.text += "▼";
            textCount = textLim * 5;
        }
        else
        {
            textCount++;
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
        if (9 <= choiceCount) { return true; }

        Transform t = choicesT.GetChild(choiceCount);
        t.gameObject.SetActive(true);
        t.FindChild("Text").GetComponent<Text>().text = text.Substring(3);
        choiceCount++;
        return true;
    }

    bool SetChoices(string text)//[s]
    {
        SelectChoice(false);

        MoveChoice();

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
        string tex = text.Substring(3);
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

        if(UserData.instance.mHp.value<UserData.instance.hp.value)
        {
            UserData.instance.hp.value = UserData.instance.mHp.value;
        }
        else if (UserData.instance.hp.value<0)
        {
            UserData.instance.hp.value = 0;
        }

        return true;
    }

    bool GetVariable(string text)//[g]変数名:indexの形で指定したインデックスに読み込み
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
        int index = int.Parse(tex.Substring(1));

        if (index < 0 || subVar.Length <= index) { return true; }
        subVar[index] = var.value;

        return true;
    }

    bool UpdateStatus(string text)//[d]
    {
        stamText.text = variableDict["体力"].value.ToString() + "/"
            + variableDict["最大体力"].value.ToString();
        reachText.text = variableDict["到達度"].value.ToString() + "%";
        dateText.text = variableDict["日数"].value.ToString() + "日目";
        timeText.text = variableDict["時間"].value.ToString() + ":00";
        return true;
    }

    bool GetRandom(string text)//[n]indexの形で指定したインデックスに乱数取得0~100
    {
        string t = text.Split(']')[1];
        int index = int.Parse(t);

        if (index < 0 || subVar.Length <= index) { return true; }
        subVar[index] = UnityEngine.Random.Range(0, 100);
        return true;
    }
    
    bool Highlight(string text)//[l], キャラハイライト
    {
        char c = text.Split(']')[1][0];
        switch(c)
        {
            case 'l':
                leftChara.color = Color.white;
                rightChara.color = Color.gray;
                break;
            case 'r':
                rightChara.color = Color.white;
                leftChara.color = Color.gray;
                break;
            case 'n':
                rightChara.color = Color.gray;
                leftChara.color = Color.gray;
                break;
            case 'f':
                rightChara.color = Color.white;
                leftChara.color = Color.white;
                break;
        }
        return true;
    }

    bool CheckFlag(string text)//[f], 分岐終点
    {
        string[] elem = text.Substring(3).Split(':');
        int val1 = TextToVariable(elem[0]);
        int val2 = TextToVariable(elem[2]);
        string itr = elem[1];

        bool flag = false;
        switch (itr)
        {
            case "<":
                flag = val1 < val2;
                break;
            case ">":
                flag = val1 > val2;
                break;
            case "=":
                flag = val1 == val2;
                break;
            case "<=":
                flag = val1 <= val2;
                break;
            case ">=":
                flag = val1 >= val2;
                break;
        }

        if (!flag)
        {
            string t = "分岐終点";
            if (elem.Length == 4) { t += elem[3]; }
            JumpIndex(t);
        }
        Debug.Log(val1);
        return true;
    }

    bool ChangeBackSprite(string text)//[b]画像名:時間, 背景画像変更,フェードあり
    {
        if (fadeCnt == 0)
        {
            string[] elem = text.Substring(3).Split(':');
            if (!backSpDict.ContainsKey(elem[0])) { return true; }
            back2.sprite = backSpDict[elem[0]];
            fadeLim = int.Parse(elem[1]);
        }
        else if (fadeCnt == fadeLim)
        {
            back1.sprite = back2.sprite;
            back1.color = Color.white;
            back2.color = new Color(1, 1, 1, 0);
            fadeCnt = 0;
            return true;
        }

        fadeCnt++;
        back1.color -= new Color(0, 0, 0, 1.0f / fadeLim);
        back2.color += new Color(0, 0, 0, 1.0f / fadeLim);
        return false;
    }

    bool ChangeCharaSprite(string text)//[m]左右:画像名:時間,キャラ画像変更,フェードあり
    {
        if (fadeCnt == 0)
        {
            string[] elem = text.Substring(3).Split(':');
            if (!charaSpDict.ContainsKey(elem[1])) { return true; }
            if (elem[0].Equals("l"))
            {
                charaTemp = leftChara;
                charaTemp2 = leftChara2;
            }
            else
            {
                charaTemp = rightChara;
                charaTemp2 = rightChara2;
            }

            charaTemp2.sprite = charaSpDict[elem[1]];
            fadeLim = int.Parse(elem[2]);
        }
        else if (fadeCnt == fadeLim)
        {
            charaTemp.sprite = charaTemp2.sprite;
            charaTemp.color = Color.white;
            charaTemp2.color = new Color(1, 1, 1, 0);
            fadeCnt = 0;
            return true;
        }

        fadeCnt++;
        charaTemp.color -= new Color(0, 0, 0, 1.0f  / fadeLim);
        charaTemp2.color += new Color(0, 0, 0, 1.0f / fadeLim);

        return false;
    }

    bool SetItemChoices(string text)//[x]
    {
        Transform t;
        for (choiceCount = 0;
            choiceCount < UserData.instance.itemList.Count; choiceCount++)
        {
                t = choicesT.GetChild(choiceCount);
                t.gameObject.SetActive(true);
                t.FindChild("Text").GetComponent<Text>().text
                    = 0 < UserData.instance.itemList[choiceCount].count
                    ? UserData.instance.itemList[choiceCount].name
                    : "??";
        }
        t = choicesT.GetChild(choiceCount);
        t.gameObject.SetActive(true);
        t.FindChild("Text").GetComponent<Text>().text = "戻る→";
        choiceCount++;
        messageText.text = UserData.instance.itemList[0].desTxt;
        return true;
    }

    bool UseItem(string text)//[u]
    {
        SelectChoice(false);

        if (MoveChoice())
        {
            messageText.text = selectIndex < choiceCount - 1
                ? UserData.instance.itemList[selectIndex].desTxt
                : "行動選択に戻ります。";
        }

        if (Input.GetKeyDown(KeyCode.Space)
            && 0 < UserData.instance.itemList[selectIndex].count)
        {
            int selTemp = selectIndex;
            int choTemp = choiceCount;
            foreach (Transform t in choicesT)
            {
                t.gameObject.SetActive(false);
            }
            selectIndex = 0;
            choiceCount = 0;

            if (selTemp < choTemp - 1)
            {
                if (UserData.instance.itemList[selTemp].Effect())
                { UserData.instance.itemList[selTemp].count--; }
                messageCacheList
                    = ArrayAdvance.MergeArray(messageCacheList,
                    UserData.instance.itemList[selTemp].useTxt);
            }
            else
            {
                EndEvent("");
            }
            return true;
        }

        SelectChoice(true);
        return false;
    }
    #endregion

    #region 文字処理補助メソッド
    void GetEventSet()//条件に合う定期イベント呼び出し、無ければランダムイベント
    {
        if (UserData.mReach <= UserData.instance.reach.value)
        {
            textSet = textSet == summit ? ending : summit;//頂上、エンディングイベント
        }
        else
        {
            textSet=randomEvents[
            UnityEngine.Random.Range(0, randomEvents.Length - 1)];
        }
    }

    bool WriteChar(string text)
    {
        if (!WriteVariable(text))//変数呼び出し
        {
            messageText.text += text[letterIndex];
            letterIndex++;
        }
        if (letterIndex == text.Length)
        {
            letterIndex = 0;
            messageText.text += "\r\n";
            return true;
        }
        return false;
    }

    bool WriteVariable(string text)
    {
        if (text[letterIndex].Equals('[')
            && letterIndex < text.Length - 1)//変数呼び出し
        {
            string t = text.Substring(letterIndex + 1).Split(']')[0];
            if (variableDict.ContainsKey(t))
            {
                messageText.text += variableDict[t].value.ToString();
                letterIndex += t.Length + 2;
                return true;
            }
            else if (t.Length == 1 && 0 <= t[0] - '0' && t[0] - '0' <= 9)//予備変数
            {
                messageText.text += subVar[t[0] - '0'].ToString();
                letterIndex += 3;
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    int TextToVariable(string text)
    {
        int val;

        if (variableDict.ContainsKey(text))
        {
            val = variableDict[text].value;
        }
        else if (text[0] == '[' && 0 <= text[1] - '0' && text[1] - '0' <= 9)//予備変数
        {
            val = subVar[text[1] - '0'];
        }
        else
        {
            val = int.Parse(text);
        }
        return val;
    }

    bool MoveChoice()
    {
        int slTemp = selectIndex;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            selectIndex = selectIndex < choiceCount - 1 ? selectIndex + 1 : 0;
        }
        if (lineChoices <= choiceCount)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                selectIndex = choiceCount <= selectIndex + lineChoices
                    ? selectIndex % lineChoices : selectIndex + lineChoices;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                selectIndex = selectIndex < lineChoices
                       ? selectIndex % lineChoices : selectIndex - lineChoices;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            selectIndex = 0 < selectIndex ? selectIndex - 1 : choiceCount - 1;
        }
        return slTemp != selectIndex;
    }

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

    void HideWin()
    {
        messageText.transform.parent.gameObject.SetActive(!Input.GetKey(KeyCode.Z));
    }
    #endregion
}

public enum VariableNames
{
    日数 = 0, 時間, 現在地
}