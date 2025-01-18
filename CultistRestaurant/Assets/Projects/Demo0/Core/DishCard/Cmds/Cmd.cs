using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Projects.Demo0.Core
{
public class CmdContext
{
	public GameObject TargetGameObj;
}
public abstract class Cmd
{
	public float DelaySec;
	public abstract void Execute(CmdContext ctx);
}
public static class CmdExtensions
{
	public static IEnumerator ExecuteCmdList(this IEnumerable<Cmd> cmdList, CmdContext ctx)
	{
		foreach (var cmd in cmdList)
		{
			yield return cmd.DelaySec > 0 ? new WaitForSeconds(cmd.DelaySec) : null;
			cmd.Execute(ctx);
		}
	}
}
}