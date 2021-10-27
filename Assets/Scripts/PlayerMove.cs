using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float _mouseRotateSpeed;
    [SerializeField] private float _acceleration;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private GameObject _engineFire;
    [SerializeField] private float _maximumSpeed;

    private Rigidbody2D _rigidbody;
    private InputFromPlayer _input;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (_input.IsAlternativeControl)
        {
            MouseRotate();
        }

        PlayThrustSound();
        ActivateEngineFire();
    }

    private void PlayThrustSound()
    {
        if (_input.Accel > 0)
        {
            SoundController.Instance.PlayLoopingSound();
        }
    }

    private void FixedUpdate()
    {
        Move();
        LimitingSpeed();
    }



    public void Init(InputFromPlayer input)
    {
        _input = input;
    }

    public void StopMovement()
    {
        _rigidbody.angularVelocity = 0f;
        _rigidbody.velocity = Vector2.zero;
    }

    private void LimitingSpeed()
    {
        if (_rigidbody.velocity.sqrMagnitude > _maximumSpeed)
        {
            _rigidbody.velocity *= 0.99f;
        }
    }

    private void Move()
    {
        _rigidbody.AddRelativeForce(Vector2.up * Mathf.Clamp(_input.Accel, 0f, 1f) * Time.fixedDeltaTime * _acceleration);
        if (!_input.IsAlternativeControl)
        {
            _rigidbody.AddTorque(_input.Rotate * Time.fixedDeltaTime * _rotationSpeed);
        }
    }

    private void MouseRotate()
    {
        Vector3 aimDirection = (_input.GetMouseWorldPosition() - transform.position).normalized;
        float angle = Mathf.Atan2(aimDirection.x, aimDirection.y) * Mathf.Rad2Deg;
        Quaternion finalRotation = Quaternion.Euler(0, 0, -angle);
        transform.rotation = Quaternion.SlerpUnclamped(transform.rotation, finalRotation, _mouseRotateSpeed * Time.deltaTime);
    }

    private void ActivateEngineFire()
    {
        if (_input.Accel > 0 && _engineFire.activeInHierarchy == false)
        {
            _engineFire.SetActive(true);
        }
        else if (_input.Accel == 0 && _engineFire.activeInHierarchy == true)
        {
            _engineFire.SetActive(false);
        }
    }
}
