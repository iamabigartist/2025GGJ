using System.Collections;
using System.Collections.Generic;
using Projects.Demo0.Core.GameGlobal;
using Projects.Demo0.Core.Level;
using Projects.Demo0.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Projects.Demo0.Core.Mgr
{
public class GameRoundMgr : MonoSingleton<GameRoundMgr>
{
	public int PlayerHP;
	public bool PlayerDead => PlayerHP <= 0;
	public int GetCardTotalNum;
	public int WorldPollutedNum;


	public bool DishServeOutSignal;
	public bool OnDishServeOutSignal() => DishServeOutSignal = false;

	public bool ContinueSignal;
	void OnContinueSignal() => ContinueSignal = true;

	public DishCardObject CurrentCard;
	void OnServeBtnClicked(bool accept)
	{
		if (CurrentCard) { CurrentCard.OffTable(accept); }
	}

	GameGlobalConfig gameConfig;
	List<LevelDoc> levelDocList;
	CurtainUIMgr curtainUIMgr;
	PlayerUIMgr playerUIMgr;

	void Start()
	{
		gameConfig = GameDocMgr.Instance.m_GameGlobalConfig;
		levelDocList = gameConfig.LevelList;
		curtainUIMgr = CurtainUIMgr.Instance;
		playerUIMgr = PlayerUIMgr.Instance;

		curtainUIMgr.OnContinueBtnClicked += OnContinueSignal;
		playerUIMgr.OnServeBtnClicked += OnServeBtnClicked;

		StartCoroutine(GameRoundCoroutine());
	}

	void OnDestroy()
	{
		curtainUIMgr.OnContinueBtnClicked -= OnContinueSignal;
		playerUIMgr.OnServeBtnClicked -= OnServeBtnClicked;
		StopAllCoroutines();
	}

	public IEnumerator LevelCoroutine(LevelDoc levelDoc)
	{
		PlayerHP = gameConfig.MaxHP;
		var deck = LevelDeckGenerator.GenerateDeck(levelDoc);
		GetCardTotalNum += deck.Count;
		foreach (var card in deck)
		{
			card.OnStartCheck += () =>
			{
				playerUIMgr.SetServeBtnInteractive(true);
			};
			card.OnServeOut += tuple =>
			{
				playerUIMgr.SetServeBtnInteractive(false);
				var (hpChange, acceptPolluted) = tuple;
				if (acceptPolluted) { WorldPollutedNum++; }
				PlayerHP += hpChange;
				OnDishServeOutSignal();
			};
			CurrentCard = card;
			card.OntoTable();
			yield return new WaitUntil(() => DishServeOutSignal);
			if (PlayerDead) { break; }
		}
	}

	public IEnumerator GameRoundCoroutine()
	{
		// 关卡循环
		for (int i = 0; i < levelDocList.Count; i++)
		{
			yield return StartCoroutine(LevelCoroutine(levelDocList[i]));
			if (PlayerDead) { break; }
			curtainUIMgr.ShowDayEnd(i);
			ContinueSignal = false;
			yield return new WaitUntil(() => ContinueSignal);
		}
		// 处理结局
		if (PlayerDead) { curtainUIMgr.ShowDeadEnd(); }
		else
		{
			if ((float)WorldPollutedNum / GetCardTotalNum <=
				gameConfig.GoodEnd_AcceptPolluteRatioRequired) { curtainUIMgr.ShowGoodEnd(); }
			else { curtainUIMgr.ShowBadEnd(); }
		}
		ContinueSignal = false;
		yield return new WaitUntil(() => ContinueSignal);
		ReturnToIntroScene();
	}
	public static void ReturnToIntroScene() => SceneManager.LoadScene("IntroScene", LoadSceneMode.Single);
}
}