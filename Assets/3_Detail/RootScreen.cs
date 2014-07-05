using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RootScreen : MonoBehaviour {

	public UIRoot uiRoot;
	public Camera modelCamera;
	public UIPanel uiPanel;
	public GameObject ModelRoot;
	public GameObject uiFade;

	public GameObject uiTop;
	public GameObject uiSecond;
	public GameObject uiThird;

	public GameObject uiList;

	public GameObject uiListItem;

	public GameObject uiRightMenu;
	public GameObject uiLeftMenu;

	public GameObject uiAutorotateOn;
	public GameObject uiAutorotateOff;

	public UISlider uiCameraDistSlider;

	public UISprite uiPointTitle;
	public UISprite uiSpecTitle;

	public GameObject ShowHideBtn;

	public Font [] fontList = new Font[8];

	[HideInInspector]
	public float width;
	[HideInInspector]
	public float height;

	private GameObject model;
	private AssetBundle assetBundle = null;
	private Texture2D imageTexture = null;

	private float cameraElevation = Mathf.PI * 0.5f;
	private float cameraPolar = Mathf.PI * 0.5f;
	private float cameraDist = 10.0f;
	private bool bAutoRotate;

	private ArrayList specList = new ArrayList();

	private bool showhide;

	private bool bLoaded = false;
	private bool bScreensaverMode = false;
	private float ScreensaverTime = 0.0f;

	enum eCamera_Update {
		eCamera_None,
		eCamera_Up,
		eCamera_Down,
		eCamera_Right,
		eCamera_Left
	};
	private eCamera_Update updateCamera = eCamera_Update.eCamera_None;

	// Use this for initialization
	void Start () {
		bLoaded = false;
		bScreensaverMode = false;
		DataManager.Instance.currentScene = 3;

		uiFade.SetActive( true );
		if (Screen.height == 1050)
		{
			if( Screen.width == 1680 )
			{
				modelCamera.pixelRect = new Rect( 0.0f, 0.0f, 1680.0f, 1050.0f );
			}
			else if( Screen.width == 3360 )
			{
				modelCamera.pixelRect = new Rect( 1680.0f, 0.0f, 3360.0f, 1050.0f );
			}
			uiRoot.manualHeight = 1050;
			uiPanel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1680.0f, 1050f );

			width = 1680;
			height = 1050;
			uiPanel.transform.localPosition = new Vector3( 840.0f+0.5f, 0.0f+1, 0.0f );
		} else if( Screen.height == 1080 )
		{
			if( Screen.width == 1920 )
			{
				modelCamera.pixelRect = new Rect( 0.0f, 0.0f, 1920.0f, 1080.0f );
				//uiPanel.gameObject.transform.localPosition = new Vector3( 960.0f, 0.0f, 0.0f );
				uiRoot.manualHeight = 1080;
				uiPanel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080f );
				uiPanel.transform.localPosition = new Vector3( 840.0f + 60.0f+7+25, 0.0f+1, 0.0f );
			}
			else if( Screen.width == 3840 )
			{
				modelCamera.pixelRect = new Rect( 1920.0f, 0.0f, 3040.0f, 1080.0f );
				//uiPanel.gameObject.transform.localPosition = new Vector3( 960.0f, 0.0f, 0.0f );
				uiRoot.manualHeight = 1080;
				uiPanel.baseClipRegion = new Vector4( 0.0f, 0.0f, 1920.0f, 1080f );
				uiPanel.transform.localPosition = new Vector3( 840.0f + 60.0f+7, 0.0f+1, 0.0f );
			}

			width = 1920;
			height = 1080;
		}

		{
			TweenPosition []aniPos = ShowHideBtn.GetComponents<TweenPosition> ();
			
			ShowHideBtn.transform.localPosition = new Vector3 ( 244, -100, 0);
			foreach( TweenPosition tpos in aniPos )
			{
				if( tpos.tweenGroup == 0 || tpos.tweenGroup == 2 )
				{
					tpos.from = new Vector3( 244, -100, 0 );
					tpos.to = new Vector3( -80,  -100, 0 );
				}
				else if( tpos.tweenGroup == 1 || tpos.tweenGroup == 3 )
				{
					tpos.from = new Vector3( -80, -100, 0 );
					tpos.to = new Vector3( 244, -100, 0 );
				}
			}
			showhide = true;
		}

		if( DataManager.Instance.bScreenSaverMode )
		{
			if( DataManager.Instance.pageGroup != -1 || DataManager.Instance.pageLanguage != -1 )
			{
				DataManager.Instance.pageGroup = -1;
				DataManager.Instance.pageLanguage = -1;

				DataManager.Instance.SetCurrentListData();
			}

			DataManager.Instance.currentId = 0;
			DataManager.Instance.currentPage = 0;
			bScreensaverMode = true;
		}

		//uiAutorotateOff.SetActive( false );
		AutoRotateOff();
		UpdateCamera();

		StartCoroutine (LoadData ());
	}
	
	// Update is called once per frame
	void Update () 
	{
		if( bLoaded )
		{
			if( bAutoRotate )
			{
				cameraPolar += Time.deltaTime * 0.5f;
				UpdateCamera();
			}

			if( updateCamera == eCamera_Update.eCamera_Up )
			{
				cameraElevation += Time.deltaTime * 0.5f;
				UpdateCamera();
			}
			else if( updateCamera == eCamera_Update.eCamera_Down )
			{
				cameraElevation -= Time.deltaTime * 0.5f;
				UpdateCamera();
			}
			else if( updateCamera == eCamera_Update.eCamera_Left )
			{
				cameraPolar -= Time.deltaTime * 0.5f;
				UpdateCamera();
			}
			else if( updateCamera == eCamera_Update.eCamera_Right )
			{
				cameraPolar += Time.deltaTime * 0.5f;
				UpdateCamera();
			}

			if( DataManager.Instance.reloading )
			{
				assetBundle.Unload (true);
				DataManager.Instance.currentScene = 0;
				Application.LoadLevel( "1_Loading" );
			}
			if( DataManager.Instance.bScreenSaverMode )
			{
				bScreensaverMode = true;

				if( DataManager.Instance.pageGroup != -1 || DataManager.Instance.pageLanguage != -1 )
				{
					DataManager.Instance.pageGroup = -1;
					DataManager.Instance.pageLanguage = -1;
					
					DataManager.Instance.SetCurrentListData();
					
					DataManager.Instance.currentId = 0;
					DataManager.Instance.currentPage = 0;
				}

				AutoRotateOn();
			}
		}

		if( bScreensaverMode )
		{
			if( DataManager.Instance.bScreenSaverMode == false && bLoaded )
			{
				assetBundle.Unload (true);
				Application.LoadLevel( "2_List" );
			}

			ScreensaverTime += Time.deltaTime;

			if( ScreensaverTime > DataManager.Instance.ScreenSaverShowtime )
			{
				NextItem();
				ScreensaverTime = 0.0f;
			}
		}
	}

	IEnumerator LoadData()
	{
		bLoaded = false;

		foreach( GameObject go in specList )
		{
			Destroy( go );
		}

		specList.Clear();

		int lan = DataManager.Instance.GetLanguage();
		Font fontTitle = fontList[lan * 2];
		Font fontDesc = fontList[lan * 2 + 1];

		if (assetBundle != null) 
		{
			assetBundle.Unload (true);
			assetBundle = null;
		}

		if (imageTexture != null) 
		{
			Destroy( imageTexture );
			imageTexture = null;
			UITexture ut = uiTop.GetComponent<UITexture>();
			ut.mainTexture = imageTexture;
		}
		string modelname = DataManager.Instance.GetCurrentModel();

		Debug.Log( "ModelName : " + modelname );
		
		bool BundleExist = System.IO.File.Exists (modelname);
		if (BundleExist)
		{
			byte[] buffer = null;
			buffer = System.IO.File.ReadAllBytes(modelname);
			AssetBundleCreateRequest request = AssetBundle.CreateFromMemory (buffer);
			yield return request;
			if (request.assetBundle == null) {
				yield break;
			} else if (request.assetBundle.mainAsset == null) {
				yield break;
			}
			
			assetBundle = request.assetBundle;
			
			model = (GameObject)Instantiate( request.assetBundle.mainAsset );//, Vector3.zero, Quaternion.identity );
		}

		CamaraCenter();

		string [] strPoint = new string[] {
			"list_point_ko",
			"list_point_en",
			"list_point_jp",
			"list_point_cn"
		};
		uiPointTitle.spriteName = strPoint[lan];

		string [] strSpec = new string[] {
			"list_spec_ko",
			"list_spec_en",
			"list_spec_jp",
			"list_spec_cn"
		};
		uiSpecTitle.spriteName = strSpec[lan];

		// loading the file:
		string targetFile = "file://" + DataManager.Instance.GetCurrentItem().detail["image"];
		WWW www = new WWW(targetFile);
		yield return www;
		imageTexture = www.texture;

		if (imageTexture != null) 
		{
			UITexture ut = uiTop.GetComponent<UITexture>();
			ut.mainTexture = imageTexture;
			ut.MakePixelPerfect();
		}
		www.Dispose ();

		UILabel second = uiSecond.GetComponent<UILabel> ();
		second.trueTypeFont = fontDesc;
		second.text = DataManager.Instance.GetCurrentItem().detail ["feature"];

		foreach (KeyValuePair<string, string> kv in DataManager.Instance.GetCurrentItem().detail) 
		{
			if( kv.Key == "image" || kv.Key == "feature" || kv.Key == "thumbnail" )
				continue;
			GameObject item = (GameObject)AssetBundle.Instantiate( uiListItem, Vector3.zero, Quaternion.identity );

			item.transform.parent = uiList.transform;

			UILabel []labels = item.GetComponentsInChildren<UILabel>();
			foreach( UILabel label in labels )
			{
				if( label.name == "Name" )
				{
					label.trueTypeFont = fontTitle;
					label.text = kv.Key;
				}
				else if( label.name == "Value" )
				{
					label.trueTypeFont = fontDesc;
					label.text = kv.Value;
				}
			}

			specList.Add( item );

			item.transform.localPosition = Vector3.zero;
			item.transform.localRotation = Quaternion.identity;
			item.transform.localScale = Vector3.one;
		}
		UIGrid grid = uiList.GetComponent<UIGrid>();
		grid.repositionNow = true;

		TweenAlpha []tas = uiFade.GetComponents<TweenAlpha>();
		foreach( TweenAlpha ta in tas )
		{
			if( ta.tweenGroup == 0 )
				ta.PlayForward();
		}

		if( bScreensaverMode )
			AutoRotateOn();

		bLoaded = true;
	}

	public void ShowMenu()
	{
		TweenPosition []tas = uiRightMenu.GetComponentsInChildren<TweenPosition>();
		foreach( TweenPosition ta in tas )
		{
			if( ta.tweenGroup == 0 )
				ta.PlayForward();
		}
		tas = uiLeftMenu.GetComponentsInChildren<TweenPosition>();
		foreach( TweenPosition ta in tas )
		{
			if( ta.tweenGroup == 0 )
				ta.PlayForward();
		}
	}

	public void HideMenu()
	{
		TweenPosition []tas = uiRightMenu.GetComponentsInChildren<TweenPosition>();
		foreach( TweenPosition ta in tas )
		{
			if( ta.tweenGroup == 1 )
				ta.PlayForward();
		}
		tas = uiLeftMenu.GetComponentsInChildren<TweenPosition>();
		foreach( TweenPosition ta in tas )
		{
			if( ta.tweenGroup == 1 )
				ta.PlayForward();
		}
	}

	public void LoadEnd()
	{
	}

	public void OpenListLevel()
	{
		Application.LoadLevel( "2_List" );
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


	public void LoadList()
	{
		assetBundle.Unload (true);
		FadeOut();
	}

	public void CameraDistChange()
	{
		cameraDist = 8.0f - 4.0f * uiCameraDistSlider.value;
		UpdateCamera();
	}

	public void CameraZoomUP()
	{
		uiCameraDistSlider.value += 0.1f;
	}

	public void CameraZoomDown()
	{
		uiCameraDistSlider.value -= 0.1f;
	}

	public void CamaraCenter()
	{
		cameraElevation = Mathf.PI * 0.5f;
		cameraPolar = Mathf.PI * 0.5f;
		uiCameraDistSlider.value = 0.0f;

		UpdateCamera();
	}

	public void CameraMove( Vector3 delta )
	{
		cameraPolar += delta.x;
		cameraElevation -= delta.y;
		UpdateCamera();
	}

	public void AutoRotateOn()
	{
		uiAutorotateOn.SetActive( false );
		uiAutorotateOff.SetActive( true );
		bAutoRotate = true;
	}

	public void AutoRotateOff()
	{
		uiAutorotateOn.SetActive( true );
		uiAutorotateOff.SetActive( false );
		bAutoRotate = false;
	}

	private void UpdateCamera()
	{
		if( cameraElevation < Mathf.PI * 0.3f )
			cameraElevation = Mathf.PI * 0.3f;

		if( cameraElevation > Mathf.PI * 0.9f )
			cameraElevation = Mathf.PI * 0.9f;

		modelCamera.transform.localPosition = new Vector3( -cameraDist * Mathf.Sin( cameraElevation ) * Mathf.Cos( cameraPolar ), 
		                                                  -cameraDist * Mathf.Cos( cameraElevation ),
		                                                  -cameraDist * Mathf.Sin( cameraElevation )  * Mathf.Sin( cameraPolar ) );
		modelCamera.transform.localRotation = Quaternion.Euler( cameraElevation/  Mathf.PI * 180.0f - 90, -cameraPolar /  Mathf.PI * 180.0f + 90, 0.0f );
	}

	public void PrevItem()
	{
		DataManager.Instance.PrevItem();
		StartCoroutine( LoadData() );
	}

	public void NextItem()
	{
		DataManager.Instance.NextItem();
		StartCoroutine( LoadData() );
	}

	public void CameraUpPress()
	{
		updateCamera = eCamera_Update.eCamera_Up;
	}

	public void CameraUpRelease()
	{
		updateCamera = eCamera_Update.eCamera_None;
	}

	public void CameraDownPress()
	{
		updateCamera = eCamera_Update.eCamera_Down;
	}
	
	public void CameraDownRelease()
	{
		updateCamera = eCamera_Update.eCamera_None;
	}

	public void CameraRightPress()
	{
		updateCamera = eCamera_Update.eCamera_Right;
	}
	
	public void CameraRightRelease()
	{
		updateCamera = eCamera_Update.eCamera_None;
	}

	public void CameraLeftPress()
	{
		updateCamera = eCamera_Update.eCamera_Left;
	}
	
	public void CameraLeftRelease()
	{
		updateCamera = eCamera_Update.eCamera_None;
	}

	public void ButtonLanguage()
	{
		assetBundle.Unload (true);

		DataManager.Instance.nextScene = 3;
		Application.LoadLevel( "4_Language" );
	}

	public void ShowHideRMenu()
	{
		if( showhide )
		{
			UISprite sp = ShowHideBtn.GetComponent<UISprite>();
			sp.spriteName = "itemRpanel_show";
			TweenPosition []tps = uiRightMenu.GetComponentsInChildren<TweenPosition>();
			foreach( TweenPosition ta in tps )
			{
				if( ta.tweenGroup == 3 )
				{
					ta.ResetToBeginning();
					ta.PlayForward();
				}
			}
			showhide = false;
		}
		else
		{
			UISprite sp = ShowHideBtn.GetComponent<UISprite>();
			sp.spriteName = "itemRpanel_hide";
			TweenPosition [] tps = uiRightMenu.GetComponentsInChildren<TweenPosition>();
			foreach( TweenPosition ta in tps )
			{
				if( ta.tweenGroup == 2 )
				{
					ta.ResetToBeginning();
					ta.PlayForward();
				}
			}
			showhide = true;
		}
	}
}
