using UnityEngine;
using System.Collections;

public class RMenuFeature : MonoBehaviour {

	public UIRoot root;

	const float offsetX = 405.0f;
	const float posY = ( 387.0f );

	// Use this for initialization
	void Start () {
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
				tp.from = new Vector3( 0, -posY,0 );
				tp.to = new Vector3( offsetX, -posY,0 );
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
