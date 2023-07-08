using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CamManager : MonoBehaviour
{
    public static CamManager instance;
    [SerializeField] CinemachineVirtualCamera cam;
    void Awake()
    {
        instance = this;
    }
    public void CamShake(float duration)
    {
        StartCoroutine(Shake(duration, 5));
    }

    IEnumerator Shake(float duration, float magnitude)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        float elapsed = 0.0f;
        float originalIntensity = magnitude;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(originalIntensity, 0f, elapsed / duration);
            yield return null;
        }
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;
    }
}
