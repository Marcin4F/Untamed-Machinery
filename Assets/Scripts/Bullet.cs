using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayerMask;              // layer colliderow
    public GameObject hitParticles;

    private Vector3 lastPosition, currentPosition;
    private float lifeTime = 0.0f;                              // czas �ycia pocisku
    [SerializeField] float bulletSpeed = 40.0f;

    private int shotDamage;

    public void SetShotDamage(int damage) // setter do ustawienia obrazen od przeciwnika
    {
        shotDamage = damage;
    }

    void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        lastPosition = transform.position;
        transform.Translate(bulletSpeed * Time.deltaTime * Vector3.forward);
        lifeTime += Time.deltaTime;
        if (lifeTime > 5.0f)        // niszczenie pociskow zycjacych wiecej niz 5 sekund
        {
            Destroy(gameObject);
        }

        DetectRaycastHit();
    }

    void DetectRaycastHit()
    {
        currentPosition = transform.position;
        if (Physics.Linecast(lastPosition, currentPosition, out var rayCastHit, hitLayerMask))       // raycast od obecnej pozycji od ostatniej pozycj, jezeli cos trafil znaczy ze pocisk trafil w obiekt
        {
            // MARKERY
            Instantiate(hitParticles, rayCastHit.point, Quaternion.identity);        // zainicjalizowanie markera trafienia (pozniej zmienic na efekt wizualny np. particle
            Destroy(gameObject);
            Enemy enemy = rayCastHit.transform.GetComponent<Enemy>();
            if (enemy != null && enemy.isAlive)
            {
                enemy.TakeDamage(Player.instance.weaponDamage);
            }

            Player player = rayCastHit.transform.GetComponentInParent<Player>();
            if (player != null && player.alive)
            {
                //enemy = GetComponentInParent<Enemy>();
                Debug.Log("gracz trafiony");
                player.TakeDamage(shotDamage);
            }
        }
    }
}
