using UnityEngine;
using UnityEngine.UI;
public class MsgBbMono : MonoBehaviour
{
	public float Lifetime = 10f;
	public bool Fading = false;
	public float FadeTime = 0.5f;
	public AnimationCurve TransparentCurve;
	public Text msgText;
	public Image bgImage;

	void Start() => Invoke(nameof(StartFade), Lifetime);
	void StartFade()
	{
		msgText = GetComponentInChildren<Text>();
		bgImage = GetComponentInChildren<Image>();
		Fading = true;
	}
	void Update()
	{
		if (Fading)
		{
			FadeTime -= Time.deltaTime;
			if (FadeTime <= 0) { Destroy(gameObject); }
			else
			{
				float alpha = TransparentCurve.Evaluate(FadeTime);
				var (txtColor, bgColor) = (msgText.color, bgImage.color);
				txtColor.a = bgColor.a = alpha;
				(msgText.color, bgImage.color) = (txtColor, bgColor);
			}
		}
	}
}