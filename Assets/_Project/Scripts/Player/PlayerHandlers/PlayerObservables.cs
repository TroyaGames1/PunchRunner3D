using System;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerObservables
    {
        private readonly Player _player;
        private readonly TickableManager _tickableManager;
        PlayerObservables(Player player,TickableManager tickableManager)
        {
            _player = player;
            _tickableManager = tickableManager;
        }

        public IObservable<Collision> PlayerCollisionEnterObservable => _player.GO.OnCollisionEnterAsObservable();
        public IObservable<Collider> PlayerTriggerEnterObservable => _player.GO.OnTriggerEnterAsObservable();
        
        public IObservable<Touch[]> InputObservable
            =>_tickableManager.TickStream.Select(x =>Input.touches).Where(x =>Input.touchCount > 0);

    }
}


