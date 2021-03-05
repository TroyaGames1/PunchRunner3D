using System;
using Dreamteck.Splines;
using Events;
using PlayerState;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{ 
    public class PlayerMoveHandler: IInitializable, ITickable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private readonly SignalBus _signal;
        private readonly Settings _settings;
        public float GetDefaultSpeed => _settings.DefaultSpeed;
        

        private float _lastFrameFingerPositionX;
        private float _moveFactorX;
        private readonly SplineComputer _splineComputer;

        private PlayerMoveHandler(Player player,PlayerObservables observables, SignalBus signal,Settings settings)        
        {
            _player = player;
            _observables = observables;
            _signal = signal;
            _settings = settings;
            _splineComputer = _player.SplineFollower.spline;
        }

     

        public void Initialize()
        {
            _observables.InputObservable.Subscribe(x=>
            {

                CheckHorizontalInputs(x);
                MoveHorizontal();
            });
            _signal.Subscribe<ISignalChangeSpeed>(x=>ChangeSpeed(x.Speed));
        }

        private void ChangeSpeed(float speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }

    


        public void Tick()
        {
            ClampPlayerHorizontalPosition();
        }


        

        private void CheckHorizontalInputs(Touch[] touches)
        {
            var touch = touches[0];
            
            if (touch.phase==TouchPhase.Began)
            {
                _lastFrameFingerPositionX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved|touch.phase == TouchPhase.Stationary)
            {
                _moveFactorX = touch.position.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = touch.position.x;
            }
            else if (touch.phase== TouchPhase.Canceled | touch.phase==TouchPhase.Ended)
            {
                _moveFactorX = 0f;
            }
        }

        private void MoveHorizontal()
        {
            var swerveAmount = Time.deltaTime * _settings.SwerveSpeed * _moveFactorX;
            _player.GetAnimator.SetFloat("speed",swerveAmount);
            swerveAmount = Mathf.Clamp(swerveAmount, -_settings.MaxSwerveAmount, _settings.MaxSwerveAmount);
            _player.GO.transform.Translate(swerveAmount, 0, 0);
        }
        
        private void ClampPlayerHorizontalPosition()
        {
            var splineWorldPos = _splineComputer.EvaluatePosition( _player.SplineFollower.result.percent); 
            var _clampedPosition = Mathf.Clamp(_player.Position.z, splineWorldPos.z-1.50f, splineWorldPos.z+1.50f);
            _player.Position = new Vector3(_player.Position.x, _player.Position.y, _clampedPosition);
            _player.RigidBody.velocity = Vector3.zero;
            _player.RigidBody.angularVelocity = Vector3.zero;
           

        }
        
      
        
        [Serializable]
        public struct Settings
        {
            [VerticalGroup("GROUP1")]
            [Range(1,5)]
            public float DefaultSpeed;
            
            [HorizontalGroup("Group 2",0.5f,LabelWidth = 125)]
            [InfoBox("DefaultValue 0.25")]
            [MinValue(0.25)]
            public float SwerveSpeed; 
            
            [HorizontalGroup("Group 2",0.5f,LabelWidth = 125)]
            [InfoBox("DefaultValue 0.75")]
            [LabelText("Max Swerve")]
            [MinValue(0.75)]
            public float MaxSwerveAmount;
        }
    }
}

