using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace PlayerState
{
    public class PlayerStateManager :ITickable, IFixedTickable, IInitializable
    { 
      
        public enum PlayerStates
        {
            IdleState,
            RunningState,
            DeadState,
            FinishState,
            None
        }
    
        
        
        //GameState
        IState _currentStateHandler; 
        PlayerStates _currentState = PlayerStates.None;
        List<IState> _states;
        //
        
    
        
        [Inject]
        public void Construct(
            IdleState ıdle, RunningState running,DeadState dead,FinishState finish) 
        {
            //  _view = view;
            _states = new List<IState>
            {
                ıdle,running,dead,finish
            };
        }
        
        public PlayerStates CurrentState => _currentState;

        public void ChangeState(PlayerStates state)
        {
            if (_currentState == state)
            {
                return;
            }
            
            _currentState = state;
    
            if (_currentStateHandler != null)
            {
                _currentStateHandler.ExitState();
                _currentStateHandler = null;
            }
            _currentStateHandler = _states[(int)state];
            _currentStateHandler.EnterState();
        }
    
        public void Tick()
        {
           
            _currentStateHandler.Update();
    
          
        }
    
        public void FixedTick()
        {
            _currentStateHandler.FixedUpdate();
        }
    
        public void Initialize()
        {
            Application.targetFrameRate = 60;
            ChangeState(PlayerStates.IdleState);
        }
    }
}
