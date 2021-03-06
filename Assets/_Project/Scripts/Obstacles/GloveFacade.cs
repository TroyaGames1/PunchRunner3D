﻿using Events;
using UnityEngine;
using Zenject;

public class GloveFacade : MonoBehaviour
{
    private SignalBus _signalBus;
    [SerializeField] private float HealtValue;
    

    [Inject]
    public void Constructor(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    private void OnCollisionEnter(Collision other)
    {
        _signalBus.Fire(new SignalPlayerHit(HealtValue));
        Destroy(gameObject);
    }
}
