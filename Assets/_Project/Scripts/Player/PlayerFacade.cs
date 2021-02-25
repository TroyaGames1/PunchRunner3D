using UnityEngine;
using Zenject;

namespace PlayerBehaviors
{
    public class PlayerFacade : MonoBehaviour
    {
   
        Player _model;
        
        [Inject]
        public void Construct(Player player)
        {
            _model = player;
        }

        public bool IsDead => _model.IsDead;

        public Vector3 Position
        {
            get => _model.Position;
            set =>_model.Position=value;
        }

        public Quaternion Rotation => _model.Rotation;


        public Animator Animator => _model.GetAnimator;


        public Rigidbody Rigidbody => _model.RigidBody;
        

    }


}
