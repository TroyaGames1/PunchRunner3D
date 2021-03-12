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
    public class PlayerMoveHandler: IInitializable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private readonly SignalBus _signal;
        private readonly PlayerAnimationHandler _animationHandler;
        private Settings _settings;
        public float GetDefaultSplineSpeed => _settings.DefaultSpeed;
        public float GetDefaultMoveFactor => _settings.DefaultMovementSpeedFactor;
        

        private float _lastFrameFingerPositionX;
        private float _moveFactorX;

        private float _swerveAmount;
        private float _newXPos;
        
        private bool _ifTouching;
        
       
        private PlayerMoveHandler(Player player,PlayerObservables observables,
            SignalBus signal,Settings settings, PlayerAnimationHandler animationHandler)        
        {
            _player = player;
            _observables = observables;
            _signal = signal;
            _settings = settings;
            _animationHandler = animationHandler;
        
        }

        public void Initialize()
        {
            _settings.CurrentMovementSpeedFactor = _settings.DefaultMovementSpeedFactor;
            
            _signal.Subscribe<ISignalChangeSpeed>(x=>SignalVoidChangeSplineSpeed(x.SplineSpeed));
            _signal.Subscribe<ISignalChangeMovementSpeedFactor>
                (x=>SignalVoidChangeMovementFactor(x.SpeedFactor));

            _observables.PlayerStateObservable
                .Where(x => x == PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x =>
                {
                    CheckHorizontalInput();
                    MoveHorizontal();
                    ClampPlayerHorizontalPosition();
                });
        }

        private void SignalVoidChangeSplineSpeed(float speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }
        
        private void SignalVoidChangeMovementFactor(float speedFactor)
        {
            _settings.CurrentMovementSpeedFactor = speedFactor;
        }


        private void CheckHorizontalInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
                
                _swerveAmount = _settings.SwerveSpeed * _moveFactorX; 
                _swerveAmount = Mathf.Clamp(_swerveAmount, -_settings.MaxSwerveAmount, 
                    +_settings.MaxSwerveAmount);
                
                _newXPos = _player.GO.transform.localPosition.x + _swerveAmount;
                var difference = _player.GO.transform.localPosition.x - _newXPos;

                _animationHandler.SetFloat("speed",-difference*Time.smoothDeltaTime*3);
                _ifTouching = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0f;
                _ifTouching = false;
            }
        }

        
        private void MoveHorizontal()
        {
            _player.GO.transform.localPosition = new Vector3(Mathf.Lerp(_player.GO.transform.localPosition.x,
                    _newXPos, Time.deltaTime * _settings.CurrentMovementSpeedFactor)
                , _player.GO.transform.localPosition.y, _player.GO.transform.localPosition.z);

            if (!_ifTouching)
            {
                var difference = _player.GO.transform.localPosition.x - _newXPos;
                if (Mathf.Abs(difference) < 0.01f)
                {
                    difference = 0;
                }
                _animationHandler.SetFloat("speed", -difference*Time.smoothDeltaTime*25);
            }
        }

        private void ClampPlayerHorizontalPosition()
        {
            _player.GO.transform.localPosition = new Vector3(Mathf.Clamp(_player.GO.transform.localPosition.x, 
               -2, 2),
                     0,0); 
            _player.RigidBody.velocity = Vector3.zero; 
            _player.RigidBody.angularVelocity = Vector3.zero;
        }
        
        #region TouchInput

        private void CheckHorizontalInputs(Touch[] touches)
        {
            var touch = touches[0];
            
            if (touch.phase==TouchPhase.Began)
            {
                _moveFactorX = 0f;
                _lastFrameFingerPositionX = touch.position.x;
            }
            else if (touch.phase == TouchPhase.Moved|touch.phase == TouchPhase.Stationary)
            {
                _moveFactorX = touch.position.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = touch.position.x;
              
                _swerveAmount = _settings.SwerveSpeed * _moveFactorX; 
                _swerveAmount = Mathf.Clamp(_swerveAmount, -_settings.MaxSwerveAmount, 
                    +_settings.MaxSwerveAmount);

              
                _newXPos = _player.GO.transform.localPosition.x + _swerveAmount;
                var difference = _player.GO.transform.localPosition.x - _newXPos;

                _animationHandler.SetFloat("speed",-difference* 4 *Time.deltaTime);
                _ifTouching = true;
            }
            else if (touch.phase== TouchPhase.Canceled | touch.phase==TouchPhase.Ended)
            {
                _moveFactorX = 0f;
                _ifTouching = false;
            }
        }


        #endregion
      
        
        [Serializable]
        public struct Settings
        {
            [VerticalGroup("GROUP1")]
            [LabelText("Spline Follow Speed")]
            [Range(1,5)]
            public float DefaultSpeed;
            
            [HorizontalGroup("Group 2",0.5f,LabelWidth = 125)]
            [InfoBox("DefaultValue 0.25")]
            [MinValue(0)]
            public float SwerveSpeed; 
            
            [HorizontalGroup("Group 2",0.5f,LabelWidth = 125)]
            [InfoBox("DefaultValue 0.75")]
            [LabelText("Max Swerve")]
            [MinValue(0.75)]
            public float MaxSwerveAmount;
            
         
            [VerticalGroup("Group 3",0.5f),LabelWidth(225)]
            [InfoBox("DefaultValue 3.5")]
            [LabelText("Default Movement SpeedFactor")]
            [MinValue(0.75)]
            public float DefaultMovementSpeedFactor;
            
            [VerticalGroup("Group 3",0.5f),LabelWidth(225)]
            [LabelText("Current Movement SpeedFactor")]
            public float CurrentMovementSpeedFactor;
        }


    }
}

