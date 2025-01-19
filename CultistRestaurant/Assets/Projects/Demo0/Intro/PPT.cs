using System.Collections;
using System.Collections.Generic;
using Projects.Demo0.Core.Localization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Audio;

public class PPTController : SerializedMonoBehaviour
{
	[Header("按钮和内容控制")]
	public Image displayImage; // 图片组件
	public Text displayText; // 文本框组件
	public List<Sprite> images; // 图片数组，暴露为 List 供修改
	public List<MultiLangStr> texts = new(); // 文本数组，暴露为 List 供修改
	public List<string> audioEvents = new(); // 音频事件名
	private List<uint> _audioIds = new(); // 记录一下音频uuid

	[Header("交互控制")]
	public List<float> clickCooldowns = new List<float>(); // 点击后禁用的冷却时间（秒）
	public string sceneToLoad; // 最后一击后加载的场景名称

	[Header("隐藏选项")]
	public bool hideThisButton = true; // 是否隐藏当前按钮 (A 按钮)
	public bool hideTextBox = false; // 是否隐藏文本框

	int currentIndex = 0; // 当前切换索引
	int maxClicks; // 最大点击次数
	bool isCooldown = false; // 冷却状态

	void Start()
	{
		maxClicks = Mathf.Min(images.Count, texts.Count); // 确定最大点击次数
		GetComponent<Button>().onClick.AddListener(OnButtonClick); // 绑定按钮点击事件
		UpdateContent(); // 初始化显示
	}

	public void LoadScene()
	{
		if (!string.IsNullOrEmpty(sceneToLoad))
		{
			SceneManager.LoadScene(sceneToLoad); // 加载指定场景
			foreach (var uuid in _audioIds)
			{
				AudioManager.Instance.StopPlayingID(uuid, 2000); // 停止所有可能正在播放的声音
			}
			AudioManager.Instance.PostEvent("Play_RestaurantBase", AudioManager.Instance.globalInitializer);
		}
	}

	void OnButtonClick()
	{
		if (isCooldown) return; // 如果按钮正在冷却，直接返回
		
		currentIndex++; // 更新点击次数
		StartCoroutine(ButtonCooldown()); // 开始按钮冷却

		if (currentIndex < maxClicks)
		{
			UpdateContent(); // 更新图片和文字内容
		}
		else
		{
			// 最后一次点击时加载场景
			LoadScene();

			// 隐藏其他组件（如果勾选了对应选项）
			if (hideThisButton)
				gameObject.SetActive(false); // 隐藏当前按钮

			if (hideTextBox && displayText != null)
				displayText.gameObject.SetActive(false); // 隐藏文本框
		}
	}

	void UpdateContent()
	{
		if (currentIndex < images.Count && displayImage != null)
		{
			displayImage.sprite = images[currentIndex]; // 更新图片
		}

		if (currentIndex < texts.Count && displayText != null)
		{
			displayText.text = texts[currentIndex].ToString(); // 更新文字
		}
		
		if (currentIndex < audioEvents.Count)
		{
			if (currentIndex == 2)
			{
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.IntroView); // 如果是第二页的话，暂停音乐
			}
			
			if (currentIndex > 1)
			{
				AudioManager.Instance.StopPlayingID(_audioIds[currentIndex-1], 2000); // 先停止上一个声音
			}

			if (currentIndex == maxClicks - 1)
			{
				AudioManager.Instance.SetStateValue(AudioManager.StateConstants.GameLevelGrp, AudioManager.StateConstants.GameLevelVal.Level01); // 最后一页的话，恢复音乐
			}
			
			uint uuid = AudioManager.Instance.PostEvent(audioEvents[currentIndex], AudioManager.Instance.globalInitializer); // 再播放当前声音
			_audioIds.Add(uuid);
		}
	}

	IEnumerator ButtonCooldown()
	{
		isCooldown = true; // 标记按钮进入冷却状态
		GetComponent<Button>().interactable = false; // 禁用按钮交互

		float cooldown = 1 ; //至少1秒
		if (currentIndex < clickCooldowns.Count && clickCooldowns[currentIndex] > cooldown)
		{
			cooldown = clickCooldowns[currentIndex];
		}
		Debug.Log($"currentIndex: {currentIndex}, cooldown: {cooldown}");
		yield return new WaitForSeconds(cooldown); // 等待冷却时间

		GetComponent<Button>().interactable = true; // 恢复按钮交互
		isCooldown = false; // 冷却结束
	}
}