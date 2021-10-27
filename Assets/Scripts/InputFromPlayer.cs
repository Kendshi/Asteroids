using UnityEngine;
using System;

public class InputFromPlayer : MonoBehaviour
{
    public event Action Shoot;
    public event Action EnableMenu;

    public float Accel => _accel;
    public float Rotate => _rotate;
    public bool IsAlternativeControl => _isAlternativeControl;

    private float _accel;
    private float _rotate;
    private bool _isAlternativeControl;

    void Update()
    {
        if (!_isAlternativeControl)
        {
            _rotate = Input.GetAxis("Horizontal");
            _accel = Input.GetAxis("Vertical");
            if (Input.GetButtonDown("Fire1"))
            {
                Shoot?.Invoke();
            }
        }
        else
        {
            _accel = Mathf.Clamp(Input.GetAxis("Vertical") + Input.GetAxis("Mouse acceleration"), 0, 1);
            if (Input.GetButtonDown("Fire2"))
            {
                Shoot?.Invoke();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EnableMenu?.Invoke();
        }
    }

    public bool ChangeControlMode()
    {
        if (_isAlternativeControl)
            _isAlternativeControl = false;
        else
            _isAlternativeControl = true;

        return _isAlternativeControl;
    }

    public Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        return mousePosition;
    }
}
