using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetBehavior : MonoBehaviour
{
    private Vector3 _targetDirection = Vector3.forward;
    private float _angle = 0.0f;          // default angle direction
    private float _speedTranslate = 1.0f; // default translation speed value
    private float _speedRotate = 0.0f;    // default rotation speed value

    public void setAngleDirection(float angle_) {
        _angle = angle_;
    }

    public void setTranslationSpeed(float speedTranslate_)
    {
        _speedTranslate = speedTranslate_;
    }

    public void setRotationSpeed(float speedRotate_)
    {
        _speedRotate = speedRotate_;
    }

    // Update is called once per frame
    void Update()
    {
        //this.gameObject.transform.Translate(this.gameObject.transform.position.x);
        this.gameObject.transform.RotateAround(this.gameObject.transform.position, Vector3.forward, 20 * Time.deltaTime);
    }

    void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
