using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Camera
{
    public class CinemachineShake : MonoBehaviour
    {
        public static CinemachineShake Instance { get; private set; }
        private CinemachineVirtualCamera cinemachineVC;
        private CinemachineBasicMultiChannelPerlin cinemachineBMCP;

        private float shakeTime = 0;
        private float shakeTimer = 0;
        private float shakeIntensity = 0;

        private void Awake()
        {
            Instance = this;
            cinemachineVC = GetComponent<CinemachineVirtualCamera>();
            cinemachineBMCP = cinemachineVC.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void StartShake(float _intensity, float time)
        {
            cinemachineBMCP.m_AmplitudeGain = _intensity;

            shakeIntensity = _intensity;
            shakeTime = time;
            shakeTimer = time;
        }

        // Update is called once per frame
        void Update()
        {
            if (shakeTimer <= 0) return;

            shakeTimer -= Time.deltaTime;

            cinemachineBMCP.m_AmplitudeGain = Mathf.Lerp(shakeIntensity, 0f, 1 - (shakeTimer / shakeTime));
        }
    }
}
