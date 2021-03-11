using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;

public class CinemachineClamper : MonoBehaviour
{

    [SerializeField] private SplineComputer _splineComputer;
    [SerializeField] private SplineFollower _splineFollower;


    [SerializeField] private Vector3 PositionOffset;
    [SerializeField] private Vector3 RotationOffset;
    [SerializeField] private float Damp;
       
        private void LateUpdate()
        {
         
            var splineWorldPos = _splineComputer.EvaluatePosition( _splineFollower.result.percent);
            var splineRotation = _splineFollower.transform.eulerAngles.y ;

            var _cameraPos = splineWorldPos - _splineFollower.transform.forward * PositionOffset.x;
            _cameraPos.y = PositionOffset.y;

                            transform.position = Vector3.Lerp(transform.position,_cameraPos,Time.deltaTime*Damp);
           var currentRotation= Mathf.Lerp(transform.localEulerAngles.y,splineRotation+RotationOffset.y,Time.deltaTime*Damp);
           transform.localEulerAngles = new Vector3(RotationOffset.x, currentRotation, RotationOffset.z);
        }

    
}
