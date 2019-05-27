using System.Collections;
using CRI.HitBoxTemplate.Serial;
using UnityEngine;
using System.Collections.Generic;

namespace CRI.HitBoxTemplate.Example
{
    public class Stunfest_slot5 : MonoBehaviour
    {
        public ExampleSerialController serialController;

        public GameObject prefabTarget;

        public Vector2 maxX;
        public Vector2 maxY;

        public List<CircleTarget> targets;

        public float tolerance = 100f;

        public Vector2 timerMinMax;

        void Start()
        {
            StartCoroutine(SpawnTargetCoroutine());
            CircleTarget.OnDestroyEvent += OnDestroyTarget;
        }

        private void OnEnable()
        {
            ImpactPointControl.onImpact += OnImpact;
        }

        private void OnDisable()
        {
            ImpactPointControl.onImpact -= OnImpact;
        }

        private void OnImpact(object sender, ImpactPointControlEventArgs e)
        {
            Debug.Log(string.Format("Impact: Player [{0}], Position [{1}], Accelerometer [{2}]",
                e.playerIndex,
                e.impactPosition,
                e.accelerometer));

            for (int i = 0; i < targets.Count; i++)
            {
                if(Vector3.Distance(e.impactPosition,targets[i].transform.position) <= tolerance)
                {
                    targets[i].OnHit();
                    break;
                }
            }
        }

        private void Update()
        {
            Vector3[] accelerations = serialController.accelerations;
            for (int i = 0; i < accelerations.Length; i++)
            {
                Debug.Log(string.Format("Acceleration Player {0}", accelerations[i]));
            }
        }

        IEnumerator SpawnTargetCoroutine()
        {
            GameObject g = Instantiate(prefabTarget, new Vector3(Random.Range(maxX.x, maxX.y), Random.Range(maxY.x, maxY.y), 0), Quaternion.identity) as GameObject;

            targets.Add(g.GetComponent<CircleTarget>());

            float time = Random.Range(timerMinMax.x, timerMinMax.y);

            while (time > 0)
            {
                time -= Time.deltaTime;
                yield return null;
            }

            StartCoroutine(SpawnTargetCoroutine());
        }

        void OnDestroyTarget()
        {
            if (targets.Count > 0)
                targets.RemoveAt(0);
        }
    }
}
