using System;
using Events;
using UnityEngine;
using Zenject;

public class HitItemFacade : MonoBehaviour
{
    private SignalBus _signalBus;
    [SerializeField] private float HealtValue;
    [SerializeField] private bool _canDestroyable;
    
    

    [Inject]
    public void Constructor(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnTriggerEnter(Collider other)
    {
        _signalBus.Fire(new SignalPlayerHit(HealtValue));
        if (_canDestroyable)
        {
            Destroy(gameObject);
        }
        
    }

    
    
    
}
