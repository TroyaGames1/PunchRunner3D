using System;
using Dreamteck.Splines;
using PlayerState;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerFacade : MonoBehaviour
    {

        Player _model;
        private PlayerStateManager _stateManager;

        [Inject]
        public void Construct(Player player,PlayerStateManager stateManager)
        {
            _model = player;
            _stateManager = stateManager;
        }

        public bool IsDead => _model.IsDead;

        public Vector3 Position
        {
            get => _model.Position;
            set => _model.Position = value;
        }

        public Quaternion Rotation => _model.Rotation;

        public GameObject GO => _model.GO;

        public Animator Animator => _model.GetAnimator;


        public Rigidbody Rigidbody => _model.RigidBody;

        public SplineFollower SplineFollower => _model.SplineFollower;

        public Slider HPSlider => _model.Slider;
        
        void OnApplicationQuit()
        {
             _stateManager.CurrentStateAsIstate.ExitState();

        }


        public void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position + 2 * Vector3.up, transform.forward, Color.red, 25);
            Debug.DrawRay(transform.position + Vector3.forward / 5 + 2 * Vector3.up, transform.forward, Color.red, 25);
            Debug.DrawRay(transform.position - Vector3.forward / 5 + 2 * Vector3.up, transform.forward, Color.red, 25);

          
        }
  
    }
}
