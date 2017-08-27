public class UserData
{
    public IntVariable day, hour;
    public IntVariable hp, mHp;
    public IntVariable reach, mReach;//到達度
    public IntVariable karman;//カルマ
    public IntVariable caste;//カースト
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
        karman = new IntVariable(30);
        caste = new IntVariable((int)CasteName.アチュート);
        temperature = new IntVariable(20);
    }
}

public enum CasteName
{
    アチュート = 0, シュードラ = 10, ヴァイシャ = 20,
    クシャトリヤ = 30, ブラフマン = 40
}
