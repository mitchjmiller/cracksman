using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class EffectsManager : MonoBehaviour {
  public static EffectsManager Instance;

  [SerializeField] private CinemachineVirtualCamera virtualCam;
  CinemachineBasicMultiChannelPerlin mcp;
  private float shakeDuration = 0, shakeIntensity = 0;

  private void Awake() {
    if (Instance != null) {
      Destroy(gameObject);
      return;
    }
    Instance = this;
  }

  private void Start() {
    mcp = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
  }

  private void Update() {
    UpdateScreenShake();
  }

  private void UpdateScreenShake() {
    CinemachineBasicMultiChannelPerlin mcp = virtualCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    if (shakeDuration > 0)
      mcp.m_AmplitudeGain = Mathf.Max(0, mcp.m_AmplitudeGain - Time.deltaTime / shakeDuration * shakeIntensity);
  }

  public void ScreenShake(float duration, float intensity = 2) {
    shakeDuration = duration;
    shakeIntensity = intensity;
    mcp.m_AmplitudeGain = shakeIntensity;
  }
}
