using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UniRx;
using UnityEngine;
using Zenject;

public class DoTweenUtility : MonoBehaviour
{
    [SerializeField] private bool _canMove;
    [SerializeField] private bool _canRotate;
    [SerializeField] private bool _canScale;

    [SerializeField] private bool _reverse;

    [SerializeField] private bool _canLoop;

    [SerializeField] private bool _trigger;
    
    
    [SerializeField] private float _moveTo;
    [SerializeField] private float _moveDuration; 
    [SerializeField] private Vector3 _rotateTo;
    [SerializeField] private float _rotateDuration;
    [SerializeField] private Vector3 _scaleTo;
    [SerializeField] private float _scaleDuration;
    [SerializeField] private  Ease _easeType;

    private Tween _moveTween;
    private Tween _rotationTween;
    private Tween _scaleTween;

    private readonly List<Tween> _tweenList=new List<Tween>();

    private Vector3 _endPos;
    
    
    void Start()
    {
        
        if (_canMove)
        {

            _endPos = transform.localPosition;
            _tweenList.Add(transform
                .DOLocalMove((_endPos)+Vector3.up*_moveTo, _moveDuration)
                .SetEase(Ease.InOutQuad)
                .SetLoops(-1, LoopType.Yoyo));

        }

        if (_canRotate)
        {
            if (_reverse)
            {
                if (_canLoop)
                {
                    _tweenList.Add( transform
                        .DOLocalRotate(_rotateTo, _rotateDuration,
                            RotateMode.FastBeyond360)
                        .SetEase(_easeType).SetLoops(-1));
                }
                else
                {
                    _tweenList.Add( transform
                        .DOLocalRotate(_rotateTo, _rotateDuration,
                            RotateMode.FastBeyond360)
                        .SetEase(_easeType));
                }
              
            }
            else
            {
                if (_canLoop)
                {
                    _tweenList.Add( transform
                        .DOLocalRotate(_rotateTo, _rotateDuration, 
                            RotateMode.FastBeyond360)
                        .SetEase(_easeType).SetLoops(-1));  
                }
                else
                {
                    _tweenList.Add(transform
                        .DOLocalRotate(_rotateTo, _rotateDuration,
                            RotateMode.FastBeyond360)
                        .SetEase(_easeType));
                }
              
            }
          
        }

        if (_canScale)
        {
            _tweenList.Add(transform.DOScale(_scaleTo, _scaleDuration)
           .SetEase( Ease.InOutQuad).SetLoops(-1,LoopType.Yoyo)); 
        }
        
       
    }

  

    private void OnTriggerEnter(Collider other)
    {
        if (_trigger)
        {
            _tweenList.Add(transform
                .DOLocalRotate(_rotateTo, _rotateDuration,
                    RotateMode.FastBeyond360)
                .SetEase(_easeType));
        }
    }

    private void OnDisable()
    {
        foreach (var tween in _tweenList)
        {
            tween.Pause();
        }
        
    }

    private void OnEnable()
    {
        
        
        
        _endPos = transform.localPosition;
       foreach (var tween in _tweenList)
       {
           tween.Play();
       }

        
    }
}

