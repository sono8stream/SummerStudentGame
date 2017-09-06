using System;

public class KeyItem : Item
{
    public KeyItem(int val, string nm, string deT) : base(val, nm, deT)
    {
        useTxt = new string[7]{name + "を使用しようとしました。","[w]",
            "...。","[w]","使い方がわからなかった！","[r]","[e]" };
    }

    public override bool Effect()
    {
        return false;
    }
}
