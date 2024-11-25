using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance;
    public List<Item> Item = new List<Item>();
    public List<Item> Iventory = new List<Item>();

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
                if (Iventory.Count == 0 && Input.anyKeyDown)
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
        Iventory.Add(item);
        DataManager.instance.currentData.items[item.id] = true;
    }

    public void RemoveItem(Item item)
    {
        Iventory.Remove(item);
        DataManager.instance.currentData.items[item.id] = false;
    }

    private void LoadItem()
    {
        for (int i = 0; i < DataManager.instance.currentData.items.Length; i++)
        {
            if (DataManager.instance.currentData.items[i] && !Iventory.Contains(Item[i]))
            {
                AddItem(Item[i]);
            }
        }
    }

    public void ListItems()
    {
        LoadItem();

        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in Iventory)
        {
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemName = obj.transform.Find("Item Name").GetComponent<TextMeshProUGUI>();
            var itemIcon = obj.transform.Find("Item Icon").GetComponent<Image>();           

            itemName.text = item.itemName;
            itemIcon.sprite = item.icon;         
        }

        if (Iventory.Count != 0)
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
        instance.itemDescText.text = instance.Iventory[num].itemDesc;
    }      

    public void ItemDescTextClear()
    {
        instance.itemDescText.text = "";
    }
}
