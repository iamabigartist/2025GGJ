using UnityEngine;
using UnityEngine.UI;

public class PPTController : MonoBehaviour
{
    public Button targetButton;      // ��������ʾ�����ص�Ŀ�갴ť (B ��ť)
    public Image displayImage;       // ͼƬ���
    public Text displayText;         // �ı������
    public Sprite[] images;          // ͼƬ����
    public string[] texts;           // �ı�����

    [Header("�Ƿ���������Ԫ��")]
    public bool hideThisButton = true;   // �Ƿ����ص�ǰ��ť (A ��ť)
    public bool hideTextBox = false;    // �Ƿ������ı���
    public bool hideTargetButton = false; // �Ƿ�����Ŀ�갴ť

    private int currentIndex = 0;    // ��ǰ�л�����
    private int maxClicks;           // ���������

    void Start()
    {
        maxClicks = Mathf.Min(images.Length, texts.Length); // ȷ�����������
        GetComponent<Button>().onClick.AddListener(OnButtonClick); // �󶨰�ť����¼�
        UpdateContent();                                     // ��ʼ����ʾ
        if (hideTargetButton)
        {
            targetButton.gameObject.SetActive(false);        // ��ʼ״̬����Ŀ�갴ť
        }
    }

    void OnButtonClick()
    {
        currentIndex++;             // ��������
        if (currentIndex < maxClicks)
        {
            UpdateContent();        // ��������
        }
        else
        {
            // �����û�ѡ�����ض�ӦԪ��
            if (hideThisButton)
                gameObject.SetActive(false); // ���ص�ǰ��ť (A ��ť)

            if (hideTextBox && displayText != null)
                displayText.gameObject.SetActive(false); // �����ı���

            if (targetButton != null)
                targetButton.gameObject.SetActive(true); // ��ʾĿ�갴ť
        }
    }

    void UpdateContent()
    {
        if (displayImage != null)
            displayImage.sprite = images[currentIndex];  // ����ͼƬ

        if (displayText != null)
            displayText.text = texts[currentIndex];      // ��������
    }
}
