using System;

public class RecovItem : Item
{
    IntVariable tar;

    public RecovItem(int val,IntVariable tg, string nm, string deT) 
        : base(val, nm, deT)
    {
        tar = tg;
        useTxt = new string[8]{name + "を使用しました。","[w]",
            "力がみなぎる！","[w]",
            "アイテムはなくなった。","[r]","[d]","[e]" };
    }

    public override bool Effect()
    {
        tar.value += value;
        if (UserData.instance.mHp.value < UserData.instance.hp.value)
        {
            UserData.instance.hp.value = UserData.instance.mHp.value;
        }
        return true;//消費
    }
}
