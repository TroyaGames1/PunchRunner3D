using System;
using PlayerState;
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
        private readonly PlayerStateManager _stateManager;
        PlayerObservables(Player player,TickableManager tickableManager, PlayerStateManager stateManager)
        {
            _player = player;
            _tickableManager = tickableManager;
            _stateManager = stateManager;
        }

        public IObservable<Collision> PlayerCollisionEnterObservable => _player.GO.OnCollisionEnterAsObservable();
        public IObservable<Collision> PlayerCollisionStayObservable => _player.GO.OnCollisionStayAsObservable();
        public IObservable<Collision> PlayerCollisionExitObservable => _player.GO.OnCollisionExitAsObservable();
        public IObservable<Collider> PlayerTriggerStayObservable => _player.GO.OnTriggerStayAsObservable();
        public IObservable<Collider> PlayerTriggerExitObservable => _player.GO.OnTriggerExitAsObservable();
        public IObservable<Collider> PlayerTriggerEnterObservable => _player.GO.OnTriggerEnterAsObservable();

        public IObservable<PlayerStateManager.PlayerStates> PlayerStateObservable => _tickableManager.TickStream
            .Select(x => _stateManager.CurrentState);
        public IObservable<Touch[]> InputObservable
            =>_tickableManager.TickStream.Select(x =>Input.touches).Where(x =>Input.touchCount > 0);

    }
}


