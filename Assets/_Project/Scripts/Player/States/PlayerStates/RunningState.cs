using PlayerBehaviors;
using UnityEngine;

namespace PlayerState
{
    public class RunningState : IState
    {
        private readonly Player _player;

        RunningState(Player player)
        {
            _player = player;
        }

        public void EnterState()
        {
            _player.SplineFollower.followSpeed = 1;
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

