namespace PlayerBehaviors
{
    using Events;
    using UnityEngine;
    using Zenject;
    using System;

    public class PlayerRaycastHandler
    {
        private readonly SignalBus _signalBus;
        private readonly Player _player;
        private readonly Settings _settings;
        private StateENUM _stateEnum;

        public PlayerRaycastHandler(Player player, SignalBus signalBus, Settings settings)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
        }

        private enum StateENUM
        {
            WALKING,
            PUNCHING
        }
        
        public void CheckRayCasts()
        {
            ChangeState(CanMove ? StateENUM.WALKING : StateENUM.PUNCHING);
            
        }
        
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
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("WALK", 2));
                    break;
                case StateENUM.PUNCHING:
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("PUNCH", 0));
                    break;
            }
        }
        
        #region Raycasts

        private bool CanMove => !RayCastForward&& !RayCastRight && !RayCastLeft;
        private bool RayCastForward=>  Physics.Raycast(_player.Position, 
            _player.GO.transform.forward, 0.25f,_settings.RaycastLayer);
       
        private bool RayCastRight=>Physics.Raycast(_player.Position+Vector3.forward/5, 
            _player.GO.transform.forward, 0.25f,_settings.RaycastLayer);
        private bool RayCastLeft=>Physics.Raycast(_player.Position-Vector3.forward/5, 
            _player.GO.transform.forward, 0.25f,_settings.RaycastLayer);
       

        #endregion
        
        [Serializable]
        public struct Settings
        {
            public LayerMask RaycastLayer;
        }


    }
    

}
