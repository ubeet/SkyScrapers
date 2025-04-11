using TMPro;
using UnityEngine;

public class RulePresenter2D : RulePresenter
{
    

    public override void Set(int dataRuleCell, bool force)
    {
        base.Set(dataRuleCell, force);
        text.text = dataRuleCell == 0 ? "" : dataRuleCell.ToString();
    }

    public override void Reset()
    {
        text.text = "";
    }
}