using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsFixer : MonoBehaviour
{
    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
}
