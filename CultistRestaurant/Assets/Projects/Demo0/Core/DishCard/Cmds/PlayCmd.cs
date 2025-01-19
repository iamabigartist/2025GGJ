using Audio;
using UnityEngine;
namespace Projects.Demo0.Core
{
public class PlayCmd : Cmd
{
	public string AudioEvent = "";
	public string AnimationEvent = "";
	public override void Execute(CmdContext ctx)
	{

		if (!string.IsNullOrEmpty(AnimationEvent))
		{
			var exist = ctx.TargetGameObj.TryGetComponent<Animation>(out var animation);
			if (exist)
			{
				if (!animation.GetClip(AnimationEvent)) { Debug.Log($"目标物体 {ctx.TargetGameObj.name} 没有动画事件 {AnimationEvent}"); }
				else { animation.Play(AnimationEvent); }
			}
			else
			{
				Debug.Log($"目标物体 {ctx.TargetGameObj.name} 没有 Animation 组件，但是有动画事件 {AnimationEvent}");
			}
		}

		if (!string.IsNullOrEmpty(AudioEvent))
		{
			var instance = AudioManager.Instance;
			if (instance == null) { Debug.Log("AudioManager 未初始化"); }
			else
			{
				Debug.Log($"播放音频事件 {AudioEvent} on {ctx.TargetGameObj.name}");
				AudioManager.Instance.PostEvent(AudioEvent, AudioManager.Instance.globalInitializer);
			}
		}
	}
}
}