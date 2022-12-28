using Mirror;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickName;
    public int Index;
    [SyncVar]
    public string NickName;
    public override void OnStartClient()
    {
        _nickName.text = NickName;
    }
 


}
