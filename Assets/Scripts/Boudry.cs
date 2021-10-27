using UnityEngine;

public class Boudry : MonoBehaviour
{
    public enum Position
    {
        Vertical,
        Horizontal
    }

    public Position CurrentPosition => _position;

    [SerializeField] private float _teleportTo;
    [SerializeField] private Position _position;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform targetTransform;

        if (collision.CompareTag("Asteroid"))
            targetTransform = collision.GetComponentInParent<Rigidbody2D>().GetComponent<Transform>();
        else
            targetTransform = collision.GetComponent<Transform>();

        switch (_position)
        {
            case Position.Horizontal:
                targetTransform.position = new Vector3(targetTransform.position.x, _teleportTo, 0);
                break;
            case Position.Vertical:
                targetTransform.position = new Vector3(_teleportTo, targetTransform.position.y, 0);
                break;
        }

        if (targetTransform.TryGetComponent(out Asteroid asteroid))
        {
            asteroid.IncreaseTeleportNumber();

            if (asteroid.TeleportNumber >= 3)
            {
                Rigidbody2D rigidbody = targetTransform.GetComponent<Rigidbody2D>();
                rigidbody.velocity = Vector2.zero;
                rigidbody.angularVelocity = 0f;
                rigidbody.rotation += Random.Range(-50f, 50f) + 15f;
                rigidbody.AddRelativeForce(Vector2.up * Random.Range(1f, 2f), ForceMode2D.Impulse);
                asteroid.RestarTeleportNumber();
            }
        }
    }
}
