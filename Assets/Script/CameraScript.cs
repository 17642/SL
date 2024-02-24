using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Rendering.PostProcessing;

public class CameraScript : MonoBehaviour
{

    PostProcessVolume ppp;

    [SerializeField]
    float PostProcessingVolumeLow;

    [SerializeField]
    float PostProcessingVolumeHigh;

    [SerializeField]
    float colorGradingTime;

    private bool privateLightStatus;

    private Transform prb;

    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private float shakeMagnitude;


    void Start()
    {
        privateLightStatus = StageManager.instance.stageLight;
        ppp = GetComponent<PostProcessVolume>();
        if (StageManager.instance.stageLight)
        {
            ppp.weight = PostProcessingVolumeLow;
        }
        else
        {
            ppp.weight = PostProcessingVolumeHigh;
        }

        prb = transform.parent;
        //gameObject.transform.SetParent(null);

    }

    void LateUpdate()
    {
        if (privateLightStatus != StageManager.instance.stageLight)
        {
            if (StageManager.instance.stageLight)
            {
                StopAllCoroutines();
                StartCoroutine(SmoothWeightChange(PostProcessingVolumeLow));
            }
            else
            {
                StopAllCoroutines();
                StartCoroutine(SmoothWeightChange(PostProcessingVolumeHigh));

            }
        }
        privateLightStatus = StageManager.instance.stageLight;

        //transform.position = prb.GetComponent<Rigidbody2D>().position;
        //transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    IEnumerator SmoothWeightChange(float targetWeight)
    {
        float initialWeight = ppp.weight;
        float timer = 0f;

        while (timer < colorGradingTime)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / colorGradingTime);
            ppp.weight = Mathf.Lerp(initialWeight, targetWeight, progress);
            yield return null;
        }
    }

    public void ShakeCam()
    {
        StartCoroutine(Shake(shakeMagnitude,shakeDuration));
    }

    IEnumerator Shake(float ShakeAmount, float ShakeTime)
    {
        float timer = 0;
        while (timer <= ShakeTime)
        {
            transform.localPosition = (Vector2)UnityEngine.Random.insideUnitCircle * ShakeAmount;
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, -1f);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = new Vector3(0f, 0f, -1f);
    }

}
