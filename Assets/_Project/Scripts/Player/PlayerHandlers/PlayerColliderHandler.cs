using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerColliderHandler: IInitializable,IFixedTickable
    {
        private readonly PlayerObservables _observables;
        private readonly Player _player;
        private readonly TickableManager _tickableManager;
        private Settings _settings;
        private StateENUM _stateEnum;

        private PlayerColliderHandler(PlayerObservables playerObservables,Player player, TickableManager tickableManager, Settings settings)
        {
            _observables = playerObservables;
            _player = player;
            _tickableManager = tickableManager;
            _settings = settings;
        }


        public void FixedTick()
        {
           // _player.SplineFollower.followSpeed = 2;
            //_player.GetAnimator.Play("WALK");
            //CheckRayCast();
        }

        private void CheckRayCast()
        {
            if (!Physics.Raycast(_player.Position, _player.GO.transform.forward, 5f,_settings.Layer))
            {
                _player.GetAnimator.Play("WALK");
            }
            

        }

        private void ChangeAndCheckState()
        {
           
            switch (_stateEnum)
            {
                case StateENUM.WALK:
                    _player.SplineFollower.followSpeed = 2;
                    _player.GetAnimator.Play("WALK");
                    break;
                case StateENUM.HIT:
                    _player.SplineFollower.followSpeed = 0;
                    _player.GetAnimator.Play("PUNCH");
                    break;
                
            }
        }

        public void Initialize()
        {
           // _observables.PlayerCollisionEnterObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
           //     .Subscribe(x =>
           //     {
           //         _player.SplineFollower.followSpeed = 0;
           //         _player.GetAnimator.Play("PUNCH");
           //     }); 
            _observables.PlayerTriggerStayObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
                .Subscribe(x =>
                {
                    _stateEnum = StateENUM.EXIT;
                    _player.SplineFollower.followSpeed = 0;
                    _player.GetAnimator.Play("PUNCH");
                });
            _observables.PlayerTriggerExitObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
                .Subscribe(x =>
                {
                    _stateEnum = StateENUM.EXIT;
                    _player.SplineFollower.followSpeed = 1;
                    _player.GetAnimator.Play("WALK");
                });
            
            
           // _observables.PlayerCollisionEnterObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
           //     .Subscribe(x =>
           //     {
//
           //     });
           // _observables.PlayerTriggerExitObservable.Where(x => x.gameObject.CompareTag("Obstacle/Trigger"))
           //     .Subscribe(x =>
           //     {
           //       //  _stateEnum = StateENUM.EXIT;
           //         
           //     });
            
           _tickableManager.TickStream.Select(x => _stateEnum)
               .Where(x => x == StateENUM.EXIT)
               .Subscribe(x => CheckRayCast());
        }
        
        private enum StateENUM
        {
            WALK,
            HIT,
            EXIT
        }
        
        [Serializable]
        public struct Settings
        {
            public LayerMask Layer;
        }

      
    }

}

