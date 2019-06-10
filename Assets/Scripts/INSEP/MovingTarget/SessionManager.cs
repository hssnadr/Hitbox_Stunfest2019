using CRI.HitBoxTemplate.Example;
using System.Collections;
using UnityEngine;

namespace INSEP
{
    public class SessionManager : MonoBehaviour
    {
        public ExampleSerialController serialController;

        private bool _isPlaying = false;

        private int _score = 0 ;
        private float _curCadence = 0.0f;
        public int _curSessionTime = 0;
        public bool _onAttack = false;

        private float _sessionTimer0 = 0.0f;
        private float _actionTimer0 = 0.0f;
        public int _nAttackAction = 0;

        [SerializeField]
        private SessionTime[] _sessionTimes ;

        private void OnEnable()
        {
            TargetController.onHit += Count;
        }

        private void OnDisable()
        {
            TargetController.onHit -= Count;
        }

        void Count(object sender, TargetControllerHitEventArgs e) {
            _nAttackAction++;
            _score++;
            
            if(_nAttackAction >= _sessionTimes[_curSessionTime].nAttack)
            {
                _nAttackAction = 0;

                // Start defense coroutine
                float _elapsCurTimeAction = Time.time - _actionTimer0;
                float dlyDefense_ = _sessionTimes[_curSessionTime].timePerAction - _elapsCurTimeAction;
                StartCoroutine(TrigDefenseAction(dlyDefense_, _sessionTimes[_curSessionTime].nDefense));
            }
        }

        void Update()
        {
            if (Input.GetKeyDown("space") && !_isPlaying)
            {
                _sessionTimer0 = Time.time;
                _curSessionTime = 0;
                _actionTimer0 = Time.time;
                _isPlaying = true;
            }

            if (_isPlaying)
            {
                _curCadence = _score / (Time.time - _sessionTimer0);

                //------------------
                //-------ACTIVE-----
                //------------------
                if (Time.time - _sessionTimer0 < _sessionTimes[_curSessionTime].activeTime)
                {
                    _onAttack = true;
                }
                else
                {
                    //------------------
                    //-----RECORVERY----
                    //------------------
                    if (Time.time - _sessionTimer0 < _sessionTimes[_curSessionTime].recoveryTime)
                    {
                        if (_onAttack)
                        {
                            serialController.ScreenSaver();
                            // --> disable Unity screen to Hitbox
                        }
                        _onAttack = false;
                    }
                    else
                    {
                        _curSessionTime++;
                        _sessionTimer0 = Time.time;
                        if (_curSessionTime >= _sessionTimes.Length)
                        {
                            if (_isPlaying)
                            {
                                serialController.EndGame();
                                // --> disable Unity screen to Hitbox
                                // then wait for automatic save mode
                            }
                            _isPlaying = false;
                        }
                    }
                }
            }
        }

        private IEnumerator TrigDefenseAction(float time_, int n_)
        {
            float timePerDef_ = time_ / (float)n_;

            for (int i = 0; i < n_; i++) {
                yield return new WaitForSeconds(timePerDef_);
                Debug.Log(i);
            }
        }
    }
}