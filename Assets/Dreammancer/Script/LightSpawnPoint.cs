using UnityEngine;
using System.Collections;

public class LightSpawnPoint : MonoBehaviour {

    [SerializeField]
    private Color mLightColor;

    [SerializeField]
    private float mLightRadius;

    [SerializeField]
    private int mAngle;

    [SerializeField]
    private float mConeStart;

    private Light2D mLight;

	void Start () {
        mLight = Light2D.Create(gameObject, transform.position, mLightColor, mLightRadius, mAngle);
        mLight.LightConeStart = mConeStart;
        mLight.EnableEvents = true;
	}
	
	void Update () {
	
	}
}
