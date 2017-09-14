[System.Serializable]
public class Item
{
    protected int value;//効果値
    public int count;//個数
    public string name, desTxt;//説明文
    public string[] useTxt;

    public Item(int val,string nm, string deT)
    {
        value = val;
        name = nm;
        desTxt = deT;
        count = 0;
    }

    public virtual bool Effect()//アイテム使用時の効果、戻り値trueなら消費
    { return false; }
}
