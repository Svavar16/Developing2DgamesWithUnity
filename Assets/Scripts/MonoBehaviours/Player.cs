using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    public HitPoints hitPoints;
    public HealthBar healthBarPrefab;
    public Inventory inventoryPrefab;
    HealthBar healthBar;
    Inventory inventory;

    void Start()
    {
        ResetCharacter();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("CanBePickedUp"))
        {
            Item hitItem = collision.gameObject.GetComponent<Consumable>().item;
            if (hitItem != null)
            {
                bool shouldDisapper = false;
                switch (hitItem.itemType)
                {
                    case Item.ItemType.COIN:
                        shouldDisapper = inventory.AddItem(hitItem);
                        break;
                    case Item.ItemType.HEALTH:
                        shouldDisapper = AdjustHitPoint(hitItem.quantity);
                        break;
                    default:
                        break;
                }
                print("it: " + hitItem.objectName);
                if (shouldDisapper)
                {
                    collision.gameObject.SetActive(false);
                }
            }
            //Destroy(collision.gameObject);
        }
    }
    public bool AdjustHitPoint(int amount)
    {
        if (hitPoints.value < maxHitPoints)
        {
            hitPoints.value = hitPoints.value + amount;
            print("Adjusted hitpoints by: " + amount + ". New value: " + hitPoints.value);
            return true;
        }
        return false;
    }

    public override void ResetCharacter()
    {
        inventory = Instantiate(inventoryPrefab);
        healthBar = Instantiate(healthBarPrefab);
        healthBar.character = this;

        hitPoints.value = startingHitPoints;
    }

    public override void KillCharacter()
    {
        base.KillCharacter();

        Destroy(healthBar.gameObject);
        Destroy(inventory.gameObject);
    }

    public override IEnumerator DamageCharacter(int damage, float interval)
    {
        while (true)
        {
            hitPoints.value = hitPoints.value - damage;

            if (hitPoints.value < float.Epsilon)
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
}
