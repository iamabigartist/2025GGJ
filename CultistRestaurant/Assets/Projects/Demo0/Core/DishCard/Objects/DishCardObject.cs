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
using System.IO;
using Audio;
using Random = UnityEngine.Random;
public class DishCardObject : MonoBehaviour
{
	private static Dictionary<string, Vector2> s_PositionDict;

	private static void InitializePositionData()
	{
		if (s_PositionDict != null) return;
		
		s_PositionDict = new Dictionary<string, Vector2>();
		var posPath = "Assets/Projects/Demo0/Resources/Art/Sprites/newPos.txt";
		if (!File.Exists(posPath)) return;

		var lines = File.ReadAllLines(posPath);
		foreach (var line in lines)
		{
			var parts = line.Split(',');
			if (parts.Length >= 3 && 
				float.TryParse(parts[1], out float x) && 
				float.TryParse(parts[2], out float y))
			{
				// 在读取时就应用缩放
				s_PositionDict[parts[0].Trim()] = new Vector2(x * 0.01f, y * -0.01f);
			}
		}
	}

	public void ApplyCluePositions()
	{
		var posPath = "Assets/Projects/Demo0/Resources/Art/Sprites/newPos.txt";
		if (!File.Exists(posPath)) return;

		// 读取并解析位置数据
		var positionDict = new Dictionary<string, Vector2>();
		var lines = File.ReadAllLines(posPath);
		foreach (var line in lines)
		{
			var parts = line.Split(',');
			if (parts.Length >= 3 && 
				float.TryParse(parts[1], out float x) && 
				float.TryParse(parts[2], out float y))
			{
				positionDict[parts[0].Trim()] = new Vector2(x, y);
			}
		}

		// 应用位置到对应的线索对象
		foreach (var clueObj in m_ClueObjList)
		{
			var spriteName = clueObj.gameObject.name;
			if (positionDict.TryGetValue(spriteName, out Vector2 pos))
			{
				clueObj.gameObject.transform.localPosition = pos;
			}
		}
	}

	public static DishCardObject Create(DishCardDoc cardDoc, LevelDoc levelDoc)
	{
		InitializePositionData();  // 确保位置数据已加载
		
		var go = Instantiate(GameDocMgr.Instance.m_GameGlobalConfig.DishCardPrefab);
		go.transform.localPosition = new Vector3(-3f, 0f, 0f);
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
			var (clueObj, clueGo) = DishClueObject.Create(clueDoc, curClueStateDoc);
			// 设置父级为go的第一个子物体，如果没有则使用go本身
			var parentTransform = go.transform.childCount > 0 ? go.transform.GetChild(0) : go.transform;
			clueGo.transform.SetParent(parentTransform);
			
			// 应用预设位置，如果没有对应位置数据则使用默认位置
			var spriteName = clueGo.name;
			var lastPart = spriteName.Split('_').LastOrDefault();
			
			if (s_PositionDict != null && lastPart != null && s_PositionDict.TryGetValue(lastPart, out Vector2 pos))
			{
				clueGo.transform.localPosition = pos;
			}
			else
			{
				clueGo.transform.localPosition = Vector3.zero;
				Debug.LogWarning($"未找到位置数据: {clueGo.name},lastPart:{lastPart}");
			}
			
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
		AudioManager.Instance.PostEvent("Play_PlateCome", AudioManager.Instance.globalInitializer);
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
		AudioManager.Instance.PostEvent("Play_PlateGo", AudioManager.Instance.globalInitializer);
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
				AudioManager.Instance.PostEvent("Play_VO_Dislike", AudioManager.Instance.globalInitializer);
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptWrongDishDesc.ToString());
				return (gameConfig.AcceptWrongDish_HPChange, dishPolluted);
			}
			if (dishPolluted)
			{
				Debug.Log("接受正确污染菜品");
				AudioManager.Instance.PostEvent("Play_VO_Like", AudioManager.Instance.globalInitializer);
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptPollutedCorrectDishDesc.ToString());
				return (gameConfig.AcceptPollutedCorrectDish_HPChange, true);
			}
		}
		else
		{
			if (m_ClueObjList.All(clue => clue.PerfectClue))
			{
				Debug.Log("拒绝完美菜品");
				AudioManager.Instance.PostEvent("Play_VO_Dislike", AudioManager.Instance.globalInitializer);
				MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.RefusePerfectDishDesc.ToString());
				return (gameConfig.RefusePerfectDish_HPChange, false);
			}
		}

		if (accept) { MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.AcceptNormalDishDesc.ToString()); }
		else { MsgBbViewportMono.CurViewportAppendMsgBb(gameConfig.RefuseNormalDishDesc.ToString()); }

		return (0, false);
	}

}