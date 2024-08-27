#if UNITY_PURCHASING
using System;
using System.Collections.Generic;
using UnityEngine.Purchasing;

namespace Purchasing
{
    public sealed class PurchaseManager : IStoreListener
    {
        public bool IsInitialized { get; private set; }

        private IStoreController storeController;

        private readonly List<ICompleteListener> completeListeners;

        private readonly List<IFailListener> failListeners;

        private bool isLoading;

        private bool isPurchasing;

        private Action<InitResult> initCallback;

        private Action<PurchaseResult> purchaseCallback;

        public PurchaseManager()
        {
            failListeners = new List<IFailListener>();
            completeListeners = new List<ICompleteListener>();
        }

        #region Initialize

        public void Initialize(Action<InitResult> callback = null)
        {
            if (isLoading)
            {
                throw new Exception("Unity purchasing is already loading!");
            }

            if (IsInitialized)
            {
                throw new Exception("Unity Purchasing is already initialized");
            }

            initCallback = callback;

            var module = StandardPurchasingModule.Instance();
            var builder = ConfigurationBuilder.Instance(module);
            var productCatalog = ProductCatalog.LoadDefaultCatalog();
            IAPConfigurationHelper.PopulateConfigurationBuilder(ref builder, productCatalog);
            UnityPurchasing.Initialize(this, builder);
        }

        void IStoreListener.OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            isLoading = false;
            IsInitialized = true;

            var initResult = new InitResult
            {
                isSuccess = true
            };
            initCallback?.Invoke(initResult);
            initCallback = null;
        }

        void IStoreListener.OnInitializeFailed(InitializationFailureReason error)
        {
            isLoading = false;
            IsInitialized = false;

            var initResult = new InitResult
            {
                isSuccess = false,
                error = error
            };
            initCallback?.Invoke(initResult);
            initCallback = null;
        }

        #endregion

        #region Purchase

        public void Purchase(Product product, Action<PurchaseResult> callback = null)
        {
            Purchase(product.definition.id, callback);
        }

        public void Purchase(string productId, Action<PurchaseResult> callback = null)
        {
            if (!IsInitialized)
            {
                throw new Exception("Purchasing service is not initialized!");
            }

            if (isPurchasing)
            {
                throw new Exception("Other product is purchasing!");
            }

            purchaseCallback = callback;
            isPurchasing = true;
            storeController.InitiatePurchase(productId);
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            isPurchasing = false;

            for (int i = 0, count = failListeners.Count; i < count; i++)
            {
                var observer = failListeners[i];
                observer.OnFailed(product, failureReason);
            }

            var result = new PurchaseResult
            {
                isSuccess = false,
                error = failureReason
            };

            purchaseCallback?.Invoke(result);
            purchaseCallback = null;
        }

        PurchaseProcessingResult IStoreListener.ProcessPurchase(PurchaseEventArgs args)
        {
            isPurchasing = false;

            for (int i = 0, count = completeListeners.Count; i < count; i++)
            {
                var observer = completeListeners[i];
                observer.OnComplete(args);
            }

            var result = new PurchaseResult
            {
                isSuccess = true
            };

            purchaseCallback?.Invoke(result);
            purchaseCallback = null;
            return PurchaseProcessingResult.Complete;
        }

        #endregion

        public bool TryGetProduct(string id, out Product product)
        {
            var isInitialized = IsInitialized;
            if (isInitialized)
            {
                product = storeController.products.WithID(id);
            }
            else
            {
                product = null;
            }

            return isInitialized;
        }

        public bool TryGetAllProducts(out Product[] products)
        {
            var isInitialized = IsInitialized;
            if (isInitialized)
            {
                products = storeController.products.all;
            }
            else
            {
                products = null;
            }

            return isInitialized;
        }

        public void AddCompleteListeners(IEnumerable<ICompleteListener> listeners)
        {
            completeListeners.AddRange(listeners);
        }

        public void AddCompleteListener(ICompleteListener listener)
        {
            completeListeners.Add(listener);
        }

        public void AddFailListeners(IEnumerable<IFailListener> listeners)
        {
            failListeners.AddRange(listeners);
        }

        public void AddFailListener(IFailListener listener)
        {
            failListeners.Add(listener);
        }

        public void RemoveCompleteListener(ICompleteListener listener)
        {
            completeListeners.Remove(listener);
        }

        public void RemoveFailListener(IFailListener listener)
        {
            failListeners.Remove(listener);
        }
    }
}

#endif