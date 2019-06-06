using INSEP;
using System;
using System.IO.Ports;
using System.Linq;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace INSEP
{
    public class SessionManager : MonoBehaviour
    {
        /// <summary>
        /// Session format
        /// </summary>
        //[SerializeField]
        //private int _activeTime = 60;   // second
        //[SerializeField]
        //private int _recoveryTime = 50;  // second

        [SerializeField]
        private SessionTime[] _sessionTimes ;

        // Start is called before the first frame update
        void Start()
        {
            //StartCoroutine(Countdown(_activeTime));
        }

        // Update is called once per frame
        void Update()
        {

        }

        private IEnumerator Countdown(float duration_)
        {
            yield return new WaitForSeconds(duration_);
            print(Time.time);
        }
    }
}
