using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Dreamteck.Splines;
using UnityEngine;

public class CinemachineClamper : MonoBehaviour
{

    [SerializeField] private CinemachineVirtualCamera newcam;
    [SerializeField] private SplineComputer _splineComputer;
    [SerializeField] private SplineFollower _splineFollower;
  
    
    
    public float _offset;
    public Vector3 _pos;
    public float damp;
       
        private void LateUpdate()
        {
            var splineWorldPos = _splineComputer.EvaluatePosition( _splineFollower.result.percent);
           // var newVec=new Vector3(_pos.x, _pos.y, _player.transform.position.z + _offset);
            newcam.transform.position = Vector3.Lerp(transform.position,splineWorldPos+_pos,Time.deltaTime*damp);
        }
}
