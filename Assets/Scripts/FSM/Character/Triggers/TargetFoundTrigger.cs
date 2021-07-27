using EveryFunc;
using UnityEngine;
public class TargetFoundTrigger : FSMTrigger
{
    public override void Init()
    {
        triggerID = FSMTriggerID.TargetFound;
    }
    public override bool HandleTrigger(FSMBase fsm)
    {
        /*         if (CheckTargetInLimit (fsm))
                    Debug.Log ("checktargetLimit:" + CheckTargetInLimit (fsm));
         */
        return fsm.targetTF != null &&
        !GridManager.Instance.IsStandAWall(fsm.targetTF.position) &&
         GridManager.Instance.FindPath(fsm.targetTF.position, fsm.transform.position) != null;
    }
    /* public bool CheckTargetInLimit(FSMBase fsm)
    {
        if (fsm.targetTF == null) return false;
        //Debug.Log ("fsm:" + fsm.transform.position + ",leftdown:" + GridManager.Instance.LeftDownTF.position + ",right:" + GridManager.Instance.RightUpTF.position);
        if (fsm.targetTF.transform.position.x > GridManager.Instance.RightUpTF.position.x ||
            fsm.targetTF.transform.position.x < GridManager.Instance.LeftDownTF.position.x ||
            fsm.targetTF.transform.position.y > GridManager.Instance.RightUpTF.position.y ||
            fsm.targetTF.transform.position.y < GridManager.Instance.LeftDownTF.position.y) return false;
        return true;
    } */

}