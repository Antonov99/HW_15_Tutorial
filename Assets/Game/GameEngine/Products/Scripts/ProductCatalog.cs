using System;
using UnityEngine;

namespace Game.GameEngine.Products
{
    [CreateAssetMenu(
        fileName = "ProductCatalog",
        menuName = "GameEngine/Products/New ProductCatalog"
    )]
    public sealed class ProductCatalog : ScriptableObject
    {
        public int ProductCount
        {
            get { return products.Length; }
        }

        [SerializeField]
        private ProductConfig[] products;

        public ProductConfig GetProduct(int index)
        {
            return products[index];
        }

        public ProductConfig FindProduct(string id)
        {
            for (int i = 0, count = products.Length; i < count; i++)
            {
                var item = products[i];
                if (item.Id == id)
                {
                    return item;
                }
            }

            throw new Exception($"Product with id {id} is not found!");
        }

        public ProductConfig[] GetAllProducts()
        {
            return products;
        }
    }
}