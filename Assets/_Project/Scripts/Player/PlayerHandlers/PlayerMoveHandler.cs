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
        private float newVector;


        private bool _ifMoving = false;

        private MoveEnum _moveEnum;
       public enum MoveEnum
        {
            MOVEZ,
            MOVEX
        }
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
            _observables.InputObservable.Where(x=>_stateManager.CurrentState==PlayerStateManager.PlayerStates.RunningState).Subscribe(CheckHorizontalInputs);
            _signal.Subscribe<ISignalChangeSpeed>(x=>ChangeSpeed(x.Speed));
            _signal.Subscribe<SignalChangeAxis>(x=>ChangeAxis(x._moveEnum));

            _observables.PlayerStateObservable.Where(x => x == PlayerStateManager.PlayerStates.RunningState)
                .Subscribe(x => MoveHorizontal());

        }

        private void ChangeSpeed(float speed)
        {
            _player.SplineFollower.followSpeed = speed;
        }

        void ChangeAxis(MoveEnum moveEnum)
        {
            _moveEnum = moveEnum;
            switch (moveEnum)
            {
                case MoveEnum.MOVEX:
                    _player.SplineFollower.motion.applyPositionX = true;
                    _player.SplineFollower.motion.applyPositionZ = true;
                break;
                case MoveEnum.MOVEZ:
                    _player.SplineFollower.motion.applyPositionZ = false;
                    _player.SplineFollower.motion.applyPositionX = true;
                    break;
            }
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
                swerveAmount = Mathf.Clamp(swerveAmount, -_settings.MaxSwerveAmount, 
                    _settings.MaxSwerveAmount);

                switch (_moveEnum)
                {
                    case MoveEnum.MOVEX:
                        newVector = _player.GO.transform.position.x - swerveAmount;
                        break;
                    case MoveEnum.MOVEZ:
                        newVector = _player.GO.transform.position.z - swerveAmount;
                        break;
                }

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
            
            switch (_moveEnum)
            {
                case MoveEnum.MOVEX:
                    _player.GO.transform.position = new Vector3(Mathf.Lerp(_player.GO.transform.position.x,
                            newVector,Time.deltaTime*3.5f)
                        , _player.GO.transform.position.y,_player.GO.transform.position.z);
                    
                    if (!_ifMoving)
                    {
                        var difference = _player.GO.transform.position.x - newVector;
                
                        if (Mathf.Abs(difference) < 0.02f)
                        {
                            difference = 0;
                        }
                        _animationHandler.SetFloat("speed",difference/15f);
                    }
                    break;
                case MoveEnum.MOVEZ:
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
                    break;
            }
            

        }
        
        private void ClampPlayerHorizontalPosition()
        {
           var splineWorldPos = _splineComputer.EvaluatePosition( _player.SplineFollower.result.percent);

           switch (_moveEnum)
           {
               case MoveEnum.MOVEX:
                   var clampedNegativeX = splineWorldPos.x - 1.50f; 
                   var clampedPositiveX = splineWorldPos.x + 1.50f;
                   var clampedPositionX = Mathf.Clamp(_player.Position.x, clampedNegativeX, clampedPositiveX);
                   newVector=Mathf.Clamp(newVector,clampedNegativeX,clampedPositiveX);
                   _player.Position = new Vector3(clampedPositionX, _player.Position.y, _player.Position.z); 
               break;
               case MoveEnum.MOVEZ:
                   var clampedNegativeZ = splineWorldPos.z - 1.50f; 
                   var clampedPositiveZ = splineWorldPos.z + 1.50f;
                   var clampedPositionZ = Mathf.Clamp(_player.Position.z, clampedNegativeZ,
                       clampedPositiveZ);
                   newVector=Mathf.Clamp(newVector,clampedNegativeZ,clampedPositiveZ);
              //     _player.Position = new Vector3(_player.Position.x, _player.Position.y, clampedPositionZ); 
                   break;
           }
           
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

