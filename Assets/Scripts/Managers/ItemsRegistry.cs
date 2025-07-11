using Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Utility;

namespace Managers
{
    class ItemsRegistry : MonoBehaviour
    {
        private bool isInitialized;
        public static ItemsRegistry Instance { get; private set; }
        private readonly List<Item> Items = new();


        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Debug.LogWarning("A duplicate GameManager instance was found. Destroying it.");
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            if(!isInitialized)
            {
                isInitialized = true;
                LoadItems();
            }
        }
        public Item GetItemByName(string name) => Items.FirstOrDefault(i => i.Name == name);
        public void AddItem(Item item) => Items.Add(item);



        public void LoadItems()
        {
            string json = File.ReadAllText(Application.dataPath + "/Configs/Items.json");
            var data = JsonUtility.FromJson<ItemsData>(json);

            foreach (var itemData in data.Items)
            {
                var sprite = Resources.Load<Sprite>(itemData.ImagePath);
                var item = new Item(itemData.Name, sprite, 1);
                AddItem(item);
            }
        }
    }
}
