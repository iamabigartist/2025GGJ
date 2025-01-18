using System.Collections;
using System.Collections.Generic;
using Projects.Demo0.Core.Localization;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class PPTController : SerializedMonoBehaviour
{
	[Header("按钮和内容控制")]
	public Image displayImage; // 图片组件
	public Text displayText; // 文本框组件
	public List<Sprite> images; // 图片数组，暴露为 List 供修改
	public List<MultiLangStr> texts = new(); // 文本数组，暴露为 List 供修改

	[Header("交互控制")]
	public float clickCooldown = 2f; // 点击后禁用的冷却时间（秒）
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

	void OnButtonClick()
	{
		if (isCooldown) return; // 如果按钮正在冷却，直接返回

		StartCoroutine(ButtonCooldown()); // 开始按钮冷却
		currentIndex++; // 更新点击次数

		if (currentIndex < maxClicks)
		{
			UpdateContent(); // 更新图片和文字内容
		}
		else
		{
			// 最后一次点击时加载场景
			if (!string.IsNullOrEmpty(sceneToLoad))
			{
				SceneManager.LoadScene(sceneToLoad); // 加载指定场景
			}

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
	}

	IEnumerator ButtonCooldown()
	{
		isCooldown = true; // 标记按钮进入冷却状态
		GetComponent<Button>().interactable = false; // 禁用按钮交互

		yield return new WaitForSeconds(clickCooldown); // 等待冷却时间

		GetComponent<Button>().interactable = true; // 恢复按钮交互
		isCooldown = false; // 冷却结束
	}
}