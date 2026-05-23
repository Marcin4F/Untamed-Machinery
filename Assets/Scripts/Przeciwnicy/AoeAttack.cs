using System.Collections;
using UnityEngine;

public class AoeAttack : MonoBehaviour
{
    Collider aoeCollider;
    Renderer aoeRenderer;

    [SerializeField] int damage = 10;
    [SerializeField] float hitAlpha = 0.5f;
    [SerializeField] float windupAlpha = 0.1f;


    void Awake()
    {
        aoeCollider = GetComponent<Collider>();
        aoeCollider.enabled = false;

        aoeRenderer = GetComponent<Renderer>();
        aoeRenderer.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        Player player = other.gameObject.GetComponent<Player>();

        if (player == null)
            return;

        player.TakeDamage(damage);
    }

    public void StartWindup()
    {
        aoeCollider.enabled = false;
        aoeRenderer.enabled = true;

        StartCoroutine(WindupCoroutine());
    }

    public void Attack()
    {
        aoeCollider.enabled = true;

        StartCoroutine(AttackCoroutine());
    }

    IEnumerator WindupCoroutine()
    {
        float fadeSpeed = 0.2f;
        float alpha = 0.0f;

        while (alpha < windupAlpha)
        {
            aoeRenderer.material.color = new Color(
                aoeRenderer.material.color.r,
                1.0f, 1.0f, alpha
            );

            alpha += Time.deltaTime * fadeSpeed;

            yield return null;
        }
    }


    IEnumerator AttackCoroutine()
    {
        float fadeSpeed = 1.0f;
        float alpha = hitAlpha;

        while (alpha > 0.0f)
        {
            aoeRenderer.material.color = new Color(
                aoeRenderer.material.color.r,
                0.0f, 0.0f, alpha
            );

            alpha -= Time.deltaTime * fadeSpeed;

            yield return null;
        }

        aoeCollider.enabled = false;
        aoeRenderer.enabled = false;
    }

}
