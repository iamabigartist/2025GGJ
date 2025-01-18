using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
namespace Projects.Demo0.Core.GameGlobal
{
[CreateAssetMenu]
public class GameGlobalConfig : SerializedScriptableObject
{
	public List<DishClueDoc> DishDeck = new();
	public int ShuffleSwapCount = 10;
	public int MaxHP = 5;
	public int AcceptWrongDish_HPChange = -2;
	public int RefusePerfectDish_HPChange = -1;
	public int AcceptPollutedCorrectDish_HPChange = 1;
}
}