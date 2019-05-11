//using System.Collections;
//using System.Collections.Generic;
using CRI.HitBoxTemplate.Serial;
using System.Linq;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    public struct HitboxHitEventArgs
    {
        /// <summary>
        /// Position of the hit.
        /// </summary>
        public Vector2 hitPosition { get; }

        public HitboxHitEventArgs(Vector2 position)
        {
            this.hitPosition = position;
        }
    }

    public delegate void HitboxHitEventHandler(object sender, HitboxHitEventArgs e);
    public class HitboxManager : MonoBehaviour
    {
        public static event HitboxHitEventHandler onHit;

        public ExampleSerialController serialController;

        private Camera _hitboxCamera;

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
        }

        private void OnImpact(object sender, ImpactPointControlEventArgs e)
        {
            OnImpact(e.impactPosition);
        }

        private void OnImpact(Vector3 position)
        {
            Debug.Log("hola");
            Vector3 cameraForward = _hitboxCamera.transform.forward;
            Debug.DrawRay(position, cameraForward * 5000, Color.yellow, 10.0f);
            var hits = Physics.RaycastAll(position, cameraForward, Mathf.Infinity);
            if (hits != null && hits.Any(x => x.collider.GetComponent<Target>() != null))
            {
                var hitTargets = hits
                    .Where(
                        x => x.collider.GetComponent<Target>() != null
                        )
                    .OrderBy(
                        x => x.transform.position.z * cameraForward.z
                    );
                var first = hitTargets.First();
                Hit(first, position);
            }
        }

        private void Hit(RaycastHit hit, Vector2 position)
        {
            Debug.Log("coucou");
            hit.collider.GetComponent<Target>().Hit();
            if (onHit != null)
                onHit(this, new HitboxHitEventArgs(position));
        }

#if UNITY_EDITOR
        private void OnMouseDown()
        {
            Vector3 mousePosition = Input.mousePosition;
            if (!_hitboxCamera.orthographic)
                mousePosition.z = this.transform.position.z;
            OnImpact(_hitboxCamera.ScreenToWorldPoint(mousePosition));
        }
#endif

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
                OnMouseDown();

            //Vector3[] accelerations = serialController.accelerations;
            //for (int i = 0; i < accelerations.Length; i++)
            //{
            //    Debug.Log(string.Format("Acceleration Player {0}", accelerations[i]));
            //}
        }
    }
}
