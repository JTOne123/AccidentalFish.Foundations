﻿using System;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.Foundations.Resources.Azure.TableStorage
{
    public interface ITableStorageConcurrencyManager
    {
        Task<T> Update<T>(IAsyncTableStorageRepository<T> table, string paritionKey, Action<T> update) where T : ITableEntity, new();

        Task<T> Update<T>(IAsyncTableStorageRepository<T> table, string paritionKey, string rowKey, Action<T> update) where T : ITableEntity, new();

        Task<T> InsertOrUpdate<T>(IAsyncTableStorageRepository<T> table, string partitionKey, Action<T> update, Func<T> insert) where T : ITableEntity, new();

        Task<T> InsertOrUpdate<T>(IAsyncTableStorageRepository<T> table, string partitionKey, string rowKey, Action<T> update, Func<T> insert) where T : ITableEntity, new();
    }
}
