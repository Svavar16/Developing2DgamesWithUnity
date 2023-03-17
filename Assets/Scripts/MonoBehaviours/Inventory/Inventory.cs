using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPreFab;
    public const int numSlot = 5;
    Image[] itemImage = new Image[numSlot];
    Item[] items = new Item[numSlot];
    GameObject[] slots = new GameObject[numSlot];
    // Start is called before the first frame update
    void Start()
    {
        CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateSlots()
    {
        if (slotPreFab != null)
        {
            for (int i = 0; i < numSlot; i++)
            {
                GameObject newSlot = Instantiate(slotPreFab);
                newSlot.name = "ItemSlot_" + i;

                newSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);

                slots[i] = newSlot;

                itemImage[i] = newSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    public bool AddItem(Item itemToAdd)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].itemType == itemToAdd.itemType && itemToAdd.stackable == true)
            {
                items[i].quantity = items[i].quantity + 1;

                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();

                TextMeshProUGUI quantityText = slotScript.qtyText;

                quantityText.enabled = true;

                quantityText.text = items[i].quantity.ToString();

                return true;
            }

            if (items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantity = 1;
                itemImage[i].sprite = itemToAdd.sprite;
                itemImage[i].enabled = true;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();

                TextMeshProUGUI quantityText = slotScript.qtyText;

                quantityText.enabled = true;

                quantityText.text = items[i].quantity.ToString();

                return true;
            }
        }
        return false;
    }
}
