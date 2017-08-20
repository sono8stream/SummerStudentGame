using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public IntVariable day, hour;
    public IntVariable hp, mHp;
    public IntVariable reach, mReach;//到達度
    public IntVariable temperature;

    public static UserData instance = new UserData();

    private UserData()
    {
        day = new IntVariable(1);
        hour = new IntVariable(9);
        mHp = new IntVariable(100);
        hp = new IntVariable(mHp.value);
        reach = new IntVariable(0);
        mReach = new IntVariable(100);
        temperature = new IntVariable(20);
    }
}
