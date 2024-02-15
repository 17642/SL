using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrace : MonoBehaviour
{
    private Transform ParentTransform;
    [SerializeField]
    private float refreshTime;
    [SerializeField]
    private float fadeTime;
    [SerializeField]
    private float initialAlpha = 0.5f;

    private Material material;

    private bool privateLightStatus;



    void Start()
    {
        ParentTransform = transform.parent;
        gameObject.transform.SetParent(null);
        transform.rotation = Quaternion.identity;//회전 초기화

        Renderer renderer = ParentTransform.GetComponent<Renderer>();


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
                StopCoroutine(MoveToNewPosition());
                StopCoroutine(FadeOut());
            }
            else
            {
                StartCoroutine(MoveToNewPosition());
                renderer.material.color.a=0f;
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
                renderer.material.color.a = initialAlpha;
                StartCoroutine(FadeOut());
                yield return new WaitForSeconds(refreshTime);
            }else{
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
            renderer.material.color.a = alpha;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        renderer.material.color.a = 0f;
    }
}
