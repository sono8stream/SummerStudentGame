using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int day, hour;
    public int hp, mHp;

    // Use this for initialization
    void Start()
    {
        day = 1;
        hour = 9;
        mHp = 100;
        hp = mHp;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
