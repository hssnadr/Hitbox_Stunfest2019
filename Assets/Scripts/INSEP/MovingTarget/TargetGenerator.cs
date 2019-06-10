using CRI.HitBoxTemplate.Example;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSEP
{
    public class TargetGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _targetAttack;
        [SerializeField]
        private GameObject _targetDefense;
        private List<GameObject> _targetsList;       // list of current targets

        public LineRenderer lineDefense;

        private void Awake()
        {
            _targetsList = new List<GameObject>();
        }

        private void OnEnable()
        {
            TargetController.onHit += UpdateTargets;
        }

        private void OnDisable()
        {
            TargetController.onHit -= UpdateTargets;
        }

        void UpdateTargets(object sender, TargetControllerHitEventArgs e)
        {
            Debug.Log(e.gameObjectID);
            for(int i=0; i<_targetsList.Count; i++)
            {
                if (int.Equals(e.gameObjectID, _targetsList[i].GetInstanceID()))
                {
                    SwapToTargetType(i, _targetDefense);
                }
            }
        }

            // Start is called before the first frame update
            void Start()
        {
            // Instantiate targets
            AddTarget(_targetDefense, new Vector3(1.5f, 0, 0));
            AddTarget(_targetDefense, new Vector3(-1.5f, 0, 0));
            AddTarget(_targetAttack, new Vector3(0, 1.5f, 0));
            AddTarget(_targetDefense, new Vector3(0, -1.5f, 0));

            //lineDefense.positionCount = _targetsList.Count;
        }

        public void AddTarget(GameObject target_, Vector3 position_)
        {
            var newTarget_ = Instantiate(target_, position_, Quaternion.identity);
            newTarget_.transform.parent = gameObject.transform;
            newTarget_.transform.localPosition = position_;
            _targetsList.Add(newTarget_);
        }

        public void SwapToTargetType(int targetIndex_, GameObject toTypeTarget_)
        {
            var pos_ = _targetsList[targetIndex_].transform.localPosition;
            RemoveTarget(targetIndex_);
            AddTarget(toTypeTarget_, pos_);
        }

        public void RemoveTarget(int ind_)
        {
            if (ind_ >= 0 && ind_ < _targetsList.Count)
            {
                Destroy(_targetsList[ind_]);
                _targetsList.RemoveAt(ind_);
            }
        }

        private void Update()
        {
            lineDefense.positionCount = _targetsList.Count;
            for (int i = 0; i < _targetsList.Count; i++)
            {
                lineDefense.SetPosition(i, _targetsList[i].transform.position);
            }

            if (Input.GetKeyDown("a"))
            {
                print("a key was pressed");
                SwapToTargetType(0, _targetAttack);
                //RemoveTarget(2);
            }
        }
    }
}
