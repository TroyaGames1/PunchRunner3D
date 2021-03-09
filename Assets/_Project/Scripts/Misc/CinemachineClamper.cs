using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;

public class CinemachineClamper : MonoBehaviour
{

    [SerializeField] private SplineComputer _splineComputer;
    [SerializeField] private SplineFollower _splineFollower;

    [SerializeField] private Vector3 _newPos;
    

    private Vector3 _vector3;
    
    public float _offset;
    public Vector3 _pos;
    public Vector3 _newTest;
    public float damp;
       
        private void LateUpdate()
        {
         
            var splineWorldPos = _splineComputer.EvaluatePosition( _splineFollower.result.percent);
            var splineRotation = _splineFollower.transform.eulerAngles.y ;

            _pos = splineWorldPos - _splineFollower.transform.forward * _offset;
            _pos.y = 4;
            // var newVec=new Vector3(_pos.x, _pos.y, _player.transform.position.z + _offset);
            transform.position = Vector3.Lerp(transform.position,_pos,Time.deltaTime*damp);
           var currentRotation= Mathf.Lerp(transform.localEulerAngles.y,splineRotation,Time.deltaTime*damp);
           transform.localEulerAngles = new Vector3(0, currentRotation, 0);
        }

        public void SetNewPos()
        {
          //  _pos = _newPos;
        }
}
