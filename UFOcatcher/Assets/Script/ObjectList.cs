using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectList", menuName = "Scriptable Objects/ObjectList")]
public class ObjectList : ScriptableObject
{
    public string questName;
    public List<Utility.Objects.ObjectType> objects;
}
