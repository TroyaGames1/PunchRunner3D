using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cysharp.Threading.Tasks;
using Events;
using UnityEngine;
using Zenject;

public class CinemachineShake : MonoBehaviour
{
    private CinemachineVirtualCamera _camera;
    private float _shakeTime;
    private CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    [Inject] private SignalBus _signalBus;

    private void Awake()
    {
        _camera = GetComponent<CinemachineVirtualCamera>();
        _signalBus.Subscribe<SignalDoShake>(() => ShakeTask());
        cinemachineBasicMultiChannelPerlin =_camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

    }

 
    
    async UniTask ShakeTask()
    {
      
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain =0.7f; 
        await UniTask.Delay(TimeSpan.FromSeconds(1f));
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0;

    }
}
