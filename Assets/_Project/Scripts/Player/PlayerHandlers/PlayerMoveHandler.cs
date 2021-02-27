using PlayerState;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{ 
    public class PlayerMoveHandler: IInitializable, IFixedTickable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private TickableManager _tickableManager;

        PlayerMoveHandler(Player player,PlayerObservables observables, TickableManager tickableManager)
        {
            _player = player;
            _observables = observables;
            _tickableManager = tickableManager;
        }


        public void Initialize()
        {
            _observables.InputObservable.Subscribe(CheckInputs);
          
         // _observables.PlayerStateoObservable.Where(x=>x== PlayerStateManager.PlayerStates.RunningState)
         //     .Subscribe(x=>  {
         //             {
         //                 _player.SplineFollower.followSpeed = 2;
         //             }
         //         }
         //     );


        }
        
   

        void CheckInputs(Touch[] touches)
        {
            var touch = touches[0];

            switch (touch.phase)
            {
            
                case TouchPhase.Moved:
                    _player.Position = new Vector3(_player.Position.x,
                        _player.Position.y , _player.Position.z - touch.deltaPosition.x *Time.deltaTime*0.1f);
                    break;
                
            }
        }


        public void FixedTick()
        {
            var clampPos = Mathf.Clamp(_player.Position.z, -1.30f, 1.30f);
            _player.Position = new Vector3(_player.Position.x, _player.Position.y, clampPos);
            _player.RigidBody.velocity=Vector3.zero;
            _player.RigidBody.angularVelocity=Vector3.zero;
        }
    }
}

