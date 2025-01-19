using System.Collections.Generic;
using Projects.Demo0.Core.Level;
using Projects.Demo0.Core.Localization;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.GameGlobal
{
[CreateAssetMenu]
public class GameGlobalConfig : SerializedScriptableObject
{
	[Title("Deck")]
	public GameObject DishCardPrefab;
	public List<DishCardDoc> DishDeck = new();
	public int ShuffleSwapCount = 10;

	[Title("HP")]
	public int MaxHP = 5;
	public int AcceptWrongDish_HPChange = -2;
	public int RefusePerfectDish_HPChange = -1;
	public int AcceptPollutedCorrectDish_HPChange = 1;
	public MultiLangStr AcceptWrongDishDesc = new();
	public MultiLangStr RefusePerfectDishDesc = new();
	public MultiLangStr AcceptPollutedCorrectDishDesc = new();
	public MultiLangStr AcceptNormalDishDesc = new();
	public MultiLangStr RefuseNormalDishDesc = new();

	[Title("Level")]
	public MultiLangStr DayEndStr = new();
	public List<LevelDoc> LevelList = new();

	[Title("StoryEnd")]
	public float GoodEnd_AcceptPolluteRatioRequired = 0.5f;
	public EndTextDesc DeadEnd_Desc = new();
	public EndTextDesc GoodEnd_Desc = new();
	public EndTextDesc BadEnd_Desc = new();

	[Title("Default")]
	public Sprite DefaultDishSprite;
}
}