using System.Collections;
using System.Collections.Generic;
using CRI.HitBoxTemplate.Serial;
using System.Linq;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    public class TargetsManager : MonoBehaviour
    {
        public ExampleSerialController serialController;

        public GameObject targetPrefab;
        List<GameObject> targetsList = new List<GameObject>();

        public GameObject impactPrefab;

        private Camera _hitboxCamera;
        public Camera _debugCamera;

        private void OnEnable()
        {
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void Awake()
        {
            _hitboxCamera = this.gameObject.GetComponent<Camera>();
            targetsList = new List<GameObject>();
        }

        private void OnImpact(object sender, ImpactPointControlEventArgs e)
        {
            OnImpact(e.impactPosition);
        }

        private void OnImpact(Vector2 position2D_)
        {
            Vector3 position3D_ = new Vector3(position2D_.x, position2D_.y, 200);

            Vector3 cameraForward = _hitboxCamera.transform.forward;
            Debug.DrawRay(position2D_, cameraForward * 10000, Color.yellow, 10.0f);

            bool isTrigger_ = false ;
            Color targetColor_;
            if (targetsList.Count > 0)
            {
                RaycastHit hit;
                if (Physics.Raycast(position2D_, cameraForward, out hit))
                {
                    if (hit.collider != null && hit.transform.tag == "target")
                    {
                        targetColor_ = hit.collider.GetComponent<TargetBehavior>().GetColor();
                        hit.collider.GetComponent<TargetBehavior>().destroyTarget();
                        isTrigger_ = true;
                        Debug.Log("HIT");
                    }
                }
            }
            else {
                isTrigger_ = true;
            }

            if (isTrigger_)
                setCrownTargets(position3D_, 3);

            impactPrefab.transform.position = position3D_;
        }

        private void setCrownTargets(Vector3 position_, int nTargets_) {
            int angle0_ = Random.Range(0, 360);

            for (int i = 0; i < nTargets_; i++)
            {
                targetsList.Add((GameObject)Instantiate(targetPrefab, position_, Quaternion.identity));
                GameObject target_ = targetsList[targetsList.Count-1];
                target_.GetComponent<TargetBehavior>().SetAngleDirection(angle0_ + i * (float)360 / nTargets_);
                target_.GetComponent<TargetBehavior>().SetColor(Color.green);
            }
        }

#if UNITY_EDITOR
        private void OnMouseDown()
        {
            Vector3 mousePosition = Input.mousePosition;
            if (!_debugCamera.orthographic)
                mousePosition.z = this.transform.position.z;
            OnImpact(_debugCamera.ScreenToWorldPoint(mousePosition));
        }
#endif

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown();

            for (int i = 0; i < targetsList.Count; i++) {
                if (targetsList[i] == null)
                    targetsList.RemoveAt(i);
            }

            //Vector3[] accelerations = serialController.accelerations;
            //for (int i = 0; i < accelerations.Length; i++)
            //{
            //    Debug.Log(string.Format("Acceleration Player {0}", accelerations[i]));
            //}
        }
    }
}
