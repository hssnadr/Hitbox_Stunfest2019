using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CRI.HitBoxTemplate.Example
{
    public class TargetsManager : MonoBehaviour
    {
        public GameObject targetPrefab;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        private void OnEnable()
        {
            HitboxManager.onHit += OnHit;
        }

        private void OnDisable()
        {
            HitboxManager.onHit -= OnHit;
        }

        private void OnHit(object sender, HitboxHitEventArgs e)
        {
            Debug.Log("onHit position =" + e.hitPosition);
            Instantiate(targetPrefab, new Vector3(e.hitPosition.x, e.hitPosition.y, 10), Quaternion.identity);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}