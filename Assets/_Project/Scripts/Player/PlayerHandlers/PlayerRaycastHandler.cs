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

       
        private StateENUM _stateEnum;
        private bool _canCheckRaycast;

        private enum StateENUM
        {
            WALKING,
            PUNCHING
        }
      

        public PlayerRaycastHandler(Player player, SignalBus signalBus, Settings settings,
            TickableManager tickableManager, PlayerMoveHandler moveHandler, PlayerStateManager stateManager)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
            _tickableManager = tickableManager;
            _moveHandler = moveHandler;
            _stateManager = stateManager;
            
        }
        
        public void Initialize()
        {
            _tickableManager.TickStream
                .Where(x => _canCheckRaycast&&
                            _stateManager.CurrentState==PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x =>
            {
                CheckRayCasts();
            });
            
            _signalBus.Subscribe<SignalStartRaycasting>(() =>
            {
                _canCheckRaycast = true;
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
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("WALK", _moveHandler.GetDefaultSpeed));
                    _canCheckRaycast = false;
                    break;
                case StateENUM.PUNCHING:
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("PUNCH", 0));
                    break;
            }
        }

      
        
        
        #region Raycasts

        private bool CanMove => !RayCastForward&& !RayCastRight && !RayCastLeft;
        private bool RayCastForward=>  Physics.Raycast(_player.Position+ 1*Vector3.up, 
            _player.GO.transform.forward , 0.3f,_settings.RaycastLayer);
       
        private bool RayCastRight=>Physics.Raycast(_player.Position+Vector3.forward/6 +1*Vector3.up, 
            _player.GO.transform.forward, 0.3f,_settings.RaycastLayer);
        private bool RayCastLeft=>Physics.Raycast(_player.Position-Vector3.forward/6 +1*Vector3.up, 
            _player.GO.transform.forward, 0.3f,_settings.RaycastLayer);
       

        #endregion
        
        [Serializable]
        public struct Settings
        {
            public LayerMask RaycastLayer;
        }


    
    }
    

}
