using UnityEngine;
using System.Collections;

public class loadingIcon : MonoBehaviour {

	public GameObject icon;

	private float angle = 0.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		angle -= Time.deltaTime * 100.0f;
		icon.transform.localRotation = Quaternion.Euler( 0.0f, 0.0f, angle );
	}
}
