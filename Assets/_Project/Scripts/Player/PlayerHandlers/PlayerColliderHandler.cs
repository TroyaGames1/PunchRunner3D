using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler: IInitializable
    {
        private readonly PlayerObservables _observables;
        private readonly Player _player;
        private readonly TickableManager _tickableManager;
        private readonly Settings _settings;
        private StateENUM _stateEnum;
        
        private enum StateENUM
        {
            WALK,
            INTRIGGER
        }

        private PlayerColliderHandler(PlayerObservables playerObservables,Player player, TickableManager tickableManager, Settings settings)
        {
            _observables = playerObservables;
            _player = player;
            _tickableManager = tickableManager;
            _settings = settings;
        }

        public void Initialize()
        {
          
         //   _observables.PlayerTriggerStayObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
         //       .Subscribe(x =>
         //       {
         //           ChangeState(StateENUM.INTRIGGER);
         //       });
         //  
            
            _tickableManager.TickStream
                .Subscribe(x =>
                {
                    CheckRayCast();
                }).AddTo(_player.GO);
        }

       

        private void CheckRayCast()
        {
            ChangeState(CanMove ? StateENUM.WALK : StateENUM.INTRIGGER);
        }
        

        private void CheckState()
        {
           
            switch (_stateEnum)
            {
                case StateENUM.WALK:
                    _player.SplineFollower.followSpeed = 2;
                    _player.GetAnimator.Play("WALK");
                    break;
                case StateENUM.INTRIGGER:
                    _player.SplineFollower.followSpeed = 0;
                    _player.GetAnimator.Play("PUNCH");
                    break;
                
            }
        }

     
        private void ChangeState(StateENUM stateEnum)
        {
            if (_stateEnum == stateEnum) return;
            _stateEnum = stateEnum;
            CheckState();
        }
        
        
        
        #region Raycasts

        private bool CanMove => !RayCastForward&& !RayCastRight && !RayCastLeft;
        private bool RayCastForward=>  Physics.Raycast(_player.Position, 
            _player.GO.transform.forward, 0.25f,_settings.Layer);
       
        private bool RayCastRight=>Physics.Raycast(_player.Position+Vector3.forward/5, 
            _player.GO.transform.forward, 0.25f,_settings.Layer);
        private bool RayCastLeft=>Physics.Raycast(_player.Position-Vector3.forward/5, 
            _player.GO.transform.forward, 0.25f,_settings.Layer);
       

        #endregion
        
        [Serializable]
        public struct Settings
        {
            public LayerMask Layer;
        }

      
    }

}

