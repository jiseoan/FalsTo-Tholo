using UnityEngine;
using System.Collections;

public class InputRegion : MonoBehaviour {

	public RootScreen rootScreen;
	Vector3 mousePos = Vector2.zero;
	bool pressed = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if( pressed )
		{
			Vector3 delta = (Input.mousePosition - mousePos) * 0.01f;
			if( delta != Vector3.zero )
			{
				rootScreen.CameraMove( delta );
				mousePos += delta * 100;
			}
		}
	
	}

	void OnPress( bool isPress )
	{
		if( isPress == true )
		{
			pressed = true;
			mousePos = Input.mousePosition;
		}
		else
		{
			pressed = false;
		}

	}
}
