using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public class dataLanguage
{
	public string name;
//	public ArrayList order;
//	public string image;
}

public class dataGroup
{
	public string name;
	public ArrayList order;
}

public class dataItemlist
{
	public string name;
	public string model;
	public int korean;
	public int english;
	public int group;
	//public string group;
	public ArrayList desc;
}

public class dataDesc
{
	public string language;
	public string name;
	//public string group;
	public Dictionary <string, string> list;
	public Dictionary <string, string> detail;
}

public class dataListItem
{
	public string name;
	public string model;
	public Dictionary<string, string> detail;
}

public class DataManager : MonoBehaviour {

	static DataManager _instance;
	public GameObject rootUI = null;
	public GameObject backPanel = null;

	public Font [] fontList = new Font[8];

	private UILabel labelUI = null;
	//public string configFilename;

	[HideInInspector]
	public float width;
	[HideInInspector]
	public float height;

	[HideInInspector]
	public ArrayList languageList;
	[HideInInspector]
	public ArrayList groupList;
	[HideInInspector]
	public ArrayList itemList;

	[HideInInspector]
	public string currentLanguage;
	[HideInInspector]
	public int pageLanguage;
	[HideInInspector]
	public int sortingLanguage;
	[HideInInspector]
	public int pageGroup;
	[HideInInspector]
	public int currentPage;
	[HideInInspector]
	public int currentId;
	[HideInInspector]
	public int nextScene;
	[HideInInspector]
	public ArrayList curDataList = new ArrayList();

	//private int currentGroupPos = -1;
	[HideInInspector]
	public bool reloading = false;
	private bool reloaded = false;
	private float versionCheckTime = 0;
	private float versionCheckInterval = 0;
	private string CurrentVersion;
	private string dataupdateFilename;
	private float screenSaverInterval = 0;
	private float lastClickTime = 0;
	[HideInInspector]
	public int currentScene = 0;
	private Vector3 lastClickPos;

	[HideInInspector]
	public string urlPath = "";
	[HideInInspector]
	public bool bScreenSaverMode = false;
	[HideInInspector]
	public float ScreenSaverShowtime;

	private float AliveTick = 0.0f;

	public void Awake()
	{
		DontDestroyOnLoad (gameObject);
	}

	public static DataManager Instance
	{
		get
		{
			if (_instance == null) {
				_instance = FindObjectOfType (typeof(DataManager)) as DataManager;
				if (_instance == null) {
						_instance = new GameObject ("Data Manager", 
				        	typeof(DataManager)).GetComponent<DataManager> ();
				}
			}
			return _instance;
		}
	}

