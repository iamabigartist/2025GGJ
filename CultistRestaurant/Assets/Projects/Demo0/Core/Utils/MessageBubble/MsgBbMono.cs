using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
public class MsgBbMono : SerializedMonoBehaviour
{
	public float Lifetime = 10f;
	public bool Fading = false;
	public float FadeTime = 0.5f;
	public AnimationCurve TransparentCurve;
	public List<Graphic> GraphicsList = new();
	List<Color> OriColorList;

	void Start() => Invoke(nameof(StartFade), Lifetime);
	void StartFade()
	{
		Fading = true;
		OriColorList = GraphicsList.Select(g => g.color).ToList();
	}
	void Update()
	{
		if (Fading)
		{
			FadeTime -= Time.deltaTime;
			if (FadeTime <= 0) { Destroy(gameObject); }
			else
			{
				float ratio = TransparentCurve.Evaluate(FadeTime);
				var alphaColorList = OriColorList.Select(ori =>
				{
					var c = ori;
					c.a *= ratio;
					return c;
				}).ToList();
				for (int i = 0; i < GraphicsList.Count; i++) { GraphicsList[i].color = alphaColorList[i]; }
			}
		}
	}
}