using System.Linq;
using UnityEngine;

public class PhaseChanger : MonoBehaviour
{
    [SerializeField]
    Transform[] winTs;

    public Transform prevT;

    private void Start()
    {
        ChangeWin(WinName.TextWin);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C)
            && GetActiveTransform() != winTs[(int)WinName.LogWin])
        {
            ChangeWin(WinName.LogWin);
        }
    }

    public void ChangeWin(WinName changeName,bool on=true)
    {
        prevT = GetActiveTransform();
        foreach(Transform t in winTs)
        {
            t.gameObject.SetActive(false);
        }
        winTs[(int)changeName].gameObject.SetActive(on);
    }

    public Transform GetActiveTransform()
    {
        return winTs.FirstOrDefault(x => x.gameObject.activeSelf);
    }
}

public enum WinName { TextWin, ComWin, LogWin }
