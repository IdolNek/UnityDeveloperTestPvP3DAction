using Mirror;
using TMPro;
using UnityEngine;

public class NetworkGamePlayer : NetworkBehaviour
{
    [SerializeField] private TextMeshProUGUI _nickName;
    public int Index;
    [SyncVar]
    public string NickName;
    [SyncVar]
    public int Score;

    private void Start()
    {
        _nickName.text = NickName;
    }
}
