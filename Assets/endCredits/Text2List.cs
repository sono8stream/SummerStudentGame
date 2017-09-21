using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要

//一定時間ごとに暗転を入れ、グローバル変数sceneを増やすスクリプト

public class Text2List : MonoBehaviour {

	float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理、スクリプト上でAは0~1間で指定する
	float red, green, blue, alfa;   //パネルの色、不透明度を管理

	public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

	Image fadeImage;                //透明度を変更するパネルのイメージ
	public Text text;

	void Start () {
		fadeImage = GetComponent<Image> ();
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;
	}

	void Update () {
		if (FadeAndTime.scene == 7 && FadeAndTime.time > 3) {
			text.text = "Press space key.";
			isFadeIn = true;
		}
		if(isFadeIn){
			fadeSpeed = 0.04f;
			StartFadeIn ();
		}
	}

	void StartFadeIn(){	//FadeAndTimeのものとは異なる(逆)
		fadeImage.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
		SetAlpha ();               // c)変更した透明度をパネルに反映する
		if(alfa >= 1+0.2){		   // d)完全に不透明になったら処理を抜ける,猶予fを作り良く見せる
			isFadeIn = false;
		}
	}
	void SetAlpha(){
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}