using PlayerBehaviors;

namespace PlayerState
{
    public class RunningState : IState
    {
        private readonly Player _player;
        private readonly PlayerMoveHandler _moveHandler;

        RunningState(Player player, PlayerMoveHandler moveHandler)
        {
            _player = player;
            _moveHandler = moveHandler;
        }

        public void EnterState()
        {
            _player.SplineFollower.followSpeed = _moveHandler.GetDefaultSplineSpeed;
            _player.GetAnimator.Play("WALK");

        }

        public void ExitState()
        {
            _player.SplineFollower.followSpeed = 0;

        }

   

        public void FixedUpdate()
        {
        }

        public void Update()
        {
         
        }
        
       
    }

}

