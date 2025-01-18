using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core
{
[CreateAssetMenu]
public class DishCardDoc : SerializedScriptableObject
{
	[MultiLineProperty(Lines = 8)]
	public string RecipeDesc;
	public int Priority;
	public GameObject DishContainerPrefab;
	public List<DishClueDoc> ClueList = new();
}
}