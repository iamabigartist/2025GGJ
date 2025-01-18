using System;
using UnityEngine;
namespace Projects.Demo0.Core.Utils
{

public static class TestMsgStatic
{
	public static event Action<string> OnTestMsg;
	public static void Invoke(string str) => OnTestMsg?.Invoke(str);
}

public class TestDestroyedMono : MonoSingleton<TestDestroyedMono>
{
	void testMsg(string str) => Debug.Log($"{nameof(TestDestroyedMono)}: {str}.");
	protected override void Awake()
	{
		TestMsgStatic.OnTestMsg += testMsg;
		Destroy(gameObject);
	}
}
}