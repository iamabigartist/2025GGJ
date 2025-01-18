using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;
using Random = UnityEngine.Random;
namespace Projects.Demo0.Core.Utils.RndUtils
{
public static class RndSelect
{
	/// <summary>
	///     给定数量的元素中随机选择其中n个，并且均匀分布
	/// </summary>
	public static List<int> UniformSelect(int total, int selectCount)
	{
		var result = new List<int>();
		var partition = CeilToInt((float)total / selectCount);
		for (var i = 0; i < selectCount; i++)
		{
			result.Add(Random.Range(i * partition, Min((i + 1) * partition, total)));
		}
		return result;
	}

	/// <summary>
	///     对于容器中元素随机进行位置互换，位置越接近的元素互换概率越大
	/// </summary>
	/// <returns>原列表的副本</returns>
	public static List<int> LocalLikeShuffleSwap(List<int> itemList, int swapNum)
	{
		Debug.Log($"LocalLikeShuffleSwap: {itemList.Count} {swapNum}");
		var result = new List<int>(itemList);
		for (var i = 0; i < swapNum; i++)
		{
			var idx0 = Random.Range(0, itemList.Count);
			var idx1 = Random.Range(0, itemList.Count);
			var swapProb = 1 - Abs(idx0 - idx1) / (float)itemList.Count;
			if (swapProb < Random.value) { continue; }
			(result[idx0], result[idx1]) = (result[idx1], result[idx0]);
		}
		return result;
	}
}
}