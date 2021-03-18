using System;
using System.Collections.Generic;
using Events;
using MoreMountains.NiceVibrations;
using RayFire;
using Sirenix.OdinInspector;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

public class ObstacleFacade : MonoBehaviour
{
    private Settings _settings;
    private SignalBus _signalBus;
    public ReactiveProperty<float> health;
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private ParticleSystem _damageParticle;
    private Transform _rayfireBody;
    private MeshRenderer _meshRenderer;
    private BoxCollider[] _colliders;
    
    private bool _canCheckRaycast;
    private float _currentHitTime;
    
    [Inject]
    public void Constructor(Settings settings,SignalBus signalBus)
    {
        _settings = settings;
        _signalBus = signalBus;
        _currentHitTime = _settings.defaultHitTime;
    }

    
    private void Awake()
    {
        health.SubscribeToText(_textMesh).AddTo(this);
        _signalBus.Subscribe<SignalPlayerFailed>(x =>
        {
            enabled = false;
        });
        FindObjectComponents();
     
    }

    private void FindObjectComponents()
    {
        _rayfireBody = transform.GetChild(0).transform;
        _meshRenderer = GetComponent<MeshRenderer>();
        _colliders = GetComponents<BoxCollider>();
        
    }

    private void Update()
    {
        CheckRayCastAndTakeDamage();
    }

    private void CheckRayCastAndTakeDamage()
    {
        switch (CanTakeHit)
        {
            case true when _currentHitTime> 0:
                _currentHitTime -= Time.deltaTime;
                break;
            case true:
            {
                if (_currentHitTime <= 0)
                {
                    TakeDamage();
                    _signalBus.Fire(new SignalPlayerHit(-0.15f));
                    _signalBus.Fire(new SignalDoShake());
                    _currentHitTime = _settings.defaultHitTime;
                }

                break;
            }
            case false:
                _currentHitTime =  _settings.defaultHitTime;
                break;
        }
    }


    private void TakeDamage()
    {
        MMVibrationManager.Haptic(HapticTypes.SoftImpact,true);
        
        health.Value -= _settings.hitDamage;
        _damageParticle.Play();

        if (health.Value<=0)
        {
          DisableThisObject();
          
        }
    }

    private void DisableThisObject()
    {
        _meshRenderer.enabled = false;
        _textMesh.gameObject.SetActive(false);
        foreach (var boxCollider in _colliders)
        {
            boxCollider.enabled = false;
        }
        _rayfireBody.gameObject.SetActive(true);
         enabled = false;
    }

    private bool CanTakeHit=>
         Physics.Raycast(transform.position, transform.forward,1.4f , _settings.HitLayerMask);
    


    private void OnCollisionEnter(Collision other)
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