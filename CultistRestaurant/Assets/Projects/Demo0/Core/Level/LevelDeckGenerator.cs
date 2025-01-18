using System.Collections.Generic;
using Projects.Demo0.Core.DishCard.Objects;
using Projects.Demo0.Core.Mgr;
using Projects.Demo0.Core.Utils.RndUtils;
namespace Projects.Demo0.Core.Level
{
public class LevelDeckGenerator
{
	public static List<DishClueObject> GenerateDeck(LevelDoc levelDoc)
	{
		var globalDeckDocList = GameDocMgr.Instance.m_GameGlobalConfig.DishDeck;
		var deck = new List<DishClueObject>();
		var selectedIndexList = RndSelect.UniformSelect(levelDoc.CardCount, globalDeckDocList.Count);
		var shuffledIndexList = RndSelect.LocalLikeShuffleSwap(selectedIndexList, GameDocMgr.Instance.m_GameGlobalConfig.ShuffleSwapCount);
		foreach (var selectedIndex in selectedIndexList)
		{
			var cardDoc = globalDeckDocList[selectedIndex];
			var cardObj = DishCardObject.Create(cardDoc, levelDoc);
		}
		return deck;
	}
}
}