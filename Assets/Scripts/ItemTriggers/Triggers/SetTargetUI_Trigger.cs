using UnityEngine.UI;
public class SetTargetUI_Trigger : ITrigger
{
    public string changeTargetDetail;
    //private Text targetText;
    private void Start()
    {
  //      targetText = UIManager.Instance.targetUI.GetComponentInChildren<Text>();
    }
    public override void Action()
    {
        UIManager.Instance.SetTargetUI(changeTargetDetail);
        //FIXME：目标的展现形式调整
//        targetText.text = changeTargetDetail;
        //UIManager.Instance
    }
}
