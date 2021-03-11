using System;
using DG.Tweening;
using Events;
using Shapes;
using UniRx;
using UnityEngine;
using Zenject;

public class BoxMachine : MonoBehaviour
{
    [Inject]private SignalBus _signalBus;
    [SerializeField] private Transform _hitGO;
    [SerializeField] private Quad _quad;
    [SerializeField] private ParticleSystem[] _particleSystems;
    

    private DOTween _doTween;
    
    
    float _quadValue;
    private void Awake()
    {
        _signalBus.Subscribe<SignalPunch>(x=>Move(x.xValue));
    }
    
    void Move(float xvalue)
    {
        Observable.Timer(TimeSpan.FromSeconds(0.9f)).Subscribe(x =>
        {
            _hitGO.DOLocalRotate( new Vector3(180, 352.8849f, 0),0.2f).SetEase(Ease.OutExpo);
            
            DOTween.To( GetValue, SetValue, CalculatePoint(xvalue), 2f );
            
            foreach (var system in _particleSystems)
            {
                system.gameObject.SetActive(true);
            }
          
        });
    }

    float CalculatePoint(float xValue)
    {
        float returnValue = 0;
        switch (xValue)
        {
            case float n when n>=-438 & n<-334||n>434 & n<=538:
                returnValue= 0.8f;
                break;
            case float n when n>=-334 & n<-224||n>324 & n<=434:
                returnValue= 1.6f;
                break; 
            case float n when n>=-224 & n<-124||n>224 &n<=324:
                returnValue= 2.4f;
                break;  
            case float n when n>=-124 & n<0|| n>100& n<=224:
                returnValue= 3.2f;
                break;
            case float n when (n>=0 & n<=100):
                returnValue= 4f;
                break;
        }

        return returnValue;
    }


    private float GetValue() => _quadValue;

    private void SetValue( float newValue )=> _quadValue = newValue;
    

    private void Update()
    {
        _quad.B = new Vector3(-1, _quadValue, 0);
        _quad.C = new Vector3(0.6f, _quadValue, 0);
    }

  
}
