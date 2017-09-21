using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/// <summary>
/// インスタンスを生成すれば、Update()で呼び出してwait
/// </summary>
public class Waiter
{
    int lim, count;
    public int Limit { get { return lim; } set { lim = value; } }
    public int Count { get { return count; } }

    public Waiter(int lim, bool max = false)
    {
        this.lim = lim;
        Initialize(max);
    }

    public bool Wait(bool canCount = true)
    {
        if (canCount)
        {
            count = count < lim ? count + 1 : lim;
        }
        return count == lim;
    }

    public void Initialize(bool max = false) { count = max ? lim : 0; }
}
