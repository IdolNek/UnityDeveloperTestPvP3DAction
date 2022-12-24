using UnityEngine;

public class HealthBarAndNickNameUIToFace : MonoBehaviour
{
    private Camera _cam;
    private void Start()
    {
        _cam = Camera.main;
    }
    private void Update()
    {
        transform.LookAt(transform.position+ _cam.transform.rotation * Vector3.forward, _cam.transform.rotation * Vector3.up);
    }
}
