using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> Items = new List<Item>();

    public Transform ItemContent;
    public GameObject InventoryItem;
    public GameObject ResumeSelectBtn;
    public TextMeshProUGUI itemDescText;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator ButtonControl()
    {
        while (true)
        {
            if (EventSystem.current.currentSelectedGameObject == null)
            {
                if (Items.Count == 0 && Input.anyKeyDown)
                {
                    EventSystem.current.SetSelectedGameObject(ResumeSelectBtn);
                }
                else if (Input.anyKeyDown)
                {
                    EventSystem.current.SetSelectedGameObject(ItemContent.transform.GetChild(0).gameObject);
                }
            }
            yield return null;
        }
    }

    public void AddItem(Item item)
    {
        Items.Add(item);        
    }

    public void RemoveItem(Item item)
    {
        Items.Remove(item);
    }

    public void ListItems()
    {
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Items)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Item Name").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("Item Icon").GetComponent<Image>();           

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;         
        }

        if (Items.Count != 0)
        {
            EventSystem.current.SetSelectedGameObject(ItemContent.transform.GetChild(0).gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(ResumeSelectBtn);
        }
        StartCoroutine(ButtonControl());
    }

    public void ItemDescTextType(int num)
    {
        instance.itemDescText.text = instance.Items[num].itemDesc;
    }      

    public void ItemDescTextClear()
    {
        instance.itemDescText.text = "";
    }
}
