using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.Level
{
[CreateAssetMenu]
public class LevelDoc : SerializedScriptableObject
{
	[Title("Difficulty")]
	public int CardCount = 5;
	public float ClueWrongProb = 0.5f;
	public float CluePollutedProb = 0f;

	[Title("Scene")]
	[Tooltip("场景的污染程度，使用数字来指定一个场景演出皮肤")]
	public int PolluteSceneShowSkin = 0;
}
}