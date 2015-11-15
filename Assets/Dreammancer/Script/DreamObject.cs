using UnityEngine;
using System.Collections;

public class DreamObject : MonoBehaviour {
    public Material hitLightMaterial;

    int id = 0;
    bool isDetected = false;

    Color mOriginColor;
    Color c = Color.black;
    Renderer mRenderer;
    Material mMaterial;
    Light2D mReflectLight;

    void Start()
    {
        id = gameObject.GetInstanceID();

        Light2D.RegisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.RegisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.RegisterEventListener(LightEventListenerType.OnExit, OnLightExit);
        mRenderer = GetComponent<Renderer>();
        mMaterial = mRenderer.material;
        mOriginColor = mMaterial.color;
    }

    void OnDestroy()
    {
        /* (!) Make sure you unregister your events on destroy. If you do not
         * you might get strange errors (!) */

        Light2D.UnregisterEventListener(LightEventListenerType.OnStay, OnLightStay);
        Light2D.UnregisterEventListener(LightEventListenerType.OnEnter, OnLightEnter);
        Light2D.UnregisterEventListener(LightEventListenerType.OnExit, OnLightExit);
    }

    void Update()
    {
        if (isDetected) {
            mMaterial.color = Color.Lerp(mMaterial.color, c, Time.deltaTime * 10f);
        }
        else
            mMaterial.color = Color.Lerp(mMaterial.color, mOriginColor, Time.deltaTime * 5f);
    }

    void OnLightEnter(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id && !isDetected)
        {
            Debug.Log("Enter angle: ");
            Debug.Log(l.LightConeStart);
            mReflectLight = Light2D.Create(transform.position, Color.cyan, 5.0f, 30);
            Debug.Log(mReflectLight.transform.position);
            mReflectLight.LightConeStart = 180.0f - l.LightConeStart;
            c = mOriginColor + l.LightColor;
        }
    }

    void OnLightStay(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Stay");
            mReflectLight.transform.position = transform.position - GetComponent<Collider2D>().bounds.extents;
            isDetected = true;
        }
    }

    void OnLightExit(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Exit");
            DestroyObject(mReflectLight);
            isDetected = false;
        }
    }
}
