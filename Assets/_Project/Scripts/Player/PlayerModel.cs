using Dreamteck.Splines;
using UnityEngine;

namespace PlayerBehaviors
{
    public class Player
    {

        public Player(Rigidbody rigidBody, Animator animator,SplineFollower splineFollower)
        {
            RigidBody = rigidBody;
            GetAnimator  = animator;
            SplineFollower = splineFollower;
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
            get => RigidBody.position;
            set => RigidBody.position = value;
        }


        public void AddForce(Vector3 force)
        {
            RigidBody.AddForce(force);
        }


        public readonly Rigidbody RigidBody;

        public readonly SplineFollower SplineFollower;




    }
}
    

