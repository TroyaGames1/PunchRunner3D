using System;
using System.Collections.Generic;
using RayFire;
using TMPro;
using UniRx;
using UnityEngine;

public class ObstacleFacade : MonoBehaviour
{
    public ReactiveProperty<int> health;
    public float _hitTime = 0.5f;
   
    [SerializeField] private TextMeshProUGUI _textMesh;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private ParticleSystem _damageParticle;
    
   
    private RayfireRigid _rayfireRigid;
    private bool _canCheckRaycast;
    
   
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
           
            if (_hitTime > 0)
            {
                _hitTime -= Time.deltaTime;
            }
            else if (_hitTime <= 0)
            {
                TakeDamage();
                _hitTime = 0.5f;
            }
        }
        else if (!CanTakeHit)
        {
            _hitTime =  0.5f; //ScriptableObject'e aktarılabilir
        }
    }


    private void TakeDamage()
    {
        health.Value -= 1;
        _damageParticle.Play();

        if (health.Value<=0)
        {
            _rayfireRigid.Demolish();
        }
    }

    private bool CanTakeHit=>
        _canCheckRaycast && Physics.Raycast(transform.position, transform.forward,5 , _layerMask);
    

    private void OnTriggerEnter(Collider other)
    {
        _canCheckRaycast = true;
    }

   
}