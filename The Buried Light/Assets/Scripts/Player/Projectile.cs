using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 2f;

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    private void OnEnable()
    {
        Invoke(nameof(DestroyProjectile), lifeTime);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void DestroyProjectile()
    {
        gameObject.SetActive(false);
    }
}
