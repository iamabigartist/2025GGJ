using UnityEngine;

public class SceneEffect : MonoBehaviour
{
    // 公共属性
    [SerializeField] 
    public int isSunk
    {
        get => _distort;
        set
        {
            _distort = value;
            UpdateDistortState();
        }
    }
    private int _distort;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateDistortState(); // 初始化时更新状态
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // 更新子节点的状态
    private void UpdateDistortState()
    {
        // 查找子节点中的distort节点
        Transform distortTransform = transform.Find("distort");
        if (distortTransform != null)
        {
            distortTransform.gameObject.SetActive(_distort == 1);
        }
    }
}
