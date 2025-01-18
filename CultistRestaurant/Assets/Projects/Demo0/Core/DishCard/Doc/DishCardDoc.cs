using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core
{
[CreateAssetMenu]
public class DishCardDoc : SerializedScriptableObject
{
	public int Priority;
	public List<DishClueDoc> ClueList = new();
}
}