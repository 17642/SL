using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EnemyTrace : MonoBehaviour
{
    private Transform ParentTransform;
    [SerializeField]
    private float refreshTime;
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private float initialAlpha = 0.5f;

    Renderer renderers;

    //private Material material;

    private bool privateLightStatus;



    void Start()
    {
        ParentTransform = transform.parent;
        gameObject.transform.SetParent(null);
        transform.rotation = Quaternion.identity;//회전 초기화

        renderers = GetComponent<Renderer>();


        if (!StageManager.instance.stageLight) StartCoroutine(MoveToNewPosition());

        privateLightStatus = StageManager.instance.stageLight;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (privateLightStatus != StageManager.instance.stageLight)
        {
            if (StageManager.instance.stageLight)
            {
                StopAllCoroutines();
                renderers.material.color = new Color(renderers.material.color.r, renderers.material.color.g, renderers.material.color.b, 0f);
            }
            else
            {
                StartCoroutine(MoveToNewPosition());

            }
        }
        privateLightStatus = StageManager.instance.stageLight;
    }

    IEnumerator MoveToNewPosition()
    {
        while (true)
        {
            if (transform.position != ParentTransform.position)
            {
                transform.position = ParentTransform.position;
                transform.rotation = ParentTransform.rotation;
                renderers.material.color = new Color(renderers.material.color.r, renderers.material.color.g, renderers.material.color.b, initialAlpha);
                yield return new WaitForSeconds(refreshTime);
            }
            else
            {
                renderers.material.color = new Color(renderers.material.color.r, renderers.material.color.g, renderers.material.color.b, 0f);
                yield return null;
            }
        }
    }
    IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeTime)
        {
            float alpha = Mathf.Lerp(initialAlpha, 0f, elapsedTime / fadeTime);
            renderers.material.color = new Color(renderers.material.color.r, renderers.material.color.g, renderers.material.color.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderers.material.color = new Color(renderers.material.color.r, renderers.material.color.g, renderers.material.color.b, 0f);

    }
}
