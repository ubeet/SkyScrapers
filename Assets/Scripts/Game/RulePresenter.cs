using TMPro;
using UnityEngine;

public class RulePresenter : MonoBehaviour
{
    [SerializeField] public TMP_Text text;
    [SerializeField] public RectTransform image;
    protected int _index;

    public virtual void Init(int index)
    {
        
        _index = index;
    }
    private void Awake()
    {
        Reset();
    }

    public virtual void Reset()
    {
    }

    public virtual void Set(int dataRuleCell, bool force)
    {
        if (dataRuleCell == 0)
        {
            gameObject.SetActive(false);
        }
    }
}