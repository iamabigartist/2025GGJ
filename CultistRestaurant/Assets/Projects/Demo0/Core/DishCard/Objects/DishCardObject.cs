using System.Collections.Generic;
using System.Linq;
using Projects.Demo0.Core;
using Projects.Demo0.Core.DishCard.Objects;
using Projects.Demo0.Core.Level;
using UnityEngine;
public class DishCardObject : MonoBehaviour
{
	public static (DishCardObject dishCardObj, GameObject gameObj) Create(DishCardDoc cardDoc, LevelDoc levelDoc)
	{
		var go = new GameObject("DishCard");
		var curDishCardObj = go.AddComponent<DishCardObject>();
		curDishCardObj.m_Doc = cardDoc;
		curDishCardObj.m_ClueObjList = new();
		var (wrongProb, pollutedProb) = (levelDoc.ClueWrongProb, levelDoc.CluePollutedProb);
		foreach (var clueDoc in cardDoc.ClueList)
		{
			DishClueStateDoc curClueStateDoc;
			bool needWrong = wrongProb >= Random.value;
			bool needPolluted = pollutedProb >= Random.value;
			if (!needPolluted)
			{
				var clueStateList = clueDoc.StateDoc.Where(stateDoc => stateDoc.State.Correct != needWrong).ToList();
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
		return (curDishCardObj, go);
	}

	DishCardDoc m_Doc;
	List<DishClueObject> m_ClueObjList;
}