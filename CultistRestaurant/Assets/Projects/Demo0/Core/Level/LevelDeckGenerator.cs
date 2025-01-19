using System.Collections.Generic;
using Projects.Demo0.Core.Mgr;
using Projects.Demo0.Core.Utils.RndUtils;
namespace Projects.Demo0.Core.Level
{
public class LevelDeckGenerator
{
	public static List<DishCardObject> GenerateDeck(LevelDoc levelDoc)
	{
		var globalDeckDocList = GameDocMgr.Instance.m_GameGlobalConfig.DishDeck;
		var deck = new List<DishCardObject>();
		var selectCardCount = (int)(globalDeckDocList.Count * levelDoc.DishCardSelectRatio);
		var selectedIndexList = RndUtil.UniformSelect(selectCardCount, globalDeckDocList.Count);
		var expandedIndexList = RndUtil.ExpandDuplicate(selectedIndexList, levelDoc.GenDishCardTotalCount);
		var shuffledIndexList = RndUtil.LocalLikeShuffleSwap(selectedIndexList, GameDocMgr.Instance.m_GameGlobalConfig.ShuffleSwapCount);
		foreach (var selectedIndex in selectedIndexList)
		{
			var cardDoc = globalDeckDocList[selectedIndex];
			var cardObj = DishCardObject.Create(cardDoc, levelDoc);
			deck.Add(cardObj);
		}
		return deck;
	}
}
}