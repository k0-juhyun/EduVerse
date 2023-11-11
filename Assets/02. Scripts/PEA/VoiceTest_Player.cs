using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class VoiceTest_Player : MonoBehaviourPun
{
    private Photon.Voice.Unity.Recorder recorder;

    public void MuteStudents(bool useStudenetsMike)
    {
        photonView.RPC(nameof(RPCMuteStudents), RpcTarget.Others, useStudenetsMike);
    }

    [PunRPC]
    public void RPCMuteStudents(bool useStudentsMike)
    {
        Voice.instance.Mute(useStudentsMike);
    }
}
