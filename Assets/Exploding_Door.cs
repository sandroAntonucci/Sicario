using UnityEngine;

public class ExplodingDoor : MonoBehaviour
{
    public float explosionForce = 500f;
    public float explosionRadius = 5f;
    public float upwardsModifier = 0.5f;

    private bool hasExploded = false;

    private void OnTriggerEnter(Collider other)
    {
        //if (hasExploded) return;

        if (other.CompareTag("Player"))
        {
            Debug.Log("�Jugador entr� en el trigger!");
            Explode(other.transform.position);
            hasExploded = true;
        }
    }
    private void Explode(Vector3 explosionOrigin)
    {
        foreach (Transform piece in transform)
        {
            GameObject part = piece.gameObject;

            if (!part.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb = part.AddComponent<Rigidbody>();
                Debug.Log($"Rigidbody a�adido a {part.name}");
            }
            else
            {
                Debug.Log($"Rigidbody ya exist�a en {part.name}");
            }

            rb.AddExplosionForce(explosionForce, explosionOrigin, explosionRadius, upwardsModifier, ForceMode.Impulse);
        }

        Debug.Log("Explosi�n aplicada a todos los trozos.");
    }
}