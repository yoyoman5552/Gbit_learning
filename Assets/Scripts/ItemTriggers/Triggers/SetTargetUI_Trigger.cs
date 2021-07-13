using UnityEngine.UI;
public class SetTargetUI_Trigger : ITrigger
{
    public string changeTargetDetail;
    private Text targetText;
    private void Start()
    {
        targetText = UIManager.Instance.targetUI.GetComponent<Text>();
    }
    public override void Action()
    {
        //FIXME：目标的展现形式调整
        targetText.text = changeTargetDetail;
        //UIManager.Instance
    }
}
