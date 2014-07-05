using UnityEngine;
using System.Collections;

public class LMenu : MonoBehaviour {

	public GameObject LMenuPanel;
	// Use this for initialization
	void Start () {
		
		if (Screen.height == 1050 ) {
			transform.localPosition = new Vector3 ( -840, 525, 0);
		} else if( Screen.height == 1080 ) {
			transform.localPosition = new Vector3 ( -960, 540, 0);
		}
//		if ( DataManager.Instance.width == 1680 && DataManager.Instance.height == 1050 ) {
//			transform.localPosition = new Vector3 ( -840, 525, 0);
//		} else if( DataManager.Instance.width == 1920 && DataManager.Instance.height == 1200 ) {
//			transform.localPosition = new Vector3 ( -960, 600, 0);
//		}
		LMenuPanel.transform.localPosition = new Vector3 ( -305, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
