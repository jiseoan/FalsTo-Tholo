using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ListManager : MonoBehaviour {

	public GameObject rootUI = null;
	public GameObject backUI = null;
	public GameObject uiFade;
	public GameObject listItem;

	public GameObject uiButtonKR;
	public GameObject uiButtonEN;

	public GameObject uiPageLeft;
	public GameObject uiPageRight;
	public GameObject uiPagePrf;
	public GameObject uiPageUI;

	public GameObject searchWidget;
	private ArrayList searchButtons = new ArrayList();
	//private ArrayList searchButtonsClick = new ArrayList();
	public GameObject searchButton;
	//public GameObject searchButtonSel;

	public GameObject catWidget;
	private ArrayList catButtons = new ArrayList();
	public GameObject catButton;
	public GameObject catButtonAll;

	public GameObject searchTitle;
	public GameObject catTitle;

	public Font [] fontList;

	private EventDelegate [] buttonClick = new EventDelegate[9];
	private EventDelegate [] buttonSearch = new EventDelegate[27];
	private GameObject [] itemList = new GameObject[9];
	private Texture2D []  imageTexture = new Texture2D[9];

	private EventDelegate [] pageClick = new EventDelegate[10];

	private EventDelegate [] catClick = new EventDelegate[10];

	private ArrayList pageButtons = new ArrayList();

	private bool bLoaded = false;

	private int language;
	private int searchIdx;
	// Use this for initialization
	void Start () {
		bLoaded = false;
		DataManager.Instance.currentScene = 1;
		uiFade.SetActive( true );

		if (Screen.height == 1050)
		{
			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1050;
				
				UIPanel panel = backUI.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1680.0f, 1050f );
			}
		} else if( Screen.height == 1080 )
		{
			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1080;
				
				UIPanel panel = backUI.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080.0f );

				panel.gameObject.transform.localPosition = new Vector3( 840.0f + 60.0f, 0.0f, 0.0f );

			}
		}

		buttonClick[0] = new EventDelegate( ItemClick0 );
		buttonClick[1] = new EventDelegate( ItemClick1 );
		buttonClick[2] = new EventDelegate( ItemClick2 );
		buttonClick[3] = new EventDelegate( ItemClick3 );
		buttonClick[4] = new EventDelegate( ItemClick4 );
		buttonClick[5] = new EventDelegate( ItemClick5 );
		buttonClick[6] = new EventDelegate( ItemClick6 );
		buttonClick[7] = new EventDelegate( ItemClick7 );
		buttonClick[8] = new EventDelegate( ItemClick8 );

		buttonSearch[0] = new EventDelegate( SearchClick0 );
		buttonSearch[1] = new EventDelegate( SearchClick1 );
		buttonSearch[2] = new EventDelegate( SearchClick2 );
		buttonSearch[3] = new EventDelegate( SearchClick3 );
		buttonSearch[4] = new EventDelegate( SearchClick4 );
		buttonSearch[5] = new EventDelegate( SearchClick5 );
		buttonSearch[6] = new EventDelegate( SearchClick6 );
		buttonSearch[7] = new EventDelegate( SearchClick7 );
		buttonSearch[8] = new EventDelegate( SearchClick8 );
		buttonSearch[9] = new EventDelegate( SearchClick9 );
		buttonSearch[10] = new EventDelegate( SearchClick10 );
		buttonSearch[11] = new EventDelegate( SearchClick11 );
		buttonSearch[12] = new EventDelegate( SearchClick12 );
		buttonSearch[13] = new EventDelegate( SearchClick13 );
		buttonSearch[14] = new EventDelegate( SearchClick14 );
		buttonSearch[15] = new EventDelegate( SearchClick15 );
		buttonSearch[16] = new EventDelegate( SearchClick16 );
		buttonSearch[17] = new EventDelegate( SearchClick17 );
		buttonSearch[18] = new EventDelegate( SearchClick18 );
		buttonSearch[19] = new EventDelegate( SearchClick19 );
		buttonSearch[20] = new EventDelegate( SearchClick20 );
		buttonSearch[21] = new EventDelegate( SearchClick21 );
		buttonSearch[22] = new EventDelegate( SearchClick22 );
		buttonSearch[23] = new EventDelegate( SearchClick23 );
		buttonSearch[24] = new EventDelegate( SearchClick24 );
		buttonSearch[25] = new EventDelegate( SearchClick25 );
		buttonSearch[26] = new EventDelegate( SearchClick26 );

		pageClick[0] = new EventDelegate( buttonPage0 );
		pageClick[1] = new EventDelegate( buttonPage1 );
		pageClick[2] = new EventDelegate( buttonPage2 );
		pageClick[3] = new EventDelegate( buttonPage3 );
		pageClick[4] = new EventDelegate( buttonPage4 );
		pageClick[5] = new EventDelegate( buttonPage5 );
		pageClick[6] = new EventDelegate( buttonPage6 );
		pageClick[7] = new EventDelegate( buttonPage7 );
		pageClick[8] = new EventDelegate( buttonPage8 );
		pageClick[9] = new EventDelegate( buttonPage9 );

		catClick[0] = new EventDelegate( buttonCat0 );
		catClick[1] = new EventDelegate( buttonCat1 );
		catClick[2] = new EventDelegate( buttonCat2 );
		catClick[3] = new EventDelegate( buttonCat3 );
		catClick[4] = new EventDelegate( buttonCat4 );
		catClick[5] = new EventDelegate( buttonCat5 );
		catClick[6] = new EventDelegate( buttonCat6 );
		catClick[7] = new EventDelegate( buttonCat7 );
		catClick[8] = new EventDelegate( buttonCat8 );
		catClick[9] = new EventDelegate( buttonCat9 );

		for (int i = 0; i < 9; i++) 
		{
			imageTexture[i] = null;
		}

		for( int i = 0; i < 9; i++ )
		{
			GameObject item = (GameObject)Instantiate( listItem );
			item.transform.parent = gameObject.transform;
			item.transform.localPosition = new Vector3( 460 * ( i % 3 ), -221 * ( i /3 ), 0 );
			item.transform.localRotation = Quaternion.identity;
			item.transform.localScale = Vector3.one;
			UIButton button = item.GetComponent<UIButton>();
			//UIEventListener.Get(item).onClick += this.buttonClick[i];
			//button.onClick +=  buttonClick[i];
			button.onClick.Add( buttonClick[i] );
			itemList[i] = item;
		}

		language = -1;

		if( DataManager.Instance.currentLanguage == "Korean" )
			buttonKR();
		else
			buttonEN();

		string []strTitle = new string[] {
			"search_ko",
			"search_en",
			"search_jp",
			"search_cn"
		};
		int lan = DataManager.Instance.GetLanguage();
		UISprite sp = searchTitle.GetComponent<UISprite>();
		sp.spriteName = strTitle[lan];

		string []strCat = new string[] {
			"category_ko",
			"category_en",
			"category_jp",
			"category_cn"
		};
		sp = catTitle.GetComponent<UISprite>();
		sp.spriteName = strCat[lan];

		createCatBtn();
		//updateCategory();

		StartCoroutine( OpenPage () );
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

	public void OpenDetail1()
	{
		DataManager.Instance.currentId = 0;
		OpenLevel();
	}

	public void OpenDetail2()
	{
		DataManager.Instance.currentId = 1;
		OpenLevel();
	}

	public void OpenDetail3()
	{
		DataManager.Instance.currentId = 2;
		OpenLevel();
	}

	public void OpenDetail4()
	{
		DataManager.Instance.currentId = 3;
		OpenLevel();
	}

	public void OpenDetail5()
	{
		DataManager.Instance.currentId = 4;
		OpenLevel();
	}

	public void OpenDetail6()
	{
		DataManager.Instance.currentId = 5;
		OpenLevel();
	}

	public void OpenDetail7()
	{
		DataManager.Instance.currentId = 6;
		OpenLevel();
	}

	public void OpenDetail8()
	{
		DataManager.Instance.currentId = 7;
		OpenLevel();
	}

	public void OpenDetail9()
	{
		DataManager.Instance.currentId = 8;
		OpenLevel();
	}

	public void OpenLevel()
	{
		Application.LoadLevel( "3_Detail" );
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
	
	public void OpenPrevPage()
	{
		DataManager.Instance.currentPage--;
		if (DataManager.Instance.currentPage < 0)
			DataManager.Instance.currentPage = 0;
		StartCoroutine( OpenPage () );
	}

	public void OpenNextPage()
	{
		DataManager.Instance.currentPage++;

		if (DataManager.Instance.currentPage >= DataManager.Instance.itemList.Count / 9 )
			DataManager.Instance.currentPage--;
		StartCoroutine( OpenPage () );

	}

	IEnumerator OpenPage()
	{
		bLoaded = false;
		int lan = DataManager.Instance.GetLanguage();
		int page = DataManager.Instance.currentPage;
		for (int i = 0; i < 9; i++) 
		{
			dataListItem item = DataManager.Instance.GetItem( page * 9 + i );

			GameObject more = DataManager.FindGameObjectByName( itemList[i], "More" );
			if( item == null )
			{
				more.SetActive( false );
				UILabel [] ls = itemList[i].GetComponentsInChildren<UILabel> ();
				foreach( UILabel l in ls )
				{
					l.text = "";
				}
				UITexture t = itemList[i].GetComponentInChildren<UITexture> ();
				UITexture u = t.GetComponent<UITexture>();
				u.mainTexture = null;

				GameObject ob = DataManager.FindGameObjectByName( itemList[i], "itemDesc" );
				UISprite sp = ob.GetComponent<UISprite>();
				sp.spriteName = "list_contentr_blank";
				continue; 
			}
			else
			{
				more.SetActive( true );

				GameObject ob = DataManager.FindGameObjectByName( itemList[i], "itemDesc" );
				UISprite sp = ob.GetComponent<UISprite>();
				sp.spriteName = "list_contentr";
			}

			Debug.Log( "data " + i.ToString() + "..." + item.name);

			Dictionary<string, string> d = item.detail;
			UILabel [] labels = itemList[i].GetComponentsInChildren<UILabel> ();

			UITexture tex = itemList[i].GetComponentInChildren<UITexture> ();

			labels[0].trueTypeFont = fontList[2*lan];
			labels[0].fontSize = 24;
			labels[0].text = item.name;

			foreach (KeyValuePair<string, string> v in d) 
			{
				if( v.Key == "thumbnail" )
				{
					if( imageTexture[i] != null )
					{
						Destroy( imageTexture[i] );
						imageTexture[i] = null;

					}
					string targetFile = "file://" + v.Value;
					WWW www = new WWW(targetFile);
					yield return www;
					imageTexture[i] = www.texture;

					if (imageTexture[i] != null) 
					{
						UITexture ut = tex.GetComponent<UITexture>();
						ut.mainTexture = imageTexture[i];
						ut.MakePixelPerfect();
					}
					www.Dispose ();
				}
				else if( v.Key == "feature" )
				{
					labels[1].trueTypeFont = fontList[2*lan+1];
					labels[1].fontSize = 20;
					labels[1].text = v.Value;
				}
			}
		}

		int maxpage = DataManager.Instance.GetMaxPage();
		int curPage = DataManager.Instance.currentPage;

		foreach( GameObject b in pageButtons )
		{
			Destroy( b );
		}

		pageButtons.Clear();

		for( int i = 0; i < maxpage; i++ )
		{
			GameObject p = (GameObject)Instantiate( uiPagePrf );
			p.transform.parent = uiPageUI.transform;
			UILabel [] ls = p.GetComponentsInChildren<UILabel>();
			foreach( UILabel l in ls )
			{
				l.text = (i+1).ToString();
			}
			//p.transform.localPosition = new Vector3( 0, 0, 0 );
			p.transform.localPosition = new Vector3( (-maxpage * 62) * 0.5f + 62 * ( i + 1 )  , 0, 0 );
			p.transform.localRotation = Quaternion.identity;
			p.transform.localScale = Vector3.one;
			UIButton b = p.GetComponentInChildren<UIButton>();
			b.onClick.Add( pageClick[i] );
			if( i == curPage )
			{
				GameObject ll = DataManager.FindGameObjectByName( p, "letterBlack" );
				b.gameObject.SetActive( false );
				ll.SetActive( false );
			}

			pageButtons.Add( p );
		}

		uiPageLeft.transform.localPosition = new Vector3( ( -maxpage * 62 ) * 0.5f , 0, 0 );
		uiPageRight.transform.localPosition = new Vector3( ( maxpage * 62 ) * 0.5f + 62 , 0, 0 );


		TweenAlpha []tas = uiFade.GetComponents<TweenAlpha>();
		foreach( TweenAlpha ta in tas )
		{
			if( ta.tweenGroup == 0 )
				ta.PlayForward();
		}
		bLoaded = true;
	}

	public void ItemClick0()
	{
		ItemClick( 0 );
	}

	public void ItemClick1()
	{
		ItemClick( 1 );
	}

	public void ItemClick2()
	{
		ItemClick( 2 );
	}

	public void ItemClick3()
	{
		ItemClick( 3 );
	}

	public void ItemClick4()
	{
		ItemClick( 4 );
	}

	public void ItemClick5()
	{
		ItemClick( 5 );
	}

	public void ItemClick6()
	{
		ItemClick( 6 );
	}

	public void ItemClick7()
	{
		ItemClick( 7 );
	}

	public void ItemClick8()
	{
		ItemClick( 8 );
	}

	public void ItemClick( int id )
	{
		int page = DataManager.Instance.currentPage;
		dataListItem item = DataManager.Instance.GetItem( page * 9 + id );
		if( item == null )
			return;

		DataManager.Instance.currentId = id;
		OpenLevel();
	}

	public void SearchClick0()
	{
		SearchClick( 0 );
	}

	public void SearchClick1()
	{
		SearchClick( 1 );
	}

	public void SearchClick2()
	{
		SearchClick( 2 );
	}
	
	public void SearchClick3()
	{
		SearchClick( 3 );
	}
	
	public void SearchClick4()
	{
		SearchClick( 4 );
	}
	
	public void SearchClick5()
	{
		SearchClick( 5 );
	}

	public void SearchClick6()
	{
		SearchClick( 6 );
	}

	public void SearchClick7()
	{
		SearchClick( 7 );
	}
	
	public void SearchClick8()
	{
		SearchClick( 8 );
	}
	
	public void SearchClick9()
	{
		SearchClick( 9 );
	}
	
	public void SearchClick10()
	{
		SearchClick( 10 );
	}
	
	public void SearchClick11()
	{
		SearchClick( 11 );
	}
	
	public void SearchClick12()
	{
		SearchClick( 12 );
	}
	
	public void SearchClick13()
	{
		SearchClick( 13 );
	}
	
	public void SearchClick14()
	{
		SearchClick( 14 );
	}
	
	public void SearchClick15()
	{
		SearchClick( 15 );
	}
	
	public void SearchClick16()
	{
		SearchClick( 16 );
	}
	
	public void SearchClick17()
	{
		SearchClick( 17 );
	}
	
	public void SearchClick18()
	{
		SearchClick( 18 );
	}
	
	public void SearchClick19()
	{
		SearchClick( 19 );
	}
	
	public void SearchClick20()
	{
		SearchClick( 20 );
	}
	
	public void SearchClick21()
	{
		SearchClick( 21 );
	}
	
	public void SearchClick22()
	{
		SearchClick( 22 );
	}
	
	public void SearchClick23()
	{
		SearchClick( 23 );
	}
	
	public void SearchClick24()
	{
		SearchClick( 24 );
	}

	public void SearchClick25()
	{
		SearchClick( 25 );
	}
	
	public void SearchClick26()
	{
		SearchClick( 26 );
	}

	static string GetSearchBtnName( int language, int idx, int click )
	{
		string [] name = new string[] { "search_ko", "search_en" };
		string [] strClick = new string[] { "_def", "_clk" };

		if( idx == 0 )
		{
			if( click == 0 )
				return "allsearch_def";
			else
				return "allsearch_sel";
		}

		return name[language] + strClick[click] + "_" + idx.ToString("D2");
	}

	public void SearchClick( int pos )
	{
		if( searchIdx == pos )
			return;

		GameObject btn = (GameObject)searchButtons[searchIdx];

		UISprite sp = btn.GetComponent<UISprite>();
		sp.spriteName = GetSearchBtnName( language, searchIdx, 0 );

		searchIdx = pos;
		btn = (GameObject)searchButtons[searchIdx];
		sp = btn.GetComponent<UISprite>();
		sp.spriteName = GetSearchBtnName( language, searchIdx, 1 );

		Debug.Log( "serch : " + pos.ToString() );
		DataManager.Instance.pageLanguage = pos -1;
		DataManager.Instance.currentPage = 0;
		DataManager.Instance.SetCurrentListData();
		StartCoroutine( OpenPage() );
	}

	public void buttonKR()
	{
		if( language == 0 )
			return;

		UISprite sp = uiButtonKR.GetComponent<UISprite>();
		sp.spriteName = "search_ko_sel";

		sp = uiButtonEN.GetComponent<UISprite>();
		sp.spriteName = "search_en_def";

		language = 0;
		searchIdx = 0;
		ButtonSet();

		DataManager.Instance.sortingLanguage = 0;
		DataManager.Instance.pageLanguage = -1;
		DataManager.Instance.currentPage = 0;
		DataManager.Instance.SetCurrentListData();
		StartCoroutine ( OpenPage() );
	}

	public void buttonEN()
	{
		if( language == 1 )
			return;
		
		UISprite sp = uiButtonKR.GetComponent<UISprite>();
		sp.spriteName = "search_ko_def";
		
		sp = uiButtonEN.GetComponent<UISprite>();
		sp.spriteName = "search_en_sel";

		language = 1;
		searchIdx = 0;
		ButtonSet();

		DataManager.Instance.sortingLanguage = 1;
		DataManager.Instance.pageLanguage = -1;
		DataManager.Instance.currentPage = 0;
		DataManager.Instance.SetCurrentListData();
		StartCoroutine ( OpenPage() );
	}

	private void ButtonSet()
	{
		foreach( GameObject obj in searchButtons )
		{
			Destroy( obj );
		}
		searchButtons.Clear();

		int num;
		if( language == 0 )
			num = 14;
		else
			num = 26;

		GameObject btn = (GameObject)Instantiate( searchButton );
		btn.transform.parent = searchWidget.transform;
		btn.transform.localPosition = new Vector3( 0.0f, 0.0f, 0.0f );
		btn.transform.localRotation = Quaternion.identity;
		btn.transform.localScale = Vector3.one;
		UISprite sp = btn.GetComponent<UISprite>();
		sp.spriteName = "allsearch_sel";
		UIButton bt = btn.GetComponent<UIButton>();
		bt.onClick.Add( buttonSearch[0] );
		searchButtons.Add( btn );

		for( int i = 0; i < num; i++ )
		{
			btn = (GameObject)Instantiate( searchButton );
			btn.transform.parent = searchWidget.transform;
			btn.transform.localPosition = new Vector3( 48 * (i+1), 0.0f, 0.0f );
			btn.transform.localRotation = Quaternion.identity;
			btn.transform.localScale = Vector3.one;
			sp = btn.GetComponent<UISprite>();
			sp.spriteName = GetSearchBtnName( language, i+1, 0 );
			bt = btn.GetComponent<UIButton>();
			bt.onClick.Add( buttonSearch[i+1] );
			searchButtons.Add( btn );
		}
	}

	public void ButtonLanguage()
	{
		DataManager.Instance.nextScene = 2;
		Application.LoadLevel( "4_Language" );
	}

	public void buttonPage0()
	{
		buttonPage( 0 );
	}

	public void buttonPage1()
	{
		buttonPage( 1 );
	}

	public void buttonPage2()
	{
		buttonPage( 2 );
	}

	public void buttonPage3()
	{
		buttonPage( 3 );
	}
	
	public void buttonPage4()
	{
		buttonPage( 4 );
	}
	
	public void buttonPage5()
	{
		buttonPage( 5 );
	}
	
	public void buttonPage6()
	{
		buttonPage( 6 );
	}
	
	public void buttonPage7()
	{
		buttonPage( 7 );
	}
	
	public void buttonPage8()
	{
		buttonPage( 8 );
	}
	
	public void buttonPage9()
	{
		buttonPage( 9 );
	}
	
	public void buttonPage( int page )
	{
		if( page == DataManager.Instance.currentPage )
			return;
		Debug.Log( "Page : " + page.ToString() );
		DataManager.Instance.SetCurrentPage( page );
		StartCoroutine( OpenPage() );
	}

	public void buttonNextPage()
	{
		DataManager.Instance.SetCurrentPage( DataManager.Instance.currentPage + 1 );
		Debug.Log( "Page : " + DataManager.Instance.currentPage.ToString() );
		StartCoroutine( OpenPage() );
	}

	public void buttonPrevPage()
	{
		DataManager.Instance.SetCurrentPage( DataManager.Instance.currentPage - 1 );
		Debug.Log( "Page : " + DataManager.Instance.currentPage.ToString() );
		StartCoroutine( OpenPage() );
	}

	private void createCatBtn()
	{
		foreach( GameObject obj in catButtons )
		{
			Destroy( obj );
		}

		catButtons.Clear();


		int pos = 0;

		ArrayList groupList = DataManager.Instance.GetCurGroupList();

		GameObject btn = (GameObject)Instantiate( catButtonAll );
		btn.transform.parent = catWidget.transform;
		btn.transform.localPosition = Vector3.zero;
		btn.transform.localRotation = Quaternion.identity;
		btn.transform.localScale = Vector3.one;
		UIButton bt = btn.GetComponentInChildren<UIButton>();
		bt.onClick.Add( catClick[0] );

		catButtons.Add( btn );
		pos += 60 + 10;

		int i = 0;
		Font f = fontList[DataManager.Instance.GetLanguage() * 2];
		
		foreach( string strName in groupList )
		{
			btn = (GameObject)Instantiate( catButton );
			btn.transform.parent = catWidget.transform;
			btn.transform.localPosition = new Vector3( pos, 0, 0 );
			btn.transform.localRotation = Quaternion.identity;
			btn.transform.localScale = Vector3.one;
			bt = btn.GetComponentInChildren<UIButton>();
			bt.onClick.Add( catClick[i +1] );

			UILabel [] lbs = btn.GetComponentsInChildren<UILabel>();

			foreach( UILabel lb in lbs )
			{
				lb.trueTypeFont = f;
				lb.text = strName;
			}

			pos += 150 + 10;

			catButtons.Add( btn );

			i++;
		}

		updateCategory();
	}

	private void updateCategory()
	{
		int curCat = DataManager.Instance.pageGroup;

		for( int i = 1; i < catButtons.Count; i++ )
		{
			GameObject obj = (GameObject)catButtons[i];
			GameObject btn = DataManager.FindGameObjectByName( obj, "CatBtn" );
			btn.SetActive( true );
			
			GameObject lb = DataManager.FindGameObjectByName( obj, "CatTextBlack" );
			lb.SetActive( true );
		}

		if( curCat == -1 )
		{
			GameObject obj = (GameObject)catButtons[0];
			GameObject btn = DataManager.FindGameObjectByName( obj, "CatBtnAll" );
			btn.SetActive( false );
		}
		else
		{
			GameObject obj = (GameObject)catButtons[0];
			GameObject btn = DataManager.FindGameObjectByName( obj, "CatBtnAll" );
			btn.SetActive( true );

			obj = (GameObject)catButtons[curCat+1];
			btn = DataManager.FindGameObjectByName( obj, "CatBtn" );
			btn.SetActive( false );
			
			GameObject lb = DataManager.FindGameObjectByName( obj, "CatTextBlack" );
			lb.SetActive( false );
		}
	}

	private void buttonCat0()
	{
		buttonCat( 0 );
	}

	private void buttonCat1()
	{
		buttonCat( 1 );
	}
	
	private void buttonCat2()
	{
		buttonCat( 2 );
	}
	
	private void buttonCat3()
	{
		buttonCat( 3 );
	}
	
	private void buttonCat4()
	{
		buttonCat( 4 );
	}
	
	private void buttonCat5()
	{
		buttonCat( 5 );
	}
	
	private void buttonCat6()
	{
		buttonCat( 6 );
	}
	
	private void buttonCat7()
	{
		buttonCat( 7 );
	}
	
	private void buttonCat8()
	{
		buttonCat( 8 );
	}
	
	private void buttonCat9()
	{
		buttonCat( 9 );
	}
	
	private void buttonCat( int cat )
	{
		Debug.Log( "Cat Bttn : " + cat.ToString() );

		DataManager.Instance.pageGroup = cat - 1;
		DataManager.Instance.currentPage = 0;
		DataManager.Instance.SetCurrentListData();

		updateCategory();

		StartCoroutine ( OpenPage() );
	}


}
