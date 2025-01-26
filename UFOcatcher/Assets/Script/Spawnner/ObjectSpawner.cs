using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance;

    [SerializeField]
    private GridFloor _gridFloor;

    [SerializeField]
    private List<GameObject> _objectPrefabs;

    [SerializeField]
    private float _spawnInterval = 1.0f;

    [SerializeField]
    private AnimationCurve _spawnRateCurve;

    private float _timer = 0.0f;

    [SerializeField]
    private int maxObjectCount = 25;

    [SerializeField]
    private GameManager gameManager;

    public float explodeForce = 1000.0f;
    public int ObjectCount => GameObject.FindObjectsByType<BaseObject>(FindObjectsSortMode.InstanceID).Length;

    public bool IsReachedMaxObjectCount => (ObjectCount >= maxObjectCount);

    public List<SpawnLimit> spawnLimits;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(gameManager == null)
        {
            gameManager = GameManager.Instance;
        }
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _spawnInterval)
        {
            _timer = 0.0f;
            if(!IsReachedMaxObjectCount) StartCoroutine(SpawnObject());
        }
    }

    public bool IsLimitReach(Objects.ObjectType type)
    {
        SpawnLimit limit = spawnLimits.Find(x => x.type == type);
        if(limit.limit == 0) return false;

        return GameObject.FindObjectsByType<BaseObject>(FindObjectsSortMode.InstanceID).Count(x => x.Type == type) >=
               limit.limit;
    }

    private IEnumerator SpawnObject()
    {
        var grid = _gridFloor.GetRandomGrid().center;
        var halfGrid = _gridFloor.distanceBetweenGrids / 2.0f;
        grid.y = halfGrid;

        var spawnType = GetRandomObjectType();

        if(!IsLimitReach(spawnType))
        {
            var obj = _objectPrefabs.Find(x => x.GetComponent<BaseObject>()?.Type == spawnType);

            switch(spawnType)
            {
                case Objects.ObjectType.Cow:
                    SoundManager.instance.PlaySFX("Cow Spawn");
                    break;
                case Objects.ObjectType.Wheat:
                    SoundManager.instance.PlaySFX("Wheat Spawn");
                    break;
                case Objects.ObjectType.Egg:
                    SoundManager.instance.PlaySFX("Egg Spawn");
                    break;
                case Objects.ObjectType.Chicken:
                    SoundManager.instance.PlaySFX("Chicken Spawn");
                    break;

                default:
                    break;
            }

            var hits = Physics.BoxCastAll(grid, Vector3.one * halfGrid, Vector3.up, Quaternion.identity, 0.1f);

            var baseObjects = hits.Select(x => x.collider.GetComponent<BaseObject>()).Where(x => x != null).ToList();

            bool containWheat = baseObjects.Any(o => o.Type == Objects.ObjectType.Wheat);
            if(hits.Length > 0 && !containWheat)
            {
                foreach(var o in baseObjects)
                {
                    o.gameObject.GetComponent<Rigidbody>().AddExplosionForce(explodeForce, grid, 1.5f);
                }
            }

            yield return new WaitForEndOfFrame();

            if(!containWheat) Instantiate(obj, grid, Quaternion.identity);
        }

        _spawnInterval = _spawnRateCurve.Evaluate(time: ObjectCount / (float)maxObjectCount);
    }

    private Objects.ObjectType GetRandomObjectType()
    {
        if(Random.Range(0.0f, 1.0f) < 0.5f)
        {
            var questObj = gameManager.QuestManager.CurrentQuest.objects;
            var qType = questObj[Random.Range(0, questObj.Count)];
            if(qType == Objects.ObjectType.Egg) qType = Objects.ObjectType.Chicken;
            return qType;
        }

        return (Objects.ObjectType)Random.Range(0, (int)Objects.ObjectType.Egg);
    }
}

[Serializable]
public struct SpawnLimit
{
    public Objects.ObjectType type;
    public int limit;
}