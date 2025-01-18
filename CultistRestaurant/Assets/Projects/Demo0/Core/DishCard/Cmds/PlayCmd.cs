using Audio;
using UnityEngine;
namespace Projects.Demo0.Core
{
public class PlayCmd : Cmd
{
	public string AudioEvent;
	public string AnimationEvent;
	public override void Execute(CmdContext ctx)
	{
		var animation = ctx.TargetGameObj.GetComponent<Animation>();
		animation.Play(AnimationEvent);
		// 然后播放音频API
		AudioManager.Instance.PostEvent(AudioEvent, AudioManager.Instance.globalInitializer);
	}
}
}