	// Use this for initialization
	void Start () {
		currentScene = 0;

		if (Screen.height == 1050)
		{
			width = 1680;
			height = 1050;
			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1050;
				
				UIPanel panel = backPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1680.0f, 1050f );
			}
		} else if( Screen.height == 1080 )
		{
			width = 1920;
			height = 1080;
			
			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1080;
				
				UIPanel panel = backPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080.0f );
				
				//backPanel.gameObject.transform.localPosition = new Vector3( 960.0f, 0.0f, 0.0f );
				backPanel.gameObject.transform.localPosition = new Vector3( 840.0f + 60.0f, 0.0f, 0.0f );
			}
		}
		
		if (rootUI != null) 
		{
			labelUI = rootUI.GetComponentInChildren<UILabel> ();
		}


		StartCoroutine( LoadData() );
	}

	IEnumerator LoadData()
	{
/*
		if (Screen.height == 1050)
		{
			width = 1680;
			height = 1050;
			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1050;

				UIPanel panel = backPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1680.0f, 1050f );
			}
		} else if( Screen.height == 1080 )
		{
			width = 1920;
			height = 1080;

			if( rootUI != null )
			{
				UIRoot root = rootUI.GetComponent<UIRoot>();
				root.manualHeight = 1080;

				UIPanel panel = backPanel.GetComponent<UIPanel>();
				panel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080.0f );

				backPanel.gameObject.transform.localPosition = new Vector3( 960.0f, 0.0f, 0.0f );
			}
		}

		if (rootUI != null) 
		{
			labelUI = rootUI.GetComponentInChildren<UILabel> ();
		}
*/

		string targetFile = "";

		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
			targetFile = "file:// + " + Application.dataPath + "/../config.json";
		else if (Application.platform == RuntimePlatform.OSXPlayer)
			targetFile = "file:// + " + Application.dataPath + "/../../config.json";
		else
			targetFile = "file:// + " + Application.dataPath + "/../config.json";

		LoadingLog( "Loading : " + targetFile );
			
		WWW www = new WWW(targetFile);
		Debug.Log( "www : " + targetFile );
		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		yield return www;
		if (www.error != null)
			LoadingLog( "Error : " + www.error );
		else
			LoadingLog( "Load complete... : " + targetFile );

		JsonData jsonConfig = JsonMapper.ToObject(www.text);
		www.Dispose ();
		www = null;

		urlPath = jsonConfig["url"].ToString() + "/";
		dataupdateFilename = urlPath + jsonConfig["dataupdate"].ToString();
//		versionCheckInterval = Convert.ToInt16( jsonConfig["updatechecktime"].ToString() ) * 60;
		string settingFilenane = urlPath + jsonConfig["setting"].ToString();
//		ScreenSaverShowtime = Convert.ToInt16( jsonConfig["showtime"].ToString() ) * 60;
		
		string url = jsonConfig["url"].ToString() + "/" + jsonConfig["config"].ToString();
		Debug.Log("URL: "+ url );

		www = new WWW( dataupdateFilename );
		yield return www;
		if (www.error != null)
			LoadingLog( "Error : " + www.error );
		else
			LoadingLog( "Load complete... : " + dataupdateFilename );
		
		JsonData jsonData = JsonMapper.ToObject(www.text);
		www.Dispose ();
		www = null;
		CurrentVersion = jsonData["releaseno"].ToString();



		www = new WWW( settingFilenane );
		yield return www;
		if (www.error != null)
			LoadingLog( "Error : " + www.error );
		else
			LoadingLog( "Load complete... : " + dataupdateFilename );
		
		jsonData = JsonMapper.ToObject(www.text);
		www.Dispose ();
		www = null;

		try
		{
			screenSaverInterval = Convert.ToInt16( jsonData["screensaverInterval"].ToString() ) * 60;
		}
		catch( Exception )
		{
			screenSaverInterval = 30 * 60;
		}

		try
		{
			versionCheckInterval = Convert.ToInt16( jsonData["updateInterval"].ToString() ) * 60;
		}
		catch
		{
			versionCheckInterval = 0;
		}

		try
		{
			ScreenSaverShowtime = Convert.ToInt16( jsonData["showtime"].ToString() );
		}
		catch
		{
			ScreenSaverShowtime = 30;
		}

		lastClickTime = Time.time;

		www = new WWW(url );

		LoadingLog( "Loading : " + url );

		//Load the data and yield (wait) till it's ready before we continue executing the rest of this method.
		yield return www;
		if (www.error != null) 
		{
			Debug.Log("ERROR: "+ url );
			LoadingLog("ERROR: "+ www.error );
		}
		else
		{
			Debug.Log("Load complete... : "+ url );
			LoadingLog( "Load complete... : " + url );
		}

		LoadData (www.text);

		www.Dispose ();
		www = null;

		LoadingLog( "Json Loaded...." );

		for (int i = 0; i < itemList.Count; i++) 
		{
			dataItemlist data = (dataItemlist) itemList[i];
			www = new WWW( urlPath + data.model );
			
			yield return www;
			
			if( www.error != null )
			{
				Debug.Log( "ERROR: " + data.model );
				LoadingLog( "ERROR: " + data.model );
			}
			else
			{
				Debug.Log( "Load complete : " + data.model );
				LoadingLog( "Load complete : " + data.model );
				string purename = data.model;

				string pathname = Application.persistentDataPath + "/AssetBundle/" + purename;
				pathname = System.IO.Path.GetDirectoryName( pathname );
				System.IO.Directory.CreateDirectory( pathname );

				Debug.Log( "pathname : " + pathname );

				//string purename = System.IO.Path.GetFileName( data.model );
				System.IO.FileStream _fs = new System.IO.FileStream(Application.persistentDataPath + 
				                                                    "/AssetBundle/" + purename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
				System.IO.BinaryWriter _bw = new System.IO.BinaryWriter(_fs); 
				_bw.Write( www.bytes ); 
				_bw.Close(); 
				_fs.Close();

				if( www.assetBundle != null )
				{
					www.assetBundle.Unload( true );
				}
				
				data.model = Application.persistentDataPath + "/AssetBundle/" + purename;
				Debug.Log( "itemlist model filename : " + data.model );
				LoadingLog( "Save complete : " + data.model );
			}

			www.Dispose();
			www = null;

			for( int j = 0; j < data.desc.Count; j++ )
			{
				Debug.Log( "name : " + data.name );
				dataDesc desc = (dataDesc)data.desc[j];
				foreach( KeyValuePair<string, string> kv in desc.list )
				{
					if( kv.Key == "image" || kv.Key == "thumbnail" )
					{
						www = new WWW( urlPath + kv.Value );
						yield return www;
						
						if( www.error != null )
						{
							LoadingLog( "ERROR: " + kv.Value );
						}
						else
						{
							LoadingLog( "Load complete : " + kv.Value );

							string purename = kv.Value;
							//string purename = System.IO.Path.GetFileName( kv.Value );

							string pathname = Application.persistentDataPath + "/AssetBundle/" + purename;
							pathname = System.IO.Path.GetDirectoryName( pathname );
							System.IO.Directory.CreateDirectory( pathname );



							System.IO.FileStream _fs = new System.IO.FileStream(Application.persistentDataPath + 
							                                                    "/AssetBundle/" + purename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
							System.IO.BinaryWriter _bw = new System.IO.BinaryWriter(_fs); 
							_bw.Write( www.bytes ); 
							_bw.Close(); 
							_fs.Close();
							
							string val = Application.persistentDataPath + "/AssetBundle/" + purename;
							desc.list[kv.Key] = val;
							LoadingLog( "Save complete : " + val );
						}
						www.Dispose();
						www = null;
						break;
					}
				}

				Dictionary<string, string> temp = new Dictionary<string, string>();

				foreach( KeyValuePair< string, string> kv in desc.detail )
				{
					if( kv.Key == "image" || kv.Key == "thumbnail" )
					{
						www = new WWW( urlPath + kv.Value );
						yield return www;
						
						if( www.error != null )
						{
							LoadingLog( "ERROR: " + kv.Value );
						}
						else
						{
							LoadingLog( "Load complete : " + kv.Value );

							string purename = kv.Value;

							string pathname = Application.persistentDataPath + "/AssetBundle/" + purename;
							pathname = System.IO.Path.GetDirectoryName( pathname );
							System.IO.Directory.CreateDirectory( pathname );

							Debug.Log( "image name : " + purename );

							//string purename = System.IO.Path.GetFileName( kv.Value );
							System.IO.FileStream _fs = new System.IO.FileStream(Application.persistentDataPath + 
							                                                    "/AssetBundle/" + purename, System.IO.FileMode.Create, System.IO.FileAccess.Write);
							System.IO.BinaryWriter _bw = new System.IO.BinaryWriter(_fs); 
							_bw.Write( www.bytes ); 
							_bw.Close(); 
							_fs.Close();
							
							string val = Application.persistentDataPath + "/AssetBundle/" + purename;
							temp[kv.Key] = val;

							LoadingLog( "Save complete : " + val );
						}
						www.Dispose();
						www = null;
						//break;
					}
				}

				foreach( KeyValuePair<string, string> kv in temp )
				{
					desc.detail[kv.Key] = kv.Value;
				}

				data.desc[j] = desc;
			}

			itemList[i] = data;
		}

//		foreach( dataItemlist d in itemList )
//		{
//			Debug.Log( " model : " + d.model );
//		}

//		ArrayList arr = new ArrayList();
//		string [] temp = new string[] { "사아자", "가나다", "라마바", "차카타", "파하" };
//		foreach( string str in temp )
//		{
//			arr.Add( str );
//		}
//
//		arr.Sort();
//
//		foreach( string tt in arr )
//		{
//			Debug.Log( "sorted : " + tt );
//		}


		Debug.Log( "language count = " + languageList.Count.ToString() );
		currentLanguage = ((dataLanguage)languageList [0]).name;
		pageLanguage = -1;
		pageGroup = -1;
		//currentGroupPos = -1;
		currentPage = 0;
		currentId = 0;
		sortingLanguage =0;

		SetCurrentListData();

		if( currentScene == 0 )
		{
			reloading = false;
			Application.LoadLevel ( "2_List" );
		}
		else
			reloaded = true;
	}
	
	// Update is called once per frame
	void Update () {
		if( urlPath != "" )
		{
			versionCheckTime += Time.deltaTime;
			if( versionCheckTime > versionCheckInterval && versionCheckInterval != 0 )
			{
				StartCoroutine( LoadVersion () );
			}
		}

		if( currentScene == 0 && reloaded == true )
		{
			Application.LoadLevel( "2_List" );
		}

		if( lastClickPos !=  Input.mousePosition )
		{
			lastClickPos = Input.mousePosition;
			lastClickTime = Time.time;
			bScreenSaverMode = false;
		}

		if( screenSaverInterval != 0 && Time.time - lastClickTime >  screenSaverInterval )
		{
			bScreenSaverMode = true;
			if( currentScene != 3 )
			{
				Application.LoadLevel( "3_Detail" );
			}
		}

		if( Time.time - AliveTick > 10.0f )
		{
			AliveTick = Time.time;

			string targetFile = "";
			
			if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
				targetFile = Application.dataPath + "/../conf/alive.txt";
			else if (Application.platform == RuntimePlatform.OSXPlayer)
				targetFile = Application.dataPath + "/../../conf/alive.txt";
			else
				targetFile = Application.dataPath + "/../conf/alive.txt";

			string pathname = System.IO.Path.GetDirectoryName( targetFile );
			System.IO.Directory.CreateDirectory( pathname );


			int mili = (int)(AliveTick * 1000);

			System.IO.StreamWriter sw = new System.IO.StreamWriter( targetFile, false );
			sw.WriteLine( mili.ToString() );
			sw.Close();
			
			
//			System.IO.FileStream _fs = new System.IO.FileStream( targetFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
//			System.IO.TextWriter _tw = new System.IO.TextWriter(_fs);
//			_tw.Write( mili.ToString() );
//			_tw.Close(); 
//			_fs.Close();
		}
	}

	IEnumerator LoadVersion()
	{
		if( urlPath != "" )
		{
			WWW www = new WWW( dataupdateFilename );
			yield return www;
			if (www.error != null)
				LoadingLog( "Error : " + www.error );
			else
				LoadingLog( "Load complete... : " + dataupdateFilename );
			
			JsonData jsonData = JsonMapper.ToObject(www.text);
			www.Dispose ();
			www = null;

			if( CurrentVersion != jsonData["releaseno"].ToString() )
			{
				CurrentVersion = jsonData["releaseno"].ToString();
				reloading = true;
				reloaded = false;
				StartCoroutine( LoadData() );

			}
			versionCheckTime = 0;
		}
	}

	static string GetBaseUrl ()
	{
		if (Application.platform == RuntimePlatform.OSXEditor || Application.platform == RuntimePlatform.WindowsEditor)
			return "file:// + " + Application.dataPath + "/../builds/AssetBundles/";
		if (Application.platform == RuntimePlatform.OSXPlayer)
			return "file:// + " + Application.dataPath + "/../../AssetBundles/";
		else
			return "AssetBundles/";
	}


	private void LoadData( string jsonString )
	{
		JsonData jsonData = JsonMapper.ToObject( jsonString);

		System.IO.Directory.CreateDirectory( Application.persistentDataPath + "/AssetBundle" );
		// Language
		languageList = new ArrayList();
		for(int i = 0; i<jsonData["language"].Count; i++)
		{
			dataLanguage data = new dataLanguage();
			
			data.name = jsonData["language"][i]["name"].ToString();
			languageList.Add( data );
		}

		// group
		ArrayList groupKorean = null;
		groupList = new ArrayList ();
		for (int i = 0; i<jsonData["group"].Count; i++) 
		{
			dataGroup data = new dataGroup();

			data.name = jsonData["group"][i]["name"].ToString();
			data.order = new ArrayList();
			for( int j = 0; j < jsonData["group"][i]["order"].Count; j++ )
			{
				data.order.Add( jsonData["group"][i]["order"][j].ToString() );
			}

			if( data.name == "Korean" )
				groupKorean = data.order;
			groupList.Add ( data );
		}

		// itemlist
		itemList = new ArrayList ();
		for (int i = 0; i < jsonData["itemlist"].Count; i++)
		{
			dataItemlist data = new dataItemlist ();
			data.name = jsonData["itemlist"][i]["name"].ToString();
			data.model = jsonData["itemlist"][i]["model"].ToString();
			string group = jsonData["itemlist"][i]["group"].ToString();
			for( int ii = 0; ii < groupKorean.Count; ii++ )
			{
				if( (string)groupKorean[ii] == group )
				{
					data.group = ii;
					break;
				}
			}

			data.desc = new ArrayList();
			for( int j = 0; j < jsonData["itemlist"][i]["desc"].Count; j++ )
			{
				dataDesc desc = new dataDesc();
				JsonData jsonDesc = jsonData["itemlist"][i]["desc"][j];
				desc.language = jsonDesc["language"].ToString();
				desc.name = jsonDesc["name"].ToString();
				desc.list = new Dictionary<string,string >();
				desc.detail = new Dictionary<string,string >();
				var detailkeys = (jsonDesc["detail"] as IDictionary).Keys;
				foreach (string s in detailkeys) 
				{
					desc.detail[s] = jsonDesc["detail"][s].ToString();
					Debug.Log( s + " :  " + desc.detail[s].ToString() );
				}

				if( desc.language == "Korean" )
					data.korean = GetKoreanShotcut( desc.name[0] );
				else if( desc.language == "English" )
					data.english = desc.name.ToLower()[0] - 'a';

				data.desc.Add ( desc );
			}
			itemList.Add( data );
		}
	}

	private void LoadingLog( string str )
	{
		if (labelUI == null)
			return;

		labelUI.text += str + "\n";
	}
	
	static public GameObject FindGameObjectByName( GameObject parent, string name )
	{
		return FindGameObjectByName( parent.transform, name );
	}


	static public GameObject FindGameObjectByName( Transform parent, string name )
	{
		if (parent.name == name) return parent.gameObject;
		
		for (int i = 0; i < parent.childCount; ++i)
		{
			GameObject result = FindGameObjectByName(parent.GetChild(i), name);
			
			if (result != null) return result;
		}

		return null;
	}

	public void SetCurrentListData()
	{
		curDataList.Clear();

		foreach( dataItemlist item in itemList )
		{
			if( pageGroup != -1 )
			{
				if( item.group != pageGroup )
					continue;
			}

			if( pageLanguage != -1 )
			{
				if( sortingLanguage == 0 )
				{
					if( pageLanguage != item.korean )
						continue;
				}
				else
				{
					if( pageLanguage != item.english )
						continue;
				}
			}

			dataListItem data = new dataListItem();
			data.model = item.model;

			dataDesc desc = null;
			foreach( dataDesc d in item.desc )
			{
				if( d.language == currentLanguage )
				{
					desc = d;
					break;
				}
				else if( desc == null && d.language == "English" )
					desc = d;
			}

			data.name = desc.name;
			data.detail = desc.detail;

			curDataList.Add( data );
		}


		foreach( dataListItem i in curDataList )
		{
			Debug.Log( " curData : " + i.name );
		}
	}

	public dataListItem GetCurrentItem()
	{
		return (dataListItem)curDataList[currentId + currentPage * 9];
	}
	
	public dataListItem GetItem (int id)
	{
		if( curDataList.Count > id )
			return (dataListItem)curDataList[id];
		else
			return null;
	}

//	public dataDesc GetDesc (int id)
//	{
//		if (itemList.Count > id)
//			return GetDesc (GetItem (id));
//		else
//			return null;
//	}
//	
//	public dataDesc GetDesc( dataItemlist data )
//	{
//		foreach (dataDesc desc in data.desc) 
//		{
//			if( desc.language == currentLanguage )
//				return desc;
//		}
//		return null; 
//	}
//	
//	public dataDesc GetCurrentDesc()
//	{
//		dataItemlist data = GetCurrentItem ();
//		foreach (dataDesc desc in data.desc) 
//		{
//			if( desc.language == currentLanguage )
//				return desc;
//		}
//		return null; 
//	}
	
	public string GetCurrentModel()
	{
		dataListItem data = GetCurrentItem ();
		return data.model;
	}

	public int GetMaxPage()
	{
		return (curDataList.Count + 8 ) / 9;
	}

	public void SetCurrentPage( int page )
	{
		int max = GetMaxPage();
		if( page < 0 )
		{
			do{
				page += max;
			}while( page < 0 );
		}

		if( page >= max )
		{
			do{
				page -= max;
			}while( page >= max );
		}
		currentPage = page;
	}

	public void SetCurrentItem( int id )
	{
		int max = curDataList.Count;
		if( id < 0 )
		{
			do{
				id += max;
			}while( id < 0 );
		}

		if( id >= max )
		{
			do{
				id -= max;
			}while( id >= max );
		}
		currentId = id;

		Debug.Log( "Setcurrent item " + currentId.ToString() );
	}

	public void PrevItem()
	{
		currentId--;
		if( currentId < 0 )
		{
			currentPage--;
			currentId = 8;
			if( currentPage < 0 )
			{
				currentPage = curDataList.Count / 9;
				currentId = (curDataList.Count % 9) - 1;
			}
		}
	}

	public void NextItem()
	{
		currentId++;
		if( currentId == 9 || ( currentId == (curDataList.Count %9) && currentPage == curDataList.Count / 9) )
		{
			currentPage++;
			currentId = 0;
			if( currentPage == GetMaxPage() )
			{
				currentPage = 0;
				currentId = 0;
			}
		}
	}

	public void ChangeLanguage( string lan )
	{
		currentLanguage = lan;
		SetCurrentListData();
	}

	public ArrayList GetCurGroupList()
	{
		foreach( dataGroup g in groupList )
		{
			if( g.name == currentLanguage )
				return g.order;
		}
		return null;
	}

//	public int GetCurrentGroupPos()
//	{
//		return currentGroupPos;
//	}
//
//	public void SetCurrentGroupPos( int pos )
//	{
//		currentGroupPos = pos;
//	}

	private int GetKoreanShotcut( char korean )
	{
		if( korean < 44032 )
			return -1;
		int code = (int)korean - 44032;
		code /= 28;
		code /= 21;

		char [] KoreanChar = new char[] { 'ㄱ', 'ㄲ', 'ㄴ', 'ㄷ', 'ㄸ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅃ', 'ㅅ', 'ㅆ', 'ㅇ', 'ㅈ', 'ㅉ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'};
		char [] KoreanChar2 = new char[] { 'ㄱ', 'ㄴ', 'ㄷ', 'ㄹ', 'ㅁ', 'ㅂ', 'ㅅ', 'ㅇ', 'ㅈ', 'ㅊ', 'ㅋ', 'ㅌ', 'ㅍ', 'ㅎ'};

		char c = KoreanChar[code];
		if( c == 'ㄲ' )
			c = 'ㄱ';
		else if( c == 'ㄸ' )
			c = 'ㄷ';
		else if( c == 'ㅃ' )
			c = 'ㅂ';
		else if( c == 'ㅆ' )
			c = 'ㅅ';

		for( int i = 0; i < KoreanChar2.Length; i++ )
		{
			if( c == KoreanChar2[i] )
				return i;
		}
		return -1;
	}

	public int GetLanguage()
	{
		for( int i = 0; i < languageList.Count; i++ )
		{
			dataLanguage l = (dataLanguage)languageList[i];
			if( l.name == currentLanguage )
				return i;
		}
		return 0;
	}
}
