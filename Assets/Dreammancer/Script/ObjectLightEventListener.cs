using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class ObjectLightEventListener : MonoBehaviour {

    private SpriteRenderer mSpriteRenderer;

	// Use this for initialization
	void Start () {
        mSpriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
  
	}
}
