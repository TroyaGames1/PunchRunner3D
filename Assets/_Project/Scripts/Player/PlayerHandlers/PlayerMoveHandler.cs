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
    public class PlayerMoveHandler: IInitializable, IFixedTickable
    {
        private readonly Player _player;
        private readonly PlayerObservables _observables;
        private readonly SignalBus _signal;
        private readonly PlayerAnimationHandler _animationHandler;
        private readonly Settings _settings;
        private PlayerStateManager _stateManager;
        public float GetDefaultSpeed => _settings.DefaultSpeed;
        

        private float _lastFrameFingerPositionX;
        private float _moveFactorX;
        private readonly SplineComputer _splineComputer;

        private float swerveAmount;
        private float newXPos;
        
        private bool _ifMoving = false;
        
       
        private PlayerMoveHandler(Player player,PlayerObservables observables, SignalBus signal,Settings settings, PlayerAnimationHandler animationHandler, PlayerStateManager stateManager)        
        {
            _player = player;
            _observables = observables;
            _signal = signal;
            _settings = settings;
            _animationHandler = animationHandler;
            _stateManager = stateManager;
            _splineComputer = _player.SplineFollower.spline;
            
        }

        public void Initialize()
        {
            _observables.InputObservable.Where(x=>_stateManager.CurrentState==PlayerStateManager.PlayerStates.RunningState)
                .Subscribe();
            _signal.Subscribe<ISignalChangeSpeed>(x=>ChangeSpeed(x.Speed));

            _observables.PlayerStateObservable.Where(x => x == PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x =>
                {
                    MoveHorizontal();
                    CheckHorizontalInput();
                });
        }

        private void ChangeSpeed(float speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }

        public void FixedTick()
        {
            ClampPlayerHorizontalPosition();
        }   
        
     

        void CheckHorizontalInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _lastFrameFingerPositionX = Input.mousePosition.x;
            }
            else if (Input.GetMouseButton(0))
            {
                _moveFactorX = Input.mousePosition.x - _lastFrameFingerPositionX;
                _lastFrameFingerPositionX = Input.mousePosition.x;
                
                swerveAmount = _settings.SwerveSpeed * _moveFactorX; 
                swerveAmount = Mathf.Clamp(swerveAmount, -_settings.MaxSwerveAmount, 
                    +_settings.MaxSwerveAmount);

              
                newXPos = _player.GO.transform.localPosition.x + swerveAmount;
                var difference = _player.GO.transform.localPosition.x - newXPos;

                _animationHandler.SetFloat("speed",-difference* 10 *Time.deltaTime);
                _ifMoving = true;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                _moveFactorX = 0f;
                _ifMoving = false;
            }
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
              
                swerveAmount = _settings.SwerveSpeed * _moveFactorX; 
                swerveAmount = Mathf.Clamp(swerveAmount, -_settings.MaxSwerveAmount, 
                    +_settings.MaxSwerveAmount);

              
                newXPos = _player.GO.transform.localPosition.x + swerveAmount;
                var difference = _player.GO.transform.localPosition.x - newXPos;

                _animationHandler.SetFloat("speed",-difference* 4 *Time.deltaTime);
                _ifMoving = true;
            }
            else if (touch.phase== TouchPhase.Canceled | touch.phase==TouchPhase.Ended)
            {
                _moveFactorX = 0f;
                _ifMoving = false;
            }
        }


        #endregion
     
        private void MoveHorizontal()
        {

            _player.GO.transform.localPosition = new Vector3(Mathf.Lerp(_player.GO.transform.localPosition.x,
                    newXPos, Time.deltaTime * 3.5f)
                , _player.GO.transform.localPosition.y, _player.GO.transform.localPosition.z);

            if (!_ifMoving)
            {
                var difference = _player.GO.transform.localPosition.x - newXPos;
                if (Mathf.Abs(difference) < 0.05f)
                {
                    difference = 0;
                }
                _animationHandler.SetFloat("speed", -difference / 2.5F);
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
        
      
        
        [Serializable]
        public struct Settings
        {
            [VerticalGroup("GROUP1")]
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
        }


        
    }
}

