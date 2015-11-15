using UnityEngine;
using System.Collections;
using UnityStandardAssets._2D;

public class DreamCharacter : DreamObject {

    private static float EPILSON = 0.2f;

    private Light2D mFlashLight;
    private Bounds mBounds;

    private PlatformerCharacter2D mPlatformCharacter;

	// Use this for initialization
	public void Start () {
        base.Start();
        mBounds = GetComponent<Collider2D>().bounds;
        mFlashLight = Light2D.Create(transform.position + new Vector3(mBounds.extents.x + EPILSON, 0, 0), 
            Color.blue, 10.0f, 15);
        mFlashLight.EnableEvents = true;
        mFlashLight.LightConeStart = 0;
        mPlatformCharacter = GetComponent<PlatformerCharacter2D>();
    }
	
	// Update is called once per frame
	public void Update () {
        base.Update();
        float angle = Vector2.Angle(Vector2.right, ((mPlatformCharacter.FacingRight) ? 
            mPlatformCharacter.transform.right : -mPlatformCharacter.transform.right));

        Vector3 offset = new Vector3(mBounds.extents.x + EPILSON, 0, 0);
        mFlashLight.transform.position = transform.position + ((mPlatformCharacter.FacingRight) ? offset : -offset);
        mFlashLight.LightConeStart = angle;
    }
}
