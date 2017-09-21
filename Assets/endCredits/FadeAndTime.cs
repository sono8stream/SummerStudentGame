using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //パネルのイメージを操作するのに必要

//一定時間ごとに暗転を入れ、グローバル変数sceneを増やすスクリプト

public class FadeAndTime : MonoBehaviour {
	public static int scene;

	float fadeSpeed = 0.02f;        //透明度が変わるスピードを管理、スクリプト上でAは0~1間で指定する
	float red, green, blue, alfa;   //パネルの色、不透明度を管理

	public bool isFadeOut = false;  //フェードアウト処理の開始、完了を管理するフラグ
	public bool isFadeIn = false;   //フェードイン処理の開始、完了を管理するフラグ

	Image fadeImage;                //透明度を変更するパネルのイメージ

	public static float time;
	int mid;
	public bool used = false;

	void Start () {
		fadeImage = GetComponent<Image> ();
		red = fadeImage.color.r;
		green = fadeImage.color.g;
		blue = fadeImage.color.b;
		alfa = fadeImage.color.a;

		time = 0;
		mid = 4;
		scene = 1;
	}

	void Update () {
		time += Time.deltaTime;
		if (time > mid && !used && scene != 7) {
			used = true;
			isFadeOut = true;
		}
		if(isFadeIn){
			fadeSpeed = 0.04f;
			StartFadeIn ();
		}

		if (isFadeOut) {
			fadeSpeed = 0.04f;
			StartFadeOut ();
		}
		if (scene == 7) {
			if (time > 0) {	//凝りどころ
				if(Input.GetKey(KeyCode.Space)) Application.LoadLevel ("title");
			}
		}
	}

	void StartFadeIn(){
		alfa -= fadeSpeed;                //a)不透明度を徐々に下げる
		SetAlpha ();                      //b)変更した不透明度パネルに反映する
		if(alfa <= 0){                    //c)完全に透明になったら処理を抜ける
			isFadeIn = false;             
			fadeImage.enabled = false;    //d)パネルの表示をオフにする
			time = 0;
			used = false;
		}
	}

	void StartFadeOut(){
		fadeImage.enabled = true;  // a)パネルの表示をオンにする
		alfa += fadeSpeed;         // b)不透明度を徐々にあげる
		SetAlpha ();               // c)変更した透明度をパネルに反映する
		if(alfa >= 1+0.2){		   // d)完全に不透明になったら処理を抜ける,猶予fを作り良く見せる
			scene++;
			isFadeOut = false;
			isFadeIn = true;
		}

	}

	void SetAlpha(){
		fadeImage.color = new Color(red, green, blue, alfa);
	}
}