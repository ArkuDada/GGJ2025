using Unity.VisualScripting;
using UnityEngine;

public class GrowingObject : BaseObject
{
    private bool doGrow = true;

    [SerializeField]
    private float _growthTimer = 0;

    [SerializeField]
    private float _maxGrowth = 1.0f;

    public AnimationCurve ScoreMultCurve;

    [SerializeField]
    Vector3 _startScale;

    [SerializeField]
    Vector3 _endScale;

    public float Progress => _growthTimer / _maxGrowth;

    protected override void Update()
    {
        if(!doGrow) return;

        if(_growthTimer < _maxGrowth && State == ObjectState.Grounded)
        {
            _growthTimer += Time.deltaTime;
            if(_growthTimer >= _maxGrowth)
            {
                _growthTimer = _maxGrowth;
            }

            float t = Progress;

            if(Type == Objects.ObjectType.Wheat && TryGetComponent(out WheatData wheatData))
            {
                int state = t < 0.3f ? 0 : t < 0.6f ? 1 : 2;
                wheatData.ChangeState(state);
            }
            else if(Type == Objects.ObjectType.Egg && TryGetComponent(out EggData eggData))
            {
                transform.localScale = Vector3.Lerp(_startScale, _endScale, t);

                if(t > 0.99f)
                {
                    eggData.Hatch();
                }
            }
        }
    }

    public override void BubbleLift()
    {
        base.BubbleLift();
        doGrow = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.TryGetComponent(out BaseObject baseObject))
        {
            switch(baseObject.Type)
            {
                case Objects.ObjectType.Box:
                    baseObject.DestroyObject();
                    break;
                default:
                    break;
            }
        }
    }

    public override int GetScore()
    {
        return (int)(base.GetScore() * ScoreMultCurve.Evaluate(_growthTimer / _maxGrowth));
    }
}