using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

	// Configurable Parameters
	[SerializeField] float bgScrollSpeed = 0.5f;
	Material myMaterial;
	Vector2 offset;

	// Use this for initialization
	void Start () {
		myMaterial = GetComponent<Renderer>().material;
		offset = new Vector2(0, bgScrollSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		myMaterial.mainTextureOffset += offset * Time.deltaTime;
	}
}
