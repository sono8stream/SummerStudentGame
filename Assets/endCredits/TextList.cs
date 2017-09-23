using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//FadeAndTimeのsceneの値に基づき、テキストの文字を変えるスクリプト

public class TextList : MonoBehaviour {
	public Text text;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 center = new Vector3 (0, 0, 0);
		Vector3 pl = new Vector3 (-480, 0, 0);
		Vector3 pr = new Vector3 (480, 0, 0);

		switch (FadeAndTime.scene) {
		case 1:
			transform.localPosition = center;
			text.text = "~ ニンブパラク ~\nींबू की परीक्षा\n\n制作:\n\n";
			break;
		case 2:
			transform.localPosition = pr;
			text.text = "メインプログラマ\n\nsono";
			break;
		case 3:
			transform.localPosition = pl;
			text.text = "プログラマー\n\nakito";
			break;
		case 4:
			transform.localPosition = pr;
			text.text = "グラフィック\nデザイナー\n\neleven";
			break;
		case 5:
			transform.localPosition = pl;
			text.text = "サウンド\nクリエイター\n\nfffff";
			break;
		case 6:
			transform.localPosition = pr;
			text.text = "シナリオライター\n\nmappy";
			break;
		case 7:
			transform.localPosition = pl;
			text.text = "THENK YOU\nFOR\nPLAYING.\n\n\n  Press space key.";
			break;
		}
	}
}
