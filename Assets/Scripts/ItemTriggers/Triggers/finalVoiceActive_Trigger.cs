using UnityEngine.Events;
using UnityEngine;
using System.Collections;
public class finalVoiceActive_Trigger : ITrigger
{
    public RecorderController recorderController;
    public override void Action()
    {
        recorderController.active = true;
    }
   
}