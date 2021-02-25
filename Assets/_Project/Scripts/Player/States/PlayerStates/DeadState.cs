using UnityEngine;

namespace PlayerState
{
    public class DeadState : IState
    {
        public void EnterState()
        {

            Debug.Log("PlayerDeadStateEnter");
        }

        public void ExitState()
        {
            Debug.Log("PlayerDeadStateExit");
        }

        public void Update()
        {
        }

        public void FixedUpdate()
        {
        }
    }
}

