using UnityEngine;
using Cinemachine;

namespace Camera
{
    public class CinemachineShake : MonoBehaviour
    {
        public static CinemachineShake Instance { get; private set; }
        private CinemachineVirtualCamera _cinemachineVc;
        private CinemachineBasicMultiChannelPerlin _cinemachineBmcp;

        private float _shakeTime;
        private float _shakeTimer;
        private float _shakeIntensity;

        private void Awake()
        {
            Instance = this;
            _cinemachineVc = GetComponent<CinemachineVirtualCamera>();
            _cinemachineBmcp = _cinemachineVc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

        public void StartShake(float intensity, float time)
        {
            _cinemachineBmcp.m_AmplitudeGain = intensity;

            _shakeIntensity = intensity;
            _shakeTime = time;
            _shakeTimer = time;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_shakeTimer <= 0) return;

            _shakeTimer -= Time.deltaTime;

            _cinemachineBmcp.m_AmplitudeGain = Mathf.Lerp(_shakeIntensity, 0f, 1 - (_shakeTimer / _shakeTime));
        }
    }
}
