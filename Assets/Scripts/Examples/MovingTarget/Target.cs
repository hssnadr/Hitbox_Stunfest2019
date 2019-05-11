// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    [RequireComponent(typeof(Animator))]
    public class Target : MonoBehaviour
    {
        /// <summary>
        /// The index of the only player who can hit this target.
        /// </summary>
        internal int playerIndex;
        /// <summary>
        /// Is the target activated?
        /// </summary>
        [SerializeField]
        [Tooltip("Is the target activated?")]
        internal bool activated = false;
        /// <summary>
        /// Prefab of the feedback object.
        /// </summary>
        [SerializeField]
        [Tooltip("Prefab of the feedback object.")]
        private HitFeedbackAnimator _hitFeedbackPrefab = null;
        [SerializeField]
        [Tooltip("Gradient of the particle system when the target is activated.")]
        public Gradient _activatedGradient;
        [SerializeField]
        [Tooltip("Gradient of the particle system when the target is deactivated.")]
        public Gradient _deactivatedGradient;

        /// <summary>
        /// Time of the last hit.
        /// </summary>
        public float lastHit
        {
            get;
            private set;
        }

        /// <summary>
        /// Z position of the target during the previous frame.
        /// </summary>
        public float zPosition
        {
            get; private set;
        }

        private Vector3 _lastPosition;

        public Vector3 speedVector
        {
            get
            {
                return transform.position - _lastPosition;
            }
        }

        private Animator _animator;
        private ParticleSystem _ps;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _ps = GetComponent<ParticleSystem>();
            lastHit = Time.time;
            var col = _ps.colorOverLifetime;
            lastHit = Time.time;
            if (activated)
            {
                activated = false;
                col.color = _deactivatedGradient;
            }
            else
            {
                activated = true;
                col.color = _activatedGradient;
            }
        }

        internal void Hit()
        {
            if ((Time.time - lastHit) > 0.5f)
            {
                var col = _ps.colorOverLifetime;
                lastHit = Time.time;
                if (activated)
                {
                    activated = false;
                    col.color = _deactivatedGradient;
                }
                else
                {
                    activated = true;
                    col.color = _activatedGradient;
                }
                if (_hitFeedbackPrefab != null)
                {
                    var go = GameObject.Instantiate(_hitFeedbackPrefab, this.transform);
                    go.gameObject.layer = this.gameObject.layer;
                }
            }
        }
        
        private void Update()
        {
            _animator.SetBool("Activated", activated);
        }
    }
}