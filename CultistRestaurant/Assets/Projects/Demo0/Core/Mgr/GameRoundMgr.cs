using System;
using System.Collections;
using Projects.Demo0.Core.Utils;
namespace Projects.Demo0.Core.Mgr
{
public class GameRoundMgr : MonoSingleton<GameRoundMgr>
{
	void Start() => StartCoroutine(GameRoundCoroutine());
	public IEnumerator GameRoundCoroutine()
	{
		var levelDocList = GameDocMgr.Instance.m_GameGlobalConfig.LevelList;
		yield return null;
	}
	
}
}