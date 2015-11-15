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
            Debug.Log(c);
        }
        else
            mMaterial.color = Color.Lerp(mMaterial.color, mOriginColor, Time.deltaTime * 5f);

        isDetected = false;
    }

    void OnLightEnter(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Enter");
            c = mOriginColor + l.LightColor;
        }
    }

    void OnLightStay(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Stay");
            isDetected = true;
        }
    }

    void OnLightExit(Light2D l, GameObject g)
    {
        if (g.GetInstanceID() == id)
        {
            Debug.Log("Exit");

        }
    }
}
