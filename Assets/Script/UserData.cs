using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UserData
{
    public IntVariable day, hour;
    public IntVariable hp, mHp;
    public IntVariable reach;//到達度
    public const int mReach = 100;
    public IntVariable karman;//カルマ
    public IntVariable caste;//カースト
    public IntVariable temperature;//山頂に行くほど減少
    public IntVariable bodyTemp;//体温
    public IntVariable weatherIndex;//段階を追って変化
    public List<Item> itemList;//アイテム、個数
    public List<IntVariable> flagList;
    public const int flags = 12;

    public static UserData instance = new UserData();

    private UserData()
    {
        mHp = new IntVariable(100);
        hp = new IntVariable(mHp.value);

        InitializeData();
    }

    public void InitializeData()
    {
        day = new IntVariable(1);
        hour = new IntVariable(9);
        reach = new IntVariable(0);
        karman = new IntVariable(30);//初期値30
        caste = new IntVariable((int)CasteName.アチュート);
        temperature = new IntVariable(30);
        weatherIndex = new IntVariable(Random.Range(0, 3));//段階を追って変化

        InitializeItem();
        flagList = new List<IntVariable>();
        for (int i = 0; i < flags; i++)
        { flagList.Add(new IntVariable(0)); }
    }

    void InitializeItem()
    {
        itemList = new List<Item>();
        itemList.Add(new RecovItem(10, hp, "カルダモン",
            "チャイの香り付けによく使われる。\r\nスタミナを10回復"));
        itemList.Add(new RecovItem(30, hp, "ジンジャー",
            "健胃、保温、解熱、消炎、沈吐など多くの薬効。\r\nスタミナを30回復"));
        itemList.Add(new RecovItem(5, mHp, "ガーリック",
            "疲労回復、体力増強、滋養強壮の効果あり。\r\nスタミナ最大値を5上昇"));
        itemList.Add(new RecovItem(800, hp, "フェヌグリーク",
            "滋養強壮、栄養補給、食欲増進、解熱の効果あり。\r\nスタミナを全回復"));
        itemList.Add(new KeyItem(10, "経典群I巻",
            "何かに使えそうだ。"));
        itemList.Add(new KeyItem(10, "経典群II巻",
            "何かに使えそうだ。"));
        itemList.Add(new KeyItem(10, "ヨガ教本",
            "何かに使えそうだ。"));
        itemList.Add(new KeyItem(10, "寺院の証",
            "ヒマラヤの寺院に来た証。\r\nヒンドゥー教徒に認められるようになる。"));
    }
}

public enum CasteName
{
    アチュート = 0, シュードラ = 10, ヴァイシャ = 20,
    クシャトリヤ = 30, ブラフマン = 40
}

public enum ItemName
{
    カルダモン,ジンジャー, ガーリック, フェヌグリーク, 経典群I巻,
    経典群II巻, ヨガ教本, 寺院の証
}

public enum WeatherName
{
    晴れ = 0, 曇り, 雨
}

public enum FlagName
{
    休む2回目
}
