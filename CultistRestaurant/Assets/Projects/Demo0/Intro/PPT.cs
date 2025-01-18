using UnityEngine;
using UnityEngine.UI;

public class PPTController : MonoBehaviour
{
    public Button targetButton;      // 被控制显示和隐藏的目标按钮 (B 按钮)
    public Image displayImage;       // 图片组件
    public Text displayText;         // 文本框组件
    public Sprite[] images;          // 图片数组
    public string[] texts;           // 文本数组

    [Header("是否隐藏以下元素")]
    public bool hideThisButton = true;   // 是否隐藏当前按钮 (A 按钮)
    public bool hideTextBox = false;    // 是否隐藏文本框
    public bool hideTargetButton = false; // 是否隐藏目标按钮

    private int currentIndex = 0;    // 当前切换索引
    private int maxClicks;           // 最大点击次数

    void Start()
    {
        maxClicks = Mathf.Min(images.Length, texts.Length); // 确定最大点击次数
        GetComponent<Button>().onClick.AddListener(OnButtonClick); // 绑定按钮点击事件
        UpdateContent();                                     // 初始化显示
        if (hideTargetButton)
        {
            targetButton.gameObject.SetActive(false);        // 初始状态隐藏目标按钮
        }
    }

    void OnButtonClick()
    {
        currentIndex++;             // 更新索引
        if (currentIndex < maxClicks)
        {
            UpdateContent();        // 更新内容
        }
        else
        {
            // 根据用户选择隐藏对应元素
            if (hideThisButton)
                gameObject.SetActive(false); // 隐藏当前按钮 (A 按钮)

            if (hideTextBox && displayText != null)
                displayText.gameObject.SetActive(false); // 隐藏文本框

            if (targetButton != null)
                targetButton.gameObject.SetActive(true); // 显示目标按钮
        }
    }

    void UpdateContent()
    {
        if (displayImage != null)
            displayImage.sprite = images[currentIndex];  // 更新图片

        if (displayText != null)
            displayText.text = texts[currentIndex];      // 更新文字
    }
}
