using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    float hitPoints;
    public int damageStrenght;
    Coroutine damageCoroutine;

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints = hitPoints - damage;

            if (hitPoints <= float.Epsilon)
            {
                KillCharacter();
                break;
            }
            if (interval > float.Epsilon)
            {
                yield return new WaitForSeconds(interval);
            }
            else
            {
                break;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();

            if (damageCoroutine == null)
            {
                damageCoroutine = StartCoroutine(player.DamageCharacter(damageStrenght, 1.0f));
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (damageCoroutine != null)
            {
                StopCoroutine(damageCoroutine);
                damageCoroutine = null;
            }
        }
    }

    public override void ResetCharacter()
    {
        hitPoints = maxHitPoints;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
