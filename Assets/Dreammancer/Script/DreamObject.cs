using UnityEngine;
using System.Collections;

public class DreamObject : MonoBehaviour {
    private const float EPILSON = 1e-2f;

    public Material hitLightMaterial;

    [SerializeField]
    protected float mThreshold = 100.0f;

    [SerializeField]
    protected float mRecoverTime = 3.0f;

    protected int id = 0;
    protected bool isDetected = false;
    protected bool isEffect = false;

    protected Color mOriginColor;
    protected Color c;
    protected SpriteRenderer mRenderer;
    protected Collider2D mCollider;
    protected Rigidbody2D mRigidbody;
    protected Material mMaterial;
    protected Light2D mReflectLight;

    protected void Start()
    {
        id = gameObject.GetInstanceID();

        Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnLightExit);
        mRenderer = GetComponent<SpriteRenderer>();
        mCollider = GetComponent<Collider2D>();
        mRigidbody = GetComponent<Rigidbody2D>();
        mMaterial = mRenderer.material;
        mOriginColor = mMaterial.color;
        c = mOriginColor;
    }

    void OnDestroy()
    {
        Light2D.UnregisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.UnregisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.UnregisterEventListener(LightEventListenerType.OnExit, OnLightExit);
    }

    protected void Update()
    {
        if(mCollider.enabled && mRenderer.enabled)
        {
            float rate = 10.0f;
            mMaterial.color = Color.Lerp(mMaterial.color, c, Time.deltaTime * rate);
            Debug.Log(mMaterial.color + "; " + c);
        }
    }

    void OnLightEnter(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id && !isDetected)
        {
            c = mMaterial.color;
        }
    }

    void OnLightStay(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Stay");
            isDetected = true;
        
            if(isAffect(l.LightColor) && !isEffect)
            {
                isEffect = true;

                Color curColor = mMaterial.color;
                c = new Color(curColor.r, curColor.g, curColor.b, 0.0f);
                Debug.Log("Effect");
            }
            else if(!isEffect && c.a < mThreshold)
            {
                Debug.Log("Lighting");
                c += l.LightColor;
            }

            if (mMaterial.color.a <= 0.0f + EPILSON)
            {
                isEffect = true;
                mCollider.enabled = false;
                mRenderer.enabled = false;
                mRigidbody.gravityScale = 0.0f;
            }
        }
    }

    void OnLightExit(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Exit");
            if (isEffect)
            {
                StartCoroutine(fadeBackToOrigin(mRecoverTime));
            }

            isDetected = false;
            isEffect = false;
            c = mOriginColor;
        }
    }

    bool isAffect(Color lightColor)
    {
        Vector3 lightColorV = new Vector3(lightColor.r, lightColor.g, lightColor.b);
        Vector3 materialColorV = new Vector3(mMaterial.color.r, mMaterial.color.g, mMaterial.color.b);
        lightColorV.Normalize();
        materialColorV.Normalize();

        return (lightColorV == materialColorV && mMaterial.color.a >= mThreshold - EPILSON);
    }

    IEnumerator fadeBackToOrigin(float sec)
    {
        yield return new WaitForSeconds(sec);
        mCollider.enabled = true;
        mRenderer.enabled = true;
        mRigidbody.gravityScale = 3.0f;
    }
}
