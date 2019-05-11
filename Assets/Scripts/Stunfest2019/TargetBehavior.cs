using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private Vector3 _targetDirection = Vector3.right;   // target direction in world space
    private float _angle = 0.0f;                        // default angle direction
    private float _speedTranslate = 50.0f;              // default translation speed value
    private float _speedRotate = 0.0f;                  // default rotation speed value

    public void SetAngleDirection(float angleDeg_) {
        _angle = angleDeg_ * Mathf.Deg2Rad;             // convert function input in degree to local variable in radian
        _targetDirection = new Vector3(Mathf.Cos(_angle), Mathf.Sin(_angle), 0);
    }

    public void SetTranslationSpeed(float speedTranslate_)
    {
        _speedTranslate = speedTranslate_;
    }

    public void SetRotationSpeed(float speedRotate_)
    {
        _speedRotate = speedRotate_;
    }

    void Update()
    {
        if(_speedRotate != 0.0f)
        {
            this.gameObject.transform.RotateAround(this.gameObject.transform.position, Vector3.forward, _speedRotate * Time.deltaTime);
        }
        this.gameObject.transform.Translate(_speedTranslate *_targetDirection * Time.deltaTime, Space.World);        
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
