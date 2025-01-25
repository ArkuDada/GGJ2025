using UnityEngine;

namespace NCC.Utility.Objects
{
    public class GrowingObject : BaseObject
    {
        
        [SerializeField] private float _growthTimer = 0;
        [SerializeField] private float _maxGrowth = 1.0f;

        public AnimationCurve ScoreMultCurve;

        [SerializeField] Vector3 _startScale;
        [SerializeField] Vector3 _endScale;
        
        protected override void Update()
        {
            if(_growthTimer < _maxGrowth)
            {
                _growthTimer += Time.deltaTime;
                if(_growthTimer >= _maxGrowth)
                {
                    _growthTimer = _maxGrowth;
                }
                
                float t = _growthTimer / _maxGrowth;
                transform.localScale = Vector3.Lerp(_startScale, _endScale, t);
            }
        }
        
        public override int GetScore()
        {
            return (int)(base.GetScore() * ScoreMultCurve.Evaluate(_growthTimer / _maxGrowth));
        }

    }
}