using System.Collections;
using System.Collections.Generic;
using CRI.HitBoxTemplate.Serial;
using System.Linq;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    struct TargetProperties
    {
        private float _rotSpeed;
        private float _transSpeed;
        private float _lifeTime;
        private float _scale;

        public float rotSpeed
        {
            set {
                _rotSpeed = value ;
            }
            get{
                return _rotSpeed;
            }
        }

        public float transSpeed
        {
            set
            {
                _transSpeed = value;
            }
            get
            {
                return _transSpeed;
            }
        }

        public float lifeTime
        {
            set
            {
                _lifeTime = value;
            }
            get
            {
                return _lifeTime;
            }
        }

        public float scale
        {
            set
            {
                _scale = value;
            }
            get
            {
                return _scale;
            }
        }

        public TargetProperties(float lifeTime_, float scale_, float transSpeed_, float rotSpeed_)
        { _lifeTime = lifeTime_; _scale = scale_; _transSpeed = transSpeed_; _rotSpeed = rotSpeed_; }
    }

    public class TargetsManager : MonoBehaviour
    {
        public ExampleSerialController serialController;

        public GameObject targetPrefab;
        List<GameObject> targetsList = new List<GameObject>();

        private TargetProperties _targetPropLvl1 = new TargetProperties(5.0f, 20.0f, 100.0f, 100.0f);
        private TargetProperties _targetPropLvl2 = new TargetProperties(3.0f, 30.0f, 150.0f, 200.0f);

        public GameObject impact; // prefab to show where the impacts are detected
        private int delayOffHit = 100 ;
        private long timerOffHit0 = 0;

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
            Color targetColor_ = Color.black;
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
                    }
                }
            }
            else {
                isTrigger_ = true;
            }

            if (isTrigger_)
            {
                Color[] colTargets_;

                if (targetsList.Count == 0)
                {
                    colTargets_ = new Color[3];
                    colTargets_[0] = Color.red;
                    colTargets_[1] = Color.green;
                    colTargets_[2] = Color.blue;
                    SetCrownTargets(position3D_, colTargets_, _targetPropLvl1);
                }
                else
                {
                    if (targetColor_ == Color.red)
                    {
                        colTargets_ = new Color[2];
                        colTargets_[0] = Color.magenta;
                        colTargets_[1] = Color.yellow;
                        SetCrownTargets(position3D_, colTargets_, _targetPropLvl2);
                    }

                    if (targetColor_ == Color.green)
                    {
                        colTargets_ = new Color[2];
                        colTargets_[0] = Color.yellow;
                        colTargets_[1] = Color.cyan;
                        SetCrownTargets(position3D_, colTargets_, _targetPropLvl2);
                    }

                    if (targetColor_ == Color.blue)
                    {
                        colTargets_ = new Color[2];
                        colTargets_[0] = Color.cyan;
                        colTargets_[1] = Color.magenta;
                        SetCrownTargets(position3D_, colTargets_, _targetPropLvl2);
                    }
                }
            }

            impact.transform.position = new Vector3(position3D_.x, position3D_.y, 800); // display a mark where impacts are detected
        }

        //private void SetCrownTargets(Vector3 position_, Color[] colTargets_, float speedTrans_, float speedRot_)
        //{
        //    int angle0_ = Random.Range(0, 360);
        //    int nTargets_ = colTargets_.Length;

        //    for (int i = 0; i < nTargets_ ; i++)
        //    {
        //        targetsList.Add((GameObject)Instantiate(targetPrefab, position_, Quaternion.identity));
        //        GameObject target_ = targetsList[targetsList.Count - 1];
        //        target_.GetComponent<TargetBehavior>().SetAngleDirection(angle0_ + i * (float)360 / nTargets_);
        //        target_.GetComponent<TargetBehavior>().SetColor(colTargets_[i]);
        //        target_.GetComponent<TargetBehavior>().SetTranslationSpeed(speedTrans_);
        //        target_.GetComponent<TargetBehavior>().SetRotationAxis(position_);
        //        target_.GetComponent<TargetBehavior>().SetRotationSpeed(speedRot_);
        //    }
        //}

        private void SetCrownTargets(Vector3 position_, Color[] colTargets_, TargetProperties targetProp_)
        {
            int angle0_ = Random.Range(0, 360);
            int nTargets_ = colTargets_.Length;

            for (int i = 0; i < nTargets_; i++)
            {
                targetsList.Add((GameObject)Instantiate(targetPrefab, position_, Quaternion.identity));
                GameObject target_ = targetsList[targetsList.Count - 1];
                target_.GetComponent<TargetBehavior>().SetAngleDirection(angle0_ + i * (float)360 / nTargets_);
                target_.GetComponent<TargetBehavior>().SetColor(colTargets_[i]);
                target_.GetComponent<TargetBehavior>().SetTranslationSpeed(targetProp_.transSpeed);
                target_.GetComponent<TargetBehavior>().SetRotationAxis(position_);
                target_.GetComponent<TargetBehavior>().SetRotationSpeed(targetProp_.rotSpeed);
                target_.GetComponent<TargetBehavior>().setScale(targetProp_.scale);
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