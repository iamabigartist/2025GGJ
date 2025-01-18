using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.Level
{
[CreateAssetMenu]
public class LevelDoc : SerializedScriptableObject
{
	[Title("Difficulty")]
	public int CardCount = 7;
	[Tooltip("菜品存在错误概率")]
	public float ClueWrongProb = 0.6f;
	[Tooltip("菜品存在污染概率")]
	public float CluePollutedProb = 0.05f;
	[Tooltip("菜品错误时的错误部位比例")]
	public float ClueWrongRatio = 0.3f;
	[Tooltip("菜品污染时的污染部位比例")]
	public float CluePollutedRatio = 0.5f;

	[Title("Scene")]
	[Tooltip("场景的污染程度，使用数字来指定一个场景演出皮肤")]
	public int PolluteSceneShowSkin = 0;
}
}