using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Projects.Demo0.Core;
using Projects.Demo0.Core.DishCard.Objects;
using Projects.Demo0.Core.Level;
using Projects.Demo0.Core.Mgr;
using Projects.Demo0.Core.Utils.MessageBubble;
using UnityEngine;
using Random = UnityEngine.Random;
public class DishCardObject : MonoBehaviour
{
	public static DishCardObject Create(DishCardDoc cardDoc, LevelDoc levelDoc)
	{
		var go = Instantiate(GameDocMgr.Instance.m_GameGlobalConfig.DishCardPrefab);
		if (cardDoc.DishContainerPrefab) { Instantiate(cardDoc.DishContainerPrefab, go.transform); }
		var curDishCardObj = go.GetComponent<DishCardObject>();
		curDishCardObj.m_Doc = cardDoc;
		curDishCardObj.m_ClueObjList = new();
		var cardNeedWrong = levelDoc.ClueWrongProb >= Random.value;
		var cardNeedPolluted = levelDoc.CluePollutedProb >= Random.value;
		foreach (var clueDoc in cardDoc.ClueList)
		{
			DishClueStateDoc curClueStateDoc;
			bool clueNeedWrong = cardNeedWrong && levelDoc.ClueWrongRatio >= Random.value;
			bool clueNeedPolluted = cardNeedPolluted && levelDoc.CluePollutedRatio >= Random.value;
			if (!clueNeedPolluted)
			{
				var clueStateList = clueDoc.StateDoc.Where(stateDoc => stateDoc.State.Correct != clueNeedWrong).ToList();
				curClueStateDoc = clueStateList[Random.Range(0, clueStateList.Count)];
			}
			else
			{
				var clueStateList = clueDoc.StateDoc.Where(stateDoc => stateDoc.State.Polluted).ToList();
				curClueStateDoc = clueStateList[Random.Range(0, clueStateList.Count)];
			}
			var (clueObj, clueGo) = DishClueObject.Create(clueDoc, curClueStateDoc); //!!
			clueGo.transform.SetParent(go.transform);
			clueGo.transform.localPosition = Vector3.zero;
			curDishCardObj.m_ClueObjList.Add(clueObj);
		}
		curDishCardObj.m_SpriteList = curDishCardObj.m_ClueObjList.Where(clue => clue.gameObject.GetComponent<SpriteRenderer>()).ToList();
		curDishCardObj.gameObject.SetActive(false);
		return curDishCardObj;
	}

	public DishCardDoc m_Doc;
	public List<DishClueObject> m_ClueObjList;
	public List<DishClueObject> m_SpriteList;
	public event Action OnStartCheck;
	public event Action<(int hpChange, bool acceptPolluted)> OnServeOut;
	public Animation m_Animation;
	public float m_SpriteAlpha;
	float m_SpriteAlpha_Cached;
	void SetSpriteAlpha(float alpha)
	{
		foreach (var sprite in m_SpriteList)
		{
			var color = sprite.gameObject.GetComponent<SpriteRenderer>().color;
			color.a = alpha;
			sprite.gameObject.GetComponent<SpriteRenderer>().color = color;
		}
	}

	void Update()
	{
		if (m_SpriteAlpha != m_SpriteAlpha_Cached)
		{
			SetSpriteAlpha(m_SpriteAlpha);
			m_SpriteAlpha_Cached = m_SpriteAlpha;
		}
	}

	/// <summary>
	///     菜上检测桌
	/// </summary>
	public void OntoTable()
	{
		gameObject.SetActive(true);
		m_Animation.Play("DishOntoTable");
		StartCoroutine(OntoTable_Impl());
	}
	IEnumerator OntoTable_Impl()
	{
		yield return new WaitUntil(() => !m_Animation.isPlaying);
		OnStartCheck?.Invoke();
	}

	/// <summary>
	///     菜下检测桌
	/// </summary>
	public void OffTable(bool accept)
	{
		m_Animation.Play("DishOffTable");
		StartCoroutine(OffTable_Impl(accept));
	}
	IEnumerator OffTable_Impl(bool accept)
	{
		yield return new WaitUntil(() => !m_Animation.isPlaying);
		gameObject.SetActive(false);
		OnServeOut?.Invoke(GetDishRes(accept));
	}

	/// <summary>
	///     计算菜品在结算时的HP变化
	/// </summary>
	(int hpChange, bool acceptPolluted) GetDishRes(bool accept)
	{
		var gameConfig = GameDocMgr.Instance.m_GameGlobalConfig;
		var dishPolluted = m_ClueObjList.Exists(clue => clue.PollutedClue);
		if (accept)
		{
			if (m_ClueObjList.Exists(clue => clue.WrongClue))
			{
				Debug.Log("接受错误菜品");
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptWrongDishDesc.ToString());
				return (gameConfig.AcceptWrongDish_HPChange, dishPolluted);
			}
			if (dishPolluted)
			{
				Debug.Log("接受正确污染菜品");
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptPollutedCorrectDishDesc.ToString());
				return (gameConfig.AcceptPollutedCorrectDish_HPChange, true);
				
			}
		}
		else
		{
			if (m_ClueObjList.All(clue => clue.PerfectClue))
			{
				Debug.Log("拒绝完美菜品");
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.RefusePerfectDishDesc.ToString());
				return (gameConfig.RefusePerfectDish_HPChange, false);
			}
		}

		if (accept) { MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptNormalDishDesc.ToString()); }
		else { MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.RefuseNormalDishDesc.ToString()); }

		return (0, false);
	}

}