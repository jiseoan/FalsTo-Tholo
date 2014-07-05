using UnityEngine;
using System.Collections;

public class RMenu : MonoBehaviour {

	public UIRoot root;

	// Use this for initialization
	void Start () {
//		RootScreen screenSize = root.GetComponent<RootScreen> ();
//		if (screenSize.width == 1680 && screenSize.height == 1050 ) {
//			transform.localPosition = new Vector3 ( 840 - 405, 525, 0);
//		} else if( screenSize.width == 1920 && screenSize.height == 1080 ) {
//			transform.localPosition = new Vector3 ( 960 - 405, 540, 0);
//		}

		if (Screen.height == 1050 ) {
			transform.localPosition = new Vector3 ( 840 - 403, 525, 0);
		} else if( Screen.height == 1080 ) {
			transform.localPosition = new Vector3 ( 960 - 403, 540, 0);
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
