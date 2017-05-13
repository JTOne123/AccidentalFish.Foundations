﻿using System;
using AccidentalFish.Foundations.Resources.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.Foundations.Resources.Azure.TableStorage.Implementation
{
    internal class TableStorageRepositoryFactory : ITableStorageRepositoryFactory
    {
        private readonly ITableStorageQueryBuilder _tableStorageQueryBuilder;
        private readonly ITableContinuationTokenSerializer _tableContinuationTokenSerializer;
        private readonly IConnectionStringProvider _connectionStringProvider;
        private readonly ILoggerFactory _loggerFactory;

        public TableStorageRepositoryFactory(
            ITableStorageQueryBuilder tableStorageQueryBuilder,
            ITableContinuationTokenSerializer tableContinuationTokenSerializer,
            IConnectionStringProvider connectionStringProvider,
            ILoggerFactory loggerFactory)
        {
            _tableStorageQueryBuilder = tableStorageQueryBuilder;
            _tableContinuationTokenSerializer = tableContinuationTokenSerializer;
            _connectionStringProvider = connectionStringProvider;
            _loggerFactory = loggerFactory;            
        }

        public IAsyncTableStorageRepository<T> Create<T>(string tableName) where T : ITableEntity, new()
        {
            if (String.IsNullOrWhiteSpace(tableName)) throw new ArgumentNullException(nameof(tableName));
            string connectionString = _connectionStringProvider.Get<IAsyncTableStorageRepository<T>>(tableName);
            return new AsyncTableStorageRepository<T>(connectionString, tableName, _tableStorageQueryBuilder, _tableContinuationTokenSerializer, _loggerFactory.CreateLogger<AsyncTableStorageRepository<T>>());
        }        
    }
}
