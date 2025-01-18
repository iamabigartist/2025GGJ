using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core
{
[CreateAssetMenu]
public class ClueAnimationDoc : SerializedScriptableObject
{
	public List<AnimationClip> Clips = new();
}
}