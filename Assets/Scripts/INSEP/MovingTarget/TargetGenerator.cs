using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSEP {
    public class TargetGenerator : MonoBehaviour
    {
        [SerializeField]
        private GameObject _targetPrefab ;
        private List<GameObject> _targetsList;       // list of current targets

        private void Awake()
        {
            _targetsList = new List<GameObject>();
        }

        // Start is called before the first frame update
        void Start()
        {
            // Instantiate targets
            AddTarget(new Vector3(2.0f, 0, 0));
        }

        public void AddTarget(Vector3 position_) {
            var newTarget_ = Instantiate(_targetPrefab, position_, Quaternion.identity);
            newTarget_.transform.parent = gameObject.transform;
            newTarget_.transform.localPosition = position_;
            _targetsList.Add(newTarget_);
        }

        public void RemoveTarget(int ind_) {
            if (ind_ >= 0 && ind_ < _targetsList.Count) {
                _targetsList[ind_].GetComponent<Target>().DestroyTarget();
                _targetsList.RemoveAt(ind_);
            }
        }

    private void Update()
        {
            if (Input.GetKeyDown("space"))
            {
                print("space key was pressed");
                RemoveTarget(0);
            }
        }
    }
}
