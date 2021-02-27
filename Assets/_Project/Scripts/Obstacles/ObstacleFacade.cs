using System;
using RayFire;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleFacade : MonoBehaviour
{
    public ReactiveProperty<int> health;
    public float _hitTime = 0.5f;
    [SerializeField] private TextMeshProUGUI _textMesh;
    private HitState _hitState;
    private RayfireRigid _rayfireRigid;
    [SerializeField] private LayerMask _layerMask;

    
    RaycastHit hit;

    private void Awake()
    {
        health.SubscribeToText(_textMesh).AddTo(this);
        _rayfireRigid = GetComponent<RayfireRigid>();
    }


    private void FixedUpdate()
    {
        CheckRayCastAndTakeDamage();
    }

    private void CheckRayCastAndTakeDamage()
    {
        if (CheckRayCast())
        {
            if (_hitTime > 0)
            {
                _hitTime -= Time.deltaTime;
            }
            else if (_hitTime <= 0)
            {
                Debug.Log(hit.collider.gameObject.name);
                TakeDamage();
                _hitTime = 0.5f;
            }
        }
        else if (!CheckRayCast())
        {
            _hitTime =  0.5f; //ScriptableObject'e aktarılabilir
        }
    }

    void TakeDamage()
    {
        health.Value -= 1;

        if (health.Value<=0)
        {
            _rayfireRigid.Demolish();
        }
    }

    private bool CheckRayCast()
    {

        return _hitState == HitState.HIT & Physics.Raycast(transform.position,
            transform.forward, out hit,3 , _layerMask);
    }


    private void OnTriggerEnter(Collider other)
    {
        _hitState = HitState.HIT;
    }

    
    


    private enum HitState
    {
        AWAKE,
        HIT
    }
}