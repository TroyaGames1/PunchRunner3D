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

        private StateENUM _stateEnum;


        private bool _canCheckRaycast;

        private enum StateENUM
        {
            WALKING,
            PUNCHING
        }
      

        public PlayerRaycastHandler(Player player, SignalBus signalBus, Settings settings, TickableManager tickableManager)
        {
            _player = player;
            _signalBus = signalBus;
            _settings = settings;
            _tickableManager = tickableManager;

         
        }
        
        public void Initialize()
        {
            
            _tickableManager.TickStream.Where(x => _canCheckRaycast).Subscribe(x =>
            {
                CheckRayCasts();
            });
            _signalBus.Subscribe<SignalStartRaycasting>(() => _canCheckRaycast=true);
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
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("WALK", 2));
                    _canCheckRaycast = false;
                    break;
                case StateENUM.PUNCHING:
                    _signalBus.AbstractFire(new SignalChangeSpeedAndAnimation("PUNCH", 0));
                    break;
            }
        }
        
        #region Raycasts

        private bool CanMove => !RayCastForward&& !RayCastRight && !RayCastLeft;
        private bool RayCastForward=>  Physics.Raycast(_player.Position, 
            _player.GO.transform.forward, 0.3f,_settings.RaycastLayer);
       
        private bool RayCastRight=>Physics.Raycast(_player.Position+Vector3.forward/5, 
            _player.GO.transform.forward, 0.3f,_settings.RaycastLayer);
        private bool RayCastLeft=>Physics.Raycast(_player.Position-Vector3.forward/5, 
            _player.GO.transform.forward, 0.3f,_settings.RaycastLayer);
       

        #endregion
        
        [Serializable]
        public struct Settings
        {
            public LayerMask RaycastLayer;
        }


    
    }
    

}
