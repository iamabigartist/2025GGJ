using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PPTController : MonoBehaviour
{
    [Header("��ť�����ݿ���")]
    public Image displayImage;                // ͼƬ���
    public Text displayText;                  // �ı������
    public List<Sprite> images;               // ͼƬ���飬��¶Ϊ List ���޸�
    public List<string> texts;                // �ı����飬��¶Ϊ List ���޸�

    [Header("��������")]
    public float clickCooldown = 2f;          // �������õ���ȴʱ�䣨�룩
    public string sceneToLoad;                // ���һ������صĳ�������

    [Header("����ѡ��")]
    public bool hideThisButton = true;        // �Ƿ����ص�ǰ��ť (A ��ť)
    public bool hideTextBox = false;          // �Ƿ������ı���

    private int currentIndex = 0;             // ��ǰ�л�����
    private int maxClicks;                    // ���������
    private bool isCooldown = false;          // ��ȴ״̬

    void Start()
    {
        maxClicks = Mathf.Min(images.Count, texts.Count); // ȷ�����������
        GetComponent<Button>().onClick.AddListener(OnButtonClick); // �󶨰�ť����¼�
        UpdateContent();                                  // ��ʼ����ʾ
    }

    void OnButtonClick()
    {
        if (isCooldown) return;  // �����ť������ȴ��ֱ�ӷ���

        StartCoroutine(ButtonCooldown()); // ��ʼ��ť��ȴ
        currentIndex++;                   // ���µ������

        if (currentIndex < maxClicks)
        {
            UpdateContent();              // ����ͼƬ����������
        }
        else
        {
            // ���һ�ε��ʱ���س���
            if (!string.IsNullOrEmpty(sceneToLoad))
            {
                SceneManager.LoadScene(sceneToLoad); // ����ָ������
            }

            // ������������������ѡ�˶�Ӧѡ�
            if (hideThisButton)
                gameObject.SetActive(false); // ���ص�ǰ��ť

            if (hideTextBox && displayText != null)
                displayText.gameObject.SetActive(false); // �����ı���
        }
    }

    void UpdateContent()
    {
        if (currentIndex < images.Count && displayImage != null)
        {
            displayImage.sprite = images[currentIndex];  // ����ͼƬ
        }

        if (currentIndex < texts.Count && displayText != null)
        {
            displayText.text = texts[currentIndex];      // ��������
        }
    }

    IEnumerator ButtonCooldown()
    {
        isCooldown = true; // ��ǰ�ť������ȴ״̬
        GetComponent<Button>().interactable = false; // ���ð�ť����

        yield return new WaitForSeconds(clickCooldown); // �ȴ���ȴʱ��

        GetComponent<Button>().interactable = true; // �ָ���ť����
        isCooldown = false; // ��ȴ����
    }
}
