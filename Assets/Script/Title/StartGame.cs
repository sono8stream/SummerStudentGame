using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    SceneChanger changer;

    GameObject begin, contin;
    bool onContin;
    bool hasContinuation;
    bool onSelect;
    UserData dat;
    
    bool onTransition;
    float downLim = -400;//iniPos=-300
    float sp = 10;

    // Use this for initialization
    void Start()
    {
        dat = UserData.Load();
        if (dat != null)
        {
            hasContinuation = true;
            UserData.instance = dat;
            begin = transform.Find("Begin").gameObject;
            contin = transform.Find("Continue").gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (onTransition)
        {
            GetComponent<RectTransform>().anchoredPosition += Vector2.up * downLim;
            GetComponent<RectTransform>().anchoredPosition /= 2;
            GetComponent<Image>().color -= new Color(0, 0, 0, 0.2f);
            begin.GetComponent<Image>().color += new Color(0, 0, 0, 0.2f);
            contin.GetComponent<Image>().color += new Color(0, 0, 0, 0.2f);

            if (GetComponent<Image>().color.a <= 0)
            {
                onTransition = false;
                onSelect = true;
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (hasContinuation&&!onSelect)
            {
                onContin = true;
                onTransition = true;

                transform.FindChild("Text").gameObject.SetActive(false);
                begin.transform.FindChild("Text").gameObject.SetActive(true);
                contin.transform.FindChild("Text").gameObject.SetActive(true);
            }
            else if(onContin)
            {
                UserData.instance = dat;
                changer.OnChangeScene(1);
            }
            else
            {
                UserData.instance = new UserData();
                changer.OnChangeScene(1);
            }
        }

        if (onSelect)
        {
            SelectChoice(false);
            if (Input.GetKeyDown(KeyCode.LeftArrow) 
                || Input.GetKeyDown(KeyCode.RightArrow))
            {
                onContin = !onContin;
            }
            SelectChoice(true);
        }
    }

    void SelectChoice(bool on)
    {
        Color c = on ? Color.red : Color.white;
        GameObject g = onContin ? contin : begin;
        g.GetComponent<Image>().color = c;
        g.GetComponent<RectTransform>().localScale
            = on ? new Vector3(1.2f, 1.2f) : Vector3.one;
    }
}