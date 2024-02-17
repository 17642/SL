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



}
