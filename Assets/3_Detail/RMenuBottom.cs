using UnityEngine;
using System.Collections;

public class RMenuBottom : MonoBehaviour {

	public UIRoot root;

	const float offsetX = 405.0f;
	float []posY = new float[2]{ 387.0f + 238.0f + 268.0f, 387.0f + 238.0f + 418.0f - 120.0f };

	// Use this for initialization
	void Start () {
		RootScreen screenSize = root.GetComponent<RootScreen> ();
		TweenPosition []aniPos = GetComponents<TweenPosition> ();

		int off = 0;
		if (Screen.height == 1050 ) {
			off = 0;
		} else if( Screen.height == 1080 ) {
			off = 1;
		}

		transform.localPosition = new Vector3 ( offsetX, -posY[off], 0);
		
		foreach( TweenPosition tp in aniPos )
		{
			if( tp.tweenGroup == 0 )
			{
				tp.from = new Vector3( offsetX, -posY[off], 0 );
				tp.to = new Vector3( 0, -posY[off], 0 );
			}
			else if( tp.tweenGroup == 1 )
			{
				tp.from = new Vector3( 0, -posY[off],0 );
				tp.to = new Vector3( offsetX, -posY[off],0 );
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
