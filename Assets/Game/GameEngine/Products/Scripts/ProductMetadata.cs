using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.GameEngine.Products
{
    [Serializable]
    public sealed class ProductMetadata : IProductMetadata
    {
        public Sprite Icon
        {
            get { return icon; }
        }

        public string Title
        {
            get { return title; }
        }

        public string Decription
        {
            get { return decription; }
        }

        [PreviewField]
        [SerializeField]
        private Sprite icon;

        [SerializeField]
        private string title;

        [TextArea]
        [SerializeField]
        private string decription;
    }
}