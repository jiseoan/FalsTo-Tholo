using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(UIButton))]
public class RMenuMgr : MonoBehaviour {

	public GameObject topUI;
	public GameObject featureUI;
	public GameObject specUI;
	public GameObject bottomUI;

	//public UIButton button;
	//public UIEventTrigger trigger;

	// Use this for initialization
	void Start () {
		//EventDelegate.Add( trigger.onClick, onClick );
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnClick() {
		Debug.Log ("Click Start");

	}
}
