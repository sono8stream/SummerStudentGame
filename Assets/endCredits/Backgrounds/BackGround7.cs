using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//FadeAndTimeのsceneの値に基づき、画像を変更するスクリプト

public class BackGround7 : MonoBehaviour {
	public static int scene;

	Image back;
	float red,green,blue,alfa;

	// Use this for initialization
	void Start () {
		back = GetComponent<Image> ();
		red = back.color.r;
		green = back.color.g;
		blue = back.color.b;
		alfa = back.color.a;
	}

	// Update is called once per frame
	void Update () {
		switch (FadeAndTime.scene) {
		case 7:
			alfa = 1;
			back.color = new Color(red, green, blue, alfa);
			break;
		default:
			alfa = 0;
			back.color = new Color(red, green, blue, alfa);
			break;
		}
	}
}
