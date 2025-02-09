using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeObject", menuName = "Scriptable Objects/RecipeList")]
public class RecipeObject : ScriptableObject
{
    public string questName;
	public List<Objects.ObjectType> objects;
	public List<int> quantities;
}
