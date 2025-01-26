﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Utility;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoSingleton<ObjectSpawner>
{
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


    public float explodeForce = 1000.0f;
    public int ObjectCount => GameObject.FindObjectsByType<BaseObject>(FindObjectsSortMode.InstanceID).Length;

    public bool IsReachedMaxObjectCount => (ObjectCount >= maxObjectCount);

    private void Update()
    {
        _timer += Time.deltaTime;
        if(_timer >= _spawnInterval)
        {
            _timer = 0.0f;
            if(ObjectCount < maxObjectCount) StartCoroutine(SpawnObject());
        }
    }

    private IEnumerator SpawnObject()
    {
        var grid = _gridFloor.GetRandomGrid().center;
        var halfGrid = _gridFloor.distanceBetweenGrids / 2.0f;
        grid.y = halfGrid;

        var spawnType = GetRandomObjectType();
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

        _spawnInterval = _spawnRateCurve.Evaluate(time: ObjectCount / (float)maxObjectCount);
    }

    private Objects.ObjectType GetRandomObjectType()
    {
        if(Random.Range(0.0f, 1.0f) < 0.5f)
        {
            var questObj = GameManager.Instance.QuestManager.CurrentQuest.objects;
            return questObj[Random.Range(0, questObj.Count)];
        }


        return (Objects.ObjectType)Random.Range(0, (int)Objects.ObjectType.Egg);
    }

    protected override void Init()
    {
    }
}