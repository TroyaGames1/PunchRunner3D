using System;
using System.Collections.Generic;
using RayFire;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class ObstacleFacade : MonoBehaviour
{
    public ReactiveProperty<float> health;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private ParticleSystem _damageParticle;
    
   
    private RayfireRigid _rayfireRigid;
    private bool _canCheckRaycast;
    private float _currentHitTime;
    private Settings _settings;

    [Inject]
    public void Constructor(Settings settings)
    {
        _settings = settings;
        _currentHitTime = _settings.defaultHitTime;
    }

    
    private void Awake()
    {
        _rayfireRigid = GetComponent<RayfireRigid>();
        health.SubscribeToText(_textMesh).AddTo(this);
    }


    private void FixedUpdate()
    {
        CheckRayCastAndTakeDamage();
    }

    private void CheckRayCastAndTakeDamage()
    {
        if (CanTakeHit)
        {
           
            if (_currentHitTime> 0)
            {
                _currentHitTime -= Time.deltaTime;
            }
            else if (_currentHitTime <= 0)
            {
                TakeDamage();
                _currentHitTime = _settings.defaultHitTime;
            }
        }
        else if (!CanTakeHit)
        {
            _currentHitTime =  _settings.defaultHitTime;
        }
    }


    private void TakeDamage()
    {
        health.Value -= _settings.hitDamage;
        _damageParticle.Play();

        if (health.Value<=0)
        {
            _rayfireRigid.Demolish();
        }
    }

    private bool CanTakeHit=>
        _canCheckRaycast && Physics.Raycast(transform.position, transform.forward,5 , _settings.HitLayerMask);
    

    private void OnTriggerEnter(Collider other)
    {
        _canCheckRaycast = true;
    }

    [Serializable]
    public struct Settings
    {
        [InfoBox("Default Value 0.7")]
        [HorizontalGroup("Group 1",0.5f,LabelWidth = 125)]
        public float defaultHitTime;
        [InfoBox("Default Value 1")]
        [HorizontalGroup("Group 1",0.5f,LabelWidth = 125)]
        public float hitDamage;
        [HorizontalGroup("Group 2",LabelWidth = 0)]
        [LabelText("HitLayer")]
        [HideLabel]
        [InfoBox("Default Value Dynamic")]
        public LayerMask HitLayerMask;
    }
   
}