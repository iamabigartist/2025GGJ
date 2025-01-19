using System.Collections;
using System.Collections.Generic;
using Audio;
using Projects.Demo0.Core.GameGlobal;
using Projects.Demo0.Core.Level;
using Projects.Demo0.Core.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
namespace Projects.Demo0.Core.Mgr
{
public class GameRoundMgr : SerMonoSingleton<GameRoundMgr>
{
	public int playerHP;
	public int PlayerHP
	{
		get => playerHP;
		set
		{
			playerHP = value;
			if (playerHP < 0) { playerHP = 0; }
			playerUIMgr.SetHP(playerHP);
		}
	}
	public bool PlayerDead => PlayerHP == 0;
	public int GetCardTotalNum;
	public int WorldPollutedNum;


	public bool DishServeOutSignal;
	public bool OnDishServeOutSignal() => DishServeOutSignal = true;

	public bool ContinueSignal;
	void OnContinueSignal() => ContinueSignal = true;

	public DishCardObject CurrentCard;
	void OnServeBtnClicked(bool accept)
	{
		playerUIMgr.SetServeBtnInteractive(false);
		if (CurrentCard) { CurrentCard.OffTable(accept); }
		else { Debug.LogError("CurrentCard is null"); }
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

	void SetGameLevel(LevelDoc levelDoc)
	{
		switch (levelDoc.name)
		{
			case "关卡1":
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.Level01);
				break;
			case "关卡2":
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.Level02);
				break;
			case "关卡3":
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.Level03);
				break;
			default:
				break;
		}
	}
	public IEnumerator LevelCoroutine(LevelDoc levelDoc)
	{
		PlayerHP = gameConfig.MaxHP;
		var deck = LevelDeckGenerator.GenerateDeck(levelDoc);
		GetCardTotalNum += deck.Count;
		SetGameLevel(levelDoc);
		foreach (var card in deck)
		{
			card.OnStartCheck += () =>
			{
				playerUIMgr.SetServeBtnInteractive(true);
			};
			card.OnServeOut += tuple =>
			{
				var (hpChange, acceptPolluted) = tuple;
				if (acceptPolluted)
				{
					WorldPollutedNum++;
					AudioManager.Instance.SetGlobalRtpcValue(AudioManager.RtpcConstants.GlobalPollutionLevel, WorldPollutedNum);
				}
				PlayerHP += hpChange;
				OnDishServeOutSignal();
			};
			CurrentCard = card;
			DishServeOutSignal = false;
			card.OntoTable();
			playerUIMgr.SetRecipeDesc(card.m_Doc.RecipeDesc.ToString());
			yield return new WaitUntil(() => DishServeOutSignal);
			if (PlayerDead) { break; }
		}
	}

	public IEnumerator GameRoundCoroutine()
	{
		Debug.Log("Game Start");
		// 关卡循环
		for (int i = 0; i < levelDocList.Count; i++)
		{
			yield return StartCoroutine(LevelCoroutine(levelDocList[i]));
			if (PlayerDead) { break; }
			curtainUIMgr.ShowDayEnd(i);
			AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.BlackTransition);
			ContinueSignal = false;
			yield return new WaitUntil(() => ContinueSignal);
		}

		Debug.Log("Handle Story End");
		yield return new WaitUntil(() => !curtainUIMgr.IsMoving);
		// 处理结局
		if (PlayerDead)
		{
			AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.BlackTransition);
			SceneManager.LoadScene("OverScene");
		}
		else
		{
			if ((float)WorldPollutedNum / GetCardTotalNum <=
				gameConfig.GoodEnd_AcceptPolluteRatioRequired)
			{
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.FinishView);
				SceneManager.LoadScene("GEScene");
			}
			else
			{
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.FinishViewBad);
				SceneManager.LoadScene("BEScene");
			}
		}
	}
	public static void ReturnToIntroScene() => SceneManager.LoadScene("IntroScene");
}
}