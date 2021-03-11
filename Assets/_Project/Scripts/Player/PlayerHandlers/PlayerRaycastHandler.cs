using System.Collections.Generic;
using PlayerState;
using Sirenix.OdinInspector;

namespace PlayerBehaviors
{
    using Events;
    using UnityEngine;
    using Zenject;
    using System;
    using UniRx;

    public class PlayerRaycastHandler: IInitializable
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;
        private readonly Settings _settings;
        private readonly TickableManager _tickableManager;
        private readonly PlayerMoveHandler _moveHandler;
        private readonly PlayerStateManager _stateManager;
        private readonly PlayerObservables _observables;


        private StateENUM _stateEnum;
        private bool _canCheckRaycast;
        private RaycastHit _hit;

        private enum StateENUM
        {
            WALKING,
            PUNCHING
        }
      

        public PlayerRaycastHandler(Player player, SignalBus signalBus, Settings settings,
            TickableManager tickableManager, PlayerMoveHandler moveHandler, PlayerStateManager stateManager, PlayerObservables observables)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
            _tickableManager = tickableManager;
            _moveHandler = moveHandler;
            _stateManager = stateManager;
            _observables = observables;
        }
        
        public void Initialize()
        {
            _observables.PlayerTriggerEnterObservable.Where(x => !x.CompareTag("Obstacle"))
                .Subscribe(x => _canCheckRaycast = false);
            _observables.PlayerCollisionEnterObservable.Where(x => !x.collider.CompareTag("Obstacle"))
                .Subscribe(x => _canCheckRaycast = false);
            
            _tickableManager.TickStream
                .Where(x => _canCheckRaycast&&
                            _stateManager.CurrentState==PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x =>
            {
                CheckRayCasts();
            });
            
            
            
            _signalBus.Subscribe<SignalStartRaycasting>(() =>
            {
                if (_canCheckRaycast==false)
                {
                    _canCheckRaycast = true;
                }
            });
        }
        
       


        private void CheckRayCasts()=> ChangeState(CanMove ? StateENUM.WALKING : StateENUM.PUNCHING);
        
        private void ChangeState(StateENUM stateEnum)
        {
            if (_stateEnum == stateEnum) return;
            _stateEnum = stateEnum;
            CheckState();
        }

        private void CheckState()
        {
            switch (_stateEnum)
            {
                case StateENUM.WALKING:
                    _signalBus.AbstractFire(
                    new SignalChangeSpeedMovementFactorAndAnimation("WALK", 
                        _moveHandler.GetDefaultSplineSpeed,_moveHandler.GetDefaultMoveFactor));
                    
                    break;
                case StateENUM.PUNCHING:
                    _signalBus.AbstractFire(new SignalChangeSpeedMovementFactorAndAnimation("PUNCH", 0,2));
                    break;
            }
        }

      
        
        
        #region Raycasts

        private bool CanMove => !RayCastForward&& !RayCastRight && !RayCastLeft;
        private bool RayCastForward =>
            Physics.Raycast(_player.Position + 1 * Vector3.up,
                _player.GO.transform.forward, out _hit,0.7f, _settings.RaycastLayer);

        private bool RayCastRight=>Physics.Raycast(_player.Position+Vector3.forward/6 +1*Vector3.up, 
            _player.GO.transform.forward, 0.7f,_settings.RaycastLayer);
        private bool RayCastLeft=>Physics.Raycast(_player.Position-Vector3.forward/6 +1*Vector3.up, 
            _player.GO.transform.forward, 0.7f,_settings.RaycastLayer);
       

        #endregion
        
        [Serializable]
        public struct Settings
        {
            public LayerMask RaycastLayer;
        }


    }
    

}
