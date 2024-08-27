using System;
using UnityEngine;

namespace Game.App
{
    [Serializable]
    public sealed class Component_IAPProduct : IComponent_IAPProduct
    {
        public string ProductId
        {
            get { return productId; }
        }

        [SerializeField]
        private string productId;
    }
}