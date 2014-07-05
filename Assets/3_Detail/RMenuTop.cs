using UnityEngine;
using System.Collections;

public class RMenuTop : MonoBehaviour {

	public UIRoot root;
	// Use this for initialization

	const float offsetX = 405.0f;
	const float posY = ( 0.0f );

	void Start () {
		TweenPosition []aniPos = GetComponents<TweenPosition> ();

		transform.localPosition = new Vector3 ( offsetX, posY, 0);
		foreach( TweenPosition tpos in aniPos )
		{
			if( tpos.tweenGroup == 0 || tpos.tweenGroup == 2 )
			{
				tpos.from = new Vector3( offsetX, posY, 0 );
				tpos.to = new Vector3( 0,  posY, 0 );
			}
			else if( tpos.tweenGroup == 1 || tpos.tweenGroup == 3 )
			{
				tpos.from = new Vector3( 0, posY, 0 );
				tpos.to = new Vector3( offsetX, posY, 0 );
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
