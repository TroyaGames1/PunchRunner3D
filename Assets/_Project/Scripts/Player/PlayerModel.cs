using Dreamteck.Splines;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerBehaviors
{
    public class Player
    {

        public Player(Rigidbody rigidBody, Animator animator,SplineFollower splineFollower,Slider slider)
        {
            RigidBody = rigidBody;
            GetAnimator  = animator;
            SplineFollower = splineFollower;
            Slider = slider;
        }


      

        public bool IsDead
        {
            get; set;
        }

        public readonly Animator GetAnimator;



        public Vector3 LookDir => -RigidBody.transform.right;

        public Quaternion Rotation
        {
            get => RigidBody.rotation;
            set => RigidBody.rotation = value;
        }

        public Vector3 Position
        {
            get => RigidBody.gameObject.transform.position;
            set => RigidBody.gameObject.transform.position = value;
        }

        public GameObject GO => RigidBody.gameObject;


        public void AddForce(Vector3 force)
        {
            RigidBody.AddForce(force);
        }

        
        public readonly Rigidbody RigidBody;

        public readonly SplineFollower SplineFollower;

        public readonly Slider Slider;


    }
}
    

