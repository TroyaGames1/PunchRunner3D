using System;
using DG.Tweening;
using Dreamteck.Splines;
using Events;
using PlayerState;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{ 
    public class PlayerMoveHandler: IInitializable, ITickable, IFixedTickable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private readonly SignalBus _signal;
        private PlayerAnimationHandler _animationHandler;
        private readonly Settings _settings;
        public float GetDefaultSpeed => _settings.DefaultSpeed;
        

        private float _lastFrameFingerPositionX;
        private float _moveFactorX;
        private readonly SplineComputer _splineComputer;

        private float swerveAmount;
        private float newVector;


        private bool _ifMoving = false;
        private PlayerMoveHandler(Player player,PlayerObservables observables, SignalBus signal,Settings settings, PlayerAnimationHandler animationHandler)        
        {
            _player = player;
            _observables = observables;
            _signal = signal;
            _settings = settings;
            _animationHandler = animationHandler;
            _splineComputer = _player.SplineFollower.spline;
        }

     

        public void Initialize()
        {
            _observables.InputObservable.Subscribe(x=>
            {

                CheckHorizontalInputs(x);
                
            });
            _signal.Subscribe<ISignalChangeSpeed>(x=>ChangeSpeed(x.Speed));
        }

        private void ChangeSpeed(float speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }

    


        public void Tick()
        {
            MoveHorizontal();
        }
        public void FixedTick()
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
            

                 swerveAmount = _settings.SwerveSpeed * _moveFactorX;
                swerveAmount = Mathf.Clamp(swerveAmount, -_settings.MaxSwerveAmount, _settings.MaxSwerveAmount);
                 newVector = _player.GO.transform.position.z - swerveAmount;
                 
                 _animationHandler.SetFloat("speed",swerveAmount*Time.deltaTime);
                 
                 _ifMoving = true;

            }
            else if (touch.phase== TouchPhase.Canceled | touch.phase==TouchPhase.Ended)
            {
                _moveFactorX = 0f;
                _ifMoving = false;
            }
        }

        private void MoveHorizontal()
        {
            _player.GO.transform.position = new Vector3(_player.GO.transform.position.x, _player.GO.transform.position.y,
                Mathf.Lerp(_player.GO.transform.position.z,newVector,Time.deltaTime*3.5f));


            if (!_ifMoving)
            {
                var difference = _player.GO.transform.position.z - newVector;
                
                if (Mathf.Abs(difference) < 0.02f)
                {
                    difference = 0;
                }
                _animationHandler.SetFloat("speed",difference/15f);
            }

        }
        
        private void ClampPlayerHorizontalPosition()
        {
           var splineWorldPos = _splineComputer.EvaluatePosition( _player.SplineFollower.result.percent);
           var clampedNegative = splineWorldPos.z - 1.50f; 
           var clampedPositive = splineWorldPos.z + 1.50f;
           var clampedPosition = Mathf.Clamp(_player.Position.z, clampedNegative, clampedPositive);

           newVector=Mathf.Clamp(newVector,clampedNegative,clampedPositive); 
           
          _player.Position = new Vector3(_player.Position.x, _player.Position.y, clampedPosition); 
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
            [MinValue(0.15)]
            public float SwerveSpeed; 
            
            [HorizontalGroup("Group 2",0.5f,LabelWidth = 125)]
            [InfoBox("DefaultValue 0.75")]
            [LabelText("Max Swerve")]
            [MinValue(0.75)]
            public float MaxSwerveAmount;
        }

      
    }
}

