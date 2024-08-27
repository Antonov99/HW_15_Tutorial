using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Products
{
    public class ProductBuyer
    {
        public event Action<Product> OnBuyStarted; 

        public event Action<Product> OnBuyCompleted;

        private readonly List<IProductBuyCondition> conditions;

        private readonly List<IProductBuyProcessor> processors;

        private readonly List<IProductBuyCompletor> completors;

        public ProductBuyer()
        {
            conditions = new List<IProductBuyCondition>();
            processors = new List<IProductBuyProcessor>();
            completors = new List<IProductBuyCompletor>();
        }

        public bool CanBuyProduct(Product product)
        {
            for (int i = 0, count = conditions.Count; i < count; i++)
            {
                var condition = conditions[i];
                if (!condition.CanBuy(product))
                {
                    return false;
                }
            }

            return true;
        }
        
        [Button]
        public void BuyProduct(ProductConfig config)
        {
            BuyProduct(config.Prototype);
        }

        public void BuyProduct(Product product)
        {
            if (!CanBuyProduct(product))
            {
                Debug.LogWarning($"Can't buy product {product.Id}!");
                return;
            }
            
            OnBuyStarted?.Invoke(product);
            
            //Process buy:
            for (int i = 0, count = processors.Count; i < count; i++)
            {
                var processor = processors[i];
                processor.ProcessBuy(product);
            }
            
            //Complete buy:
            for (int i = 0, count = completors.Count; i < count; i++)
            {
                var completor = completors[i];
                completor.CompleteBuy(product);
            }
            
            OnBuyCompleted?.Invoke(product);
        }

        public void AddCondition(IProductBuyCondition condition)
        {
            conditions.Add(condition);
        }

        public void RemoveChecker(IProductBuyCondition condition)
        {
            conditions.Remove(condition);
        }

        public void AddProcessor(IProductBuyProcessor processor)
        {
            processors.Add(processor);
        }

        public void RemoveProcessor(IProductBuyProcessor processor)
        {
            processors.Remove(processor);
        }

        public void AddCompletor(IProductBuyCompletor completor)
        {
            completors.Add(completor);
        }

        public void RemoveCompletor(IProductBuyCompletor completor)
        {
            completors.Remove(completor);
        }
    }
}