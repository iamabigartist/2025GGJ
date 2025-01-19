using System.Collections.Generic;
using Projects.Demo0.Core.Localization;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core
{
[CreateAssetMenu]
public class DishCardDoc : SerializedScriptableObject
{
	public MultiLangStr RecipeDesc;
	public int Priority;
	public GameObject DishContainerPrefab;
	public List<DishClueDoc> ClueList = new();
}
}