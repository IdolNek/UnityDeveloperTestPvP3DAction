using Mirror;
using UnityEngine;
public class PlayerInput : NetworkBehaviour
{
    [SerializeField] private PlayerAction _playerAction;
    [SerializeField] private Camera _camera;
    [SerializeField] private AudioListener _audioListener;
    private Vector3 _moveDirection;
    private bool _cursorVisible = true;
    public override void OnStartLocalPlayer()
    {
        _playerAction.enabled = true;
        _camera.enabled = true;
        _audioListener.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        if (!isLocalPlayer) return;
        GetMoveDirection();
        if (Input.GetMouseButtonUp(0)) _playerAction.Attack();
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Cursor.lockState = _cursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
    private void FixedUpdate()
    {
        if (!isLocalPlayer) return;
        if(_playerAction != null) _playerAction.MovePlayer(_moveDirection);
    }
    private void GetMoveDirection()
    {
        if (_camera == null) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 movementInput = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);
        _moveDirection = movementInput.normalized;
    }
}
