[System.Serializable]
public class KeyItem : Item
{
    public KeyItem(int val, string nm, string deT) : base(val, nm, deT)
    {
        useTxt = new string[6]{name + "を使用しようとしました。","[w]",
            "...。","[w]","使い方がわからなかった！","[r]"};
    }

    public override bool Effect()
    {
        return false;
    }
}
