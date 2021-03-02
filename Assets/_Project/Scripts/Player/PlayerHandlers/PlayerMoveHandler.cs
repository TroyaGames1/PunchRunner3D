using Events;
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
        private readonly SignalBus _signal;

        PlayerMoveHandler(Player player,PlayerObservables observables, SignalBus signal)
        {
            _player = player;
            _observables = observables;
            _signal = signal;
        }


        public void Initialize()
        {
            _observables.InputObservable.Subscribe(CheckHorizontalInputs);
            _signal.Subscribe<ISignalChangeSpeed>(x=>ChangeSpeed(x.Speed));
        }

        private void ChangeSpeed(int speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }

    


        public void FixedTick()
        {
            ClampPlayerHorizontalPosition();
        }

        private void ClampPlayerHorizontalPosition()
        {
            var clampPos = Mathf.Clamp(_player.Position.z, -1.30f, 1.30f);
            _player.Position = new Vector3(_player.Position.x, _player.Position.y, clampPos);
            _player.RigidBody.velocity = Vector3.zero;
            _player.RigidBody.angularVelocity = Vector3.zero;
        }
        
        private void CheckHorizontalInputs(Touch[] touches)
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
    }
}

