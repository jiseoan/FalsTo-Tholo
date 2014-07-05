using UnityEngine;
using System.Collections;

public class LanguageScript : MonoBehaviour {

	public GameObject uiFade;
	public GameObject uiBackPanel;

	public GameObject uiKoreanBtn;
	public GameObject uiEnglishBtn;
	public GameObject uiChinaBtn;
	public GameObject uiJapanBtn;

	public GameObject uiKoreanDummy;
	public GameObject uiEnglishDummy;
	public GameObject uiChinaDummy;
	public GameObject uiJapanDummy;

	private bool bLoaded = false;
	// Use this for initialization
	void Start () {
		bLoaded = false;
		DataManager.Instance.currentScene = 4;
		uiFade.SetActive( true );
		TweenAlpha []tas = uiFade.GetComponents<TweenAlpha>();
		foreach( TweenAlpha ta in tas )
		{
			if( ta.tweenGroup == 0 )
				ta.PlayForward();
		}

		if (Screen.height == 1050)
		{
			if( uiBackPanel != null )
			{
				UIPanel panel = uiBackPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1680.0f, 1050f );
			}
		} else if( Screen.height == 1080 )
		{
			if( uiBackPanel != null )
			{
				UIPanel panel = uiBackPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080.0f );
				
				panel.gameObject.transform.localPosition = new Vector3( 840.0f + 60.0f, 0.0f, 0.0f );
			}
		}

		if (DataManager.Instance.GetLanguage () == 0) {
			uiKoreanBtn.SetActive( false );
			uiKoreanDummy.SetActive( true );
		} else if( DataManager.Instance.GetLanguage () == 1) {
			uiEnglishBtn.SetActive( false );
			uiEnglishDummy.SetActive( true );
		} else if( DataManager.Instance.GetLanguage () == 2) {
			uiJapanBtn.SetActive( false );
			uiJapanDummy.SetActive( true );
		} else if( DataManager.Instance.GetLanguage () == 3) {
			uiChinaBtn.SetActive( false );
			uiChinaDummy.SetActive( true );
		}



		bLoaded = true;
	}
	
	// Update is called once per frame
	void Update () {
		if( bLoaded )
		{
			if( DataManager.Instance.reloading )
			{
				DataManager.Instance.currentScene = 0;
				Application.LoadLevel( "1_Loading" );
			}
		}
	}

	public void ButtonKorean()
	{
		DataManager.Instance.ChangeLanguage( "Korean" );
		FadeOut();
	}

	public void ButtonEnglish()
	{
		DataManager.Instance.ChangeLanguage( "English" );
		FadeOut();
	}

	public void ButtonJapan()
	{
		DataManager.Instance.ChangeLanguage( "Japan" );
		FadeOut();
	}

	public void ButtonChina()
	{
		DataManager.Instance.ChangeLanguage( "China" );
		FadeOut();
	}

	public void FadeOut()
	{
		TweenAlpha []tas = uiFade.GetComponents<TweenAlpha>();
		foreach( TweenAlpha ta in tas )
		{
			if( ta.tweenGroup == 1 )
				ta.PlayForward();
		}
	}

	public void LoadScene()
	{
		if( DataManager.Instance.nextScene == 2 )
			Application.LoadLevel( "2_List" );
		else if( DataManager.Instance.nextScene == 3 )
			Application.LoadLevel( "3_Detail" );
	}
}
