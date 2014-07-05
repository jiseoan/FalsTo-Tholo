using UnityEngine;
using System.Collections;

public class RMenuSpec : MonoBehaviour {

	public UIRoot root;

	const float offsetX = 405.0f;
	const float posY = ( 387.0f + 238.0f );

	int [] menuHeight = new int[2]{ 425, 425 + 30 };
	//int [] menuHeight = new int[2]{ 425, 425 + 150 };
	//int [] menuHeight = new int[2]{ 268, 268 + 150 };

	// Use this for initialization
	void Start () {
		RootScreen screenSize = root.GetComponent<RootScreen> ();
		TweenPosition []aniPos = GetComponents<TweenPosition> ();

		transform.localPosition = new Vector3 ( offsetX, -posY, 0);
		foreach( TweenPosition tp in aniPos )
		{
			if( tp.tweenGroup == 0 || tp.tweenGroup == 2 )
			{
				tp.from = new Vector3( offsetX, -posY, 0 );
				tp.to = new Vector3( 0, -posY, 0 );
			}
			else if( tp.tweenGroup == 1 || tp.tweenGroup == 3 )
			{
				tp.from = new Vector3( 0, -posY, 0 );
				tp.to = new Vector3( offsetX, -posY, 0 );
			}
		}

		UISprite sprite = GetComponent<UISprite>();
		if (screenSize.width == 1680 && screenSize.height == 1050 ) {
			sprite.width = (int)offsetX;
			sprite.height = menuHeight[0];
		} else if( screenSize.width == 1920 && screenSize.height == 1080 ) {
			sprite.width = (int)offsetX;
			sprite.height = menuHeight[1];
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
