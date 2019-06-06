using UnityEngine;

namespace INSEP
{
    [System.Serializable]
    public struct SessionTime
    {
        [SerializeField]
        public int nTarget;
        [SerializeField]
        public int activeTime;   // time in second
        [SerializeField]
        public int recoveryTime; // time in second
        [SerializeField]
        public int nAttack;
        [SerializeField]
        public int nDefense;
        [SerializeField]
        public int rythm;        // hit per minute
        

        public SessionTime(int n_, int active_, int recovery_, int nAtt_, int nDef_, int rythm_)
        {
            this.nTarget = n_;
            this.activeTime = active_;
            this.recoveryTime = recovery_;
            this.nAttack = nAtt_;
            this.nDefense = nDef_;
            this.rythm = rythm_;
            
        }
    }
}
