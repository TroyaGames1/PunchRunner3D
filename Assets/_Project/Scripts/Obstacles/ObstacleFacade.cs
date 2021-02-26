using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleFacade : MonoBehaviour
{
    public ReactiveProperty<int> _health;
    [SerializeField] private Text _textMesh;
    

    private void Awake()
    {
      //  _health.SubscribeToText(_textMesh);
    
    }

    private void OnTriggerEnter(Collider other)
    {
        _health.Value -= 1;

        if (_health.Value<=0)
        {
            //rigidfire
        }
    }
}
