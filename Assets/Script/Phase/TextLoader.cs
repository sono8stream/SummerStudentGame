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
    PhaseChanger changer;
    [SerializeField]
    Logger logger;
    [SerializeField]
    TextAsset summit, ending, over, limit;
    [SerializeField]
    TextAsset[] randomEvents;

    [SerializeField]
    Text messageText, stamText, reachText, dateText,
        timeText, tempText, weatherText, charaText;
    [SerializeField]
    AudioSource audioSource;
    [SerializeField]
    string message;
    [SerializeField]
    Transform choicesT;
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
    [SerializeField]
    AudioClip[] ses;

    int textLim = 2;//テキスト表示までのウェイト
    int tSetIndex;
    int messageIndex;
    int letterIndex;
    int textCount;
    int fadeLim;
    int fadeCnt;
    Image charaTemp, charaTemp2;
    string charaName, charaName2;
    const string PLAYER_NAME = "ヤシュパル", SISTER_NAME = "アンジュ";

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
    Dictionary<string, AudioClip> seDict;
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
        InitializeBmD();
        InitializeSeD();

        if (1 < UserData.instance.day.value)
        {
            UserData.instance.day.value--;
            EndEvent("[e]");
        }
        else
        {
            InitializeLoadMessage();
        }

        fadeLim = 0;
        fadeCnt = 0;
        charaName = PLAYER_NAME;
        charaName2 = PLAYER_NAME;
    }

    // Update is called once per frame
    void Update()
    {
        if (messageIndex == messageCacheList.Length) { return; }

        HideWin();

        if (messageIndex < messageCacheList.Length
            && messagePred(messageCacheList[messageIndex]))
        {
            do
            {
                messageIndex++;
                //Debug.Log(messageIndex);
                if (0 < messageIndex && messageIndex < messageCacheList.Length)
                {
                    subPred
                        = CheckRegular(messageCacheList[messageIndex - 1], false);
                    messagePred = CheckRegular(messageCacheList[messageIndex]);
                }
                else if (messageIndex == 0)
                {
                    messagePred = CheckRegular(messageCacheList[messageIndex]);
                    break;
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

    Predicate<string> CheckRegular(string s, bool once = true)//正規表現のチェック
    {
        Predicate<string> messagePred = WriteText;
        if (2 < s.Length)
        {
            string sTemp = s.Substring(0, 3);
            if (regExp.ContainsKey(sTemp))
            { messagePred = regExp[sTemp]; }
        }
        if (once && messagePred == WriteText)
        {
            logger.AddText("\r\n" + s);
        }
        return messagePred;
    }

    public void InitializeLoadMessage(bool onNext=false)
    {
        subVar = new int[10];
        messageCacheList = Regex.Split(textSet.text, "\r\n|\r|\n");
        messageText.text = "";
        if(onNext)
        {
            messageIndex = -1;
        }
        else
        {
            messageIndex = 0;
            messagePred = CheckRegular(messageCacheList[messageIndex]);
            Debug.Log("Loaded");
        }
    }

    void InitializeRE()
    {
        regExp = new Dictionary<string, Predicate<string>>();
        regExp.Add("[e]", EndEvent);
        regExp.Add("[z]", CallNextEvent);
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
        regExp.Add("[o]", ChangeBgm);
        regExp.Add("[q]", PlaySE);
        regExp.Add("[x]", SetItemChoices);
        regExp.Add("[u]", UseItem);
        regExp.Add("[y]", ToTitle);
        regExp.Add("[p]", ChangeItemCount);
        regExp.Add("[v]", SaveUserData);
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
        variableDict.Add("気温", UserData.instance.temperature);
        variableDict.Add("天気", UserData.instance.weatherIndex);
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
        backSpDict.Add("光", backSprites[10]);
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
        charaSpDict.Add("男性", charaSprites[0]);
    }

    void InitializeBmD()
    {
        bgmDict = new Dictionary<string, AudioClip>();
        bgmDict.Add("無し", bgms[0]);
        bgmDict.Add("山1", bgms[1]);
        bgmDict.Add("山2", bgms[2]);
        bgmDict.Add("緊張", bgms[3]);
        bgmDict.Add("TrueEnd", bgms[4]);

    }

    void InitializeSeD()
    {
        seDict = new Dictionary<string, AudioClip>();
        seDict.Add("日没", ses[0]);
    }

    #region 文字処理メソッド
    bool EndEvent(string text)//[e]
    {
        if (GetEventSet(false))
        {
            InitializeLoadMessage(true);
        }
        else
        {
            Highlight("[l]f");
            UpdateDay(text.Length < 4);
            changer.ChangeWin(WinName.ComWin);
        }
        UpdateStatus(text);
        return true;
    }

    bool CallNextEvent(string text)//[z] ,コマンド後イベント呼び出し
    {
        UpdateStatus(text);
        if (GetEventSet())
        {
            InitializeLoadMessage(true);
        }
        else
        {
            EndEvent(text);
        }
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
        Item item = null;
        string tex = text.Substring(3);
        string name = tex.Split(':')[0];

        if (variableDict.ContainsKey(name))
        {
            var = variableDict[name];
        }
        else if (UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(name)) != null)//アイテム名
        {
            item = UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(name));
            var = new IntVariable(item.count);
        }
        else if (name[0] == '(')
        {
            var = UserData.instance.flagList[
                int.Parse(name.Substring(1, name.Length - 2))];
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

        if (item != null)
        {
            item.count = var.value < 0 ? 0 : var.value;
        }

        if (UserData.instance.mHp.value < UserData.instance.hp.value)
        {
            UserData.instance.hp.value = UserData.instance.mHp.value;
        }
        else if (UserData.instance.hp.value < 0)
        {
            UserData.instance.hp.value = 0;
        }
        if (UserData.mReach < UserData.instance.reach.value)
        {
            UserData.instance.reach.value = UserData.mReach;
        }
        else if (UserData.instance.reach.value < 0)
        {
            UserData.instance.reach.value = 0;
        }

        return true;
    }

    bool GetVariable(string text)//[g]変数名:indexの形で指定したインデックスに読み込み
    {
        int val = 0;
        string tex = text.Split(']')[1];
        string name = tex.Split(':')[0];

        if (variableDict.ContainsKey(name))
        {
            val = variableDict[name].value;

        }
        else if (UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(name)) != null)//アイテム名
        {
            val = UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(name)).count;
        }
        else if(name[0]=='(')
        {
            val = UserData.instance.flagList[
                int.Parse(name.Substring(1, name.Length - 2))].value;
        }
        else
        {
            return true;
        }

        tex = tex.Split(':')[1];
        int index = int.Parse(tex);

        if (index < 0 || subVar.Length <= index) { return true; }
        subVar[index] = val;

        return true;
    }

    bool UpdateStatus(string text)//[d]
    {
        stamText.text = variableDict["体力"].value.ToString() + "/"
            + variableDict["最大体力"].value.ToString();
        reachText.text = variableDict["到達度"].value.ToString() + "%";
        dateText.text = variableDict["日数"].value.ToString() + "日目";
        //timeText.text = variableDict["時間"].value.ToString() + ":00";
        tempText.text = variableDict["気温"].value.ToString() + "℃";
        weatherText.text
            = Enum.GetName(typeof(WeatherName), variableDict["天気"].value);

        if (UserData.instance.reach.value < 30)
        {
            ChangeBackSprite("[b]入口:0");
            reachText.color = Color.white;
            UpdateBGM(bgmDict["山1"]);
        }
        else if (UserData.instance.reach.value < 70)
        {
            ChangeBackSprite("[b]山中:0");
            reachText.color = new Color(1, 0.9f, 0.7f);
            UpdateBGM(bgmDict["山1"]);
        }
        else
        {
            ChangeBackSprite("[b]山頂:0");
            reachText.color = new Color(1, 0.5f, 0);
            UpdateBGM(bgmDict["山2"]);
        }

        if (UserData.instance.hp.value < 20)
        {
            stamText.color = new Color(1, 0.25f, 0);
        }
        else if (UserData.instance.hp.value < 50)
        {
            stamText.color = new Color(1, 0.5f, 0);
        }
        else
        {
            stamText.color = Color.white;
        }
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
        RectTransform charaNameT
            = charaText.transform.parent.GetComponent<RectTransform>();
        Vector2 pos = new Vector2(-500, 185);

        switch (c)
        {
            case 'l':
                leftChara.color = Color.white;
                rightChara.color = Color.gray;
                charaText.text = charaName;
                charaNameT.localPosition = pos;
                logger.AddText("\r\n\r\n[" + charaName + "]",2);
                break;
            case 'r':
                rightChara.color = Color.white;
                leftChara.color = Color.gray;
                charaText.text = charaName2;
                charaNameT.localPosition = new Vector2(-pos.x, pos.y);
                logger.AddText("\r\n\r\n[" + charaName2 + "]",2);
                break;
            case 'n':
                rightChara.color = Color.gray;
                leftChara.color = Color.gray;
                charaNameT.localPosition = new Vector2(-pos.x + 1000, pos.y);
                logger.AddText("\r\n");
                break;
            case 'f':
                rightChara.color = Color.white;
                leftChara.color = Color.white;
                charaNameT.localPosition = new Vector2(-pos.x + 1000, pos.y);
                logger.AddText("\r\n");
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
            if (fadeLim == 0)
            {
                back1.sprite = back2.sprite;
                back1.color = Color.white;
                back2.color = new Color(1, 1, 1, 0);
                return true;
            }
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
            Debug.Log(elem[1]);
            if (!charaSpDict.ContainsKey(elem[1])) { return true; }
            Debug.Log("HasKey");
            string name;
            switch(elem[1])
            {
                case "主人公":
                case "化け物":
                case "消去":
                    name = PLAYER_NAME;
                    break;
                case "妹":
                    name = SISTER_NAME;
                    break;
                default:
                    name = elem[1];
                    break;
            }
            if (elem[0].Equals("l"))
            {
                charaTemp = leftChara;
                charaTemp2 = leftChara2;
                charaName = name;
            }
            else
            {
                charaTemp = rightChara;
                charaTemp2 = rightChara2;
                charaName2 = name;
            }

            charaTemp2.sprite = charaSpDict[elem[1]];
            AdjustImageSize(charaTemp2);
            fadeLim = int.Parse(elem[2]);
        }
        else if (fadeCnt == fadeLim)
        {
            charaTemp.sprite = charaTemp2.sprite;
            charaTemp.color = Color.white;
            AdjustImageSize(charaTemp);
            charaTemp2.color = new Color(1, 1, 1, 0);
            fadeCnt = 0;
            return true;
        }

        fadeCnt++;
        charaTemp.color -= new Color(0, 0, 0, 1.0f  / fadeLim);
        charaTemp2.color += new Color(0, 0, 0, 1.0f / fadeLim);

        return false;
    }

    bool ChangeBgm(string text)//[o]
    {
        if (!bgmDict.ContainsKey(text.Substring(3)))
        {
            return true;
        }
        audioSource.clip = bgmDict[text.Substring(3)];
        if (audioSource.clip != null)
        {
            audioSource.Play();
        }
        return true;
    }

    bool PlaySE(string text)
    {
        if (!bgmDict.ContainsKey(text.Substring(3)))
        {
            return true;
        }
        audioSource.PlayOneShot(bgmDict[text.Substring(3)]);
        return true;
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
        if (1 < choiceCount)
        {
            messageText.text = 0 < UserData.instance.itemList[0].count
                ? UserData.instance.itemList[0].desTxt + "\r\n(所持数:"
                + UserData.instance.itemList[0].count + ")"
                : "所持していないアイテムです。";
        }
        else
        {
            messageText.text = "行動選択に戻ります。";
        }
        return true;
    }

    bool UseItem(string text)//[u]
    {
        SelectChoice(false);

        if (MoveChoice())
        {
            if (selectIndex < choiceCount - 1)
            {
                messageText.text = 0 < UserData.instance.itemList[selectIndex].count
                    ? UserData.instance.itemList[selectIndex].desTxt + "\r\n(所持数:"
                    + UserData.instance.itemList[selectIndex].count + ")"
                    : "所持していないアイテムです。";
            }
            else
            {
                messageText.text = "行動選択に戻ります。";
            }
        }

        if (Input.GetKeyDown(KeyCode.Space) && (selectIndex == choiceCount - 1
            || 0 < UserData.instance.itemList[selectIndex].count))
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
            messageCacheList
                    = ArrayAdvance.MergeArray(messageCacheList,
                    new string[1] { "[e]戻る" });
            return true;
        }

        SelectChoice(true);
        return false;
    }

    bool ToTitle(string text)//[y]
    {
        int sceneIndex = text.Length <= 3 ? 0 : int.Parse(text.Substring(3));
        if (sceneIndex == 1)
        {
            UserData.instance.InitializeData();
            InitializeVD();
            EndEvent("[e]戻る");
        }
        else
        {
            GameObject.Find("SceneChanger").
                GetComponent<SceneChanger>().OnChangeScene(sceneIndex);
        }
        return true;
    }

    //[p]アイテム名:個数:確率,アイテム名:個数:確率, ...
    bool ChangeItemCount(string text)//[p],確率を指定してランダムに入手、入手テキスト
    {
        string[] opeTxt = text.Substring(3).Split(',');
        int val = UnityEngine.Random.Range(0, 100);
        int co = 0;
        int i = 0;
        Item item = null;
        for (; i < opeTxt.Length; i++)
        {
            co += int.Parse(opeTxt[i].Split(':')[2]);
            if (val < co)
            {
                item = UserData.instance.itemList.First(
                    x => x.name.Equals(opeTxt[i].Split(':')[0]));
                break;
            }
        }
        Debug.Log(item.name);
        if (item == null) { return true; }
        int delta = int.Parse(opeTxt[i].Split(':')[1]);
        if (item.count == 0 && delta < 0) { return true; }
        if (item.count + delta < 0)
        {
            delta = -item.count;
        }
        item.count += delta;
        string txt = item.name + "を" + delta.ToString() + "つ";
        txt += delta < 0 ? "失った。" : "手に入れた！";
        string[] getText = new string[2] { txt, "[r]" };

        messageCacheList = ArrayAdvance.InsertArray(
                messageCacheList, getText, messageIndex + 1);

        return true;
    }

    bool SaveUserData(string text)//[v]
    {
        UserData.Save(UserData.instance);
        return true;
    }
    #endregion

    #region 文字処理補助メソッド
    void UpdateDay(bool nextDay)
    {
        if(nextDay)
        {
            UserData.instance.day.value++;
            if (UnityEngine.Random.value < 0.1f + UserData.instance.reach.value * 0.7f
                + UserData.instance.day.value * 0.1f)
            {
                UserData.instance.weatherIndex.value += UnityEngine.Random.Range(-1, 2);
                if (UserData.instance.weatherIndex.value < 0
                    || 2 < UserData.instance.weatherIndex.value)
                {
                    UserData.instance.weatherIndex.value = 1;
                }
            }

            UserData.instance.temperature.value
                = 30 - (int)(UserData.instance.reach.value * 0.4f
                + UnityEngine.Random.Range(-2 - UserData.instance.weatherIndex.value, 3)
                + UserData.instance.day.value * 0.2f);

            audioSource.PlayOneShot(seDict["日没"]);
        }
    }

    bool GetEventSet(bool onRandom = true)//条件に合う定期イベント呼び出し、無ければランダムイベント
    {
        if (UserData.instance.hp.value <= 0)
        {
            textSet = over;
        }
        else if (UserData.mReach <= UserData.instance.reach.value)
        {
            textSet = textSet == summit ? ending : summit;//頂上、エンディングイベント
        }
        else if(UserData.instance.day.value==50)
        {
            textSet = limit;
        }
        else if (onRandom && UnityEngine.Random.value < 0.45f)
        {
            textSet = randomEvents[
            UnityEngine.Random.Range(0, randomEvents.Length)];
        }
        else { return false; }
        return true;
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
        else if (text[0] == '(')//フラグ変数
        {
            val = UserData.instance.flagList[
                int.Parse(text.Substring(1, text.Length - 2))].value;
        }
        else if (UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(text)) != null)
        {
            val = UserData.instance.itemList.FirstOrDefault(
            x => x.name.Equals(text)).count;
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
                       ? selectIndex + lineChoices * 2 : selectIndex - lineChoices;
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
        choicesT.GetChild(selectIndex).GetComponent<RectTransform>().localScale
            = on ? new Vector3(1.06f, 1.2f) : Vector3.one;
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
        bool on = !Input.GetKey(KeyCode.Z);
        foreach (Transform t in transform)
        {
            t.gameObject.SetActive(on);
        }
        GetComponent<Image>().enabled = on;
    }

    void AdjustImageSize(Image image)
    {
        Sprite s = image.sprite;
        float rate = s.texture.width / (float)s.texture.height;
        Debug.Log(rate);
        float height = image.rectTransform.sizeDelta.y;
        Debug.Log(height);
        image.rectTransform.sizeDelta = new Vector2(height * rate, height);
    }

    void UpdateBGM(AudioClip clip)
    {
        if (audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }
    #endregion
}

public enum VariableNames
{
    日数 = 0, 時間, 現在地
}