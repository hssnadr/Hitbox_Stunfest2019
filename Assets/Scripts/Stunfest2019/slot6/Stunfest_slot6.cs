using CRI.HitBoxTemplate.Serial;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    public class Stunfest_slot6 : MonoBehaviour
    {
        public ExampleSerialController serialController;
        public GameObject horizonPrefab;

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
        }

        private void Update()
        {
            Vector3[] accelerations_ = serialController.accelerations;
            if (accelerations_[0] != null)
            {
                Vector3 rotV3_ = new Vector3();
                Vector3 acceleration_ = accelerations_[0];

                //horizonPrefab.transform.Rotate = new Vector3(0,0, 90.0f * acceleration_.z, Space.self);
            }



            //for (int i = 0; i < accelerations.Length; i++)
            //{
            //    Debug.Log(string.Format("Acceleration Player {0}", accelerations[i]));
            //}



        }
    }
}
