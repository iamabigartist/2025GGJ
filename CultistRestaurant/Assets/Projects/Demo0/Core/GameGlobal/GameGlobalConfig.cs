using System.Collections.Generic;
using Projects.Demo0.Core.Level;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.GameGlobal
{
[CreateAssetMenu]
public class GameGlobalConfig : SerializedScriptableObject
{
	[Title("Deck")]
	public List<DishClueDoc> DishDeck = new();
	public int ShuffleSwapCount = 10;

	[Title("HP")]
	public int MaxHP = 5;
	public int AcceptWrongDish_HPChange = -2;
	public int RefusePerfectDish_HPChange = -1;
	public int AcceptPollutedCorrectDish_HPChange = 1;

	[Title("Level")]
	public List<LevelDoc> LevelList = new();

	[Title("StoryEnd")]
	public float GoodEnd_AcceptPolluteRatioRequired = 0.5f;
}
}