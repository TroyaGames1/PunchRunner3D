using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Events;
using Shapes;
using UniRx;
using UnityEngine;
using Zenject;

public class BoxMachine : MonoBehaviour
{
    [Inject]private SignalBus _signalBus;
    [SerializeField] private Quad _quad;

    private DOTween _doTween;
    
    float _floatValue=0;
    private void Awake()
    {
        _signalBus.Subscribe<SignalPunch>(Move);
    }
    
    void Move()
    {
        Observable.Timer(TimeSpan.FromSeconds(0.9f)).Subscribe(x =>
        {
            transform.DOLocalRotate( new Vector3(180, 352.8849f, 0),0.2f).SetEase(Ease.OutExpo);
            DOTween.To( DegiskeninDegeriniAl, DegiskeninDegeriniDegistir, 3.0f, 2f );
            //DOTween.To(() => _floatValue, yzx => _floatValue = x, 5, 1f).SetEase(Ease.InOutCubic);
        });
    }
    
    float degisken;
  
 
  
    float DegiskeninDegeriniAl()
    {
        return degisken;
    }
  
    void DegiskeninDegeriniDegistir( float yeniDeger )
    {
        degisken = yeniDeger;
    }

    private void Update()
    {
        _quad.B = new Vector3(-1, degisken, 0);
        _quad.C = new Vector3(0.6f, degisken, 0);
    }
    
    
}
