﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.Foundations.Resources.Azure.TableStorage.Implementation
{
    internal class TableStorageQueryBuilder : ITableStorageQueryBuilder
    {
        public TableQuery<T> TableQuery<T>(Dictionary<string, object> columnValues, TableStorageQueryOperator op) where T : ITableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>();
            if (columnValues != null && columnValues.Any())
            {
                List<string> tableQueries = new List<string>();
                foreach (KeyValuePair<string, object> kvp in columnValues)
                {
                    if (kvp.Value is string)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition(kvp.Key, QueryComparisons.Equal, (string)kvp.Value));
                    }
                    else if (kvp.Value is Guid)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForGuid(kvp.Key, QueryComparisons.Equal, (Guid)kvp.Value));
                    }
                    else if (kvp.Value is bool)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForBool(kvp.Key, QueryComparisons.Equal, (bool)kvp.Value));
                    }
                    else if (kvp.Value is DateTime)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForDate(kvp.Key, QueryComparisons.Equal, (DateTime)kvp.Value));
                    }
                    else if (kvp.Value is int)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForInt(kvp.Key, QueryComparisons.Equal, (int)kvp.Value));
                    }
                    else if (kvp.Value is long)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForLong(kvp.Key, QueryComparisons.Equal, (long)kvp.Value));
                    }
                    else if (kvp.Value is double)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForDouble(kvp.Key, QueryComparisons.Equal, (double)kvp.Value));
                    }
                    else
                    {
                        Type dataType = kvp.Value.GetType();
                        throw new InvalidOperationException(String.Format("Type {0} is not supported in table query operations", dataType.Name));
                    }
                }
                string queryString = tableQueries[0];
                string tableOp = op == TableStorageQueryOperator.And ? TableOperators.And : TableOperators.Or;
                for (int index = 1; index < tableQueries.Count; index++)
                {
                    string subQueryString = tableQueries[index];
                    queryString = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(queryString, tableOp, subQueryString);
                }
                query = query.Where(queryString);
            }
            
            return query;
        }

        public TableQuery<T> TableQuery<T>(string column, IEnumerable<object> values, TableStorageQueryOperator op) where T : ITableEntity, new()
        {
            TableQuery<T> query = new TableQuery<T>();
            if (!string.IsNullOrWhiteSpace(column) && values.Any())
            {
                List<string> tableQueries = new List<string>();
                foreach (object value in values)
                {
                    string s = value as string;
                    if (s != null)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, s));
                    }
                    else if (value is Guid)
                    {
                        tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForGuid(column, QueryComparisons.Equal, (Guid)value));
                    }
                }
                string queryString = tableQueries[0];
                string tableOp = op == TableStorageQueryOperator.And ? TableOperators.And : TableOperators.Or;
                for (int index = 1; index < tableQueries.Count; index++)
                {
                    string subQueryString = tableQueries[index];
                    queryString = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(queryString, tableOp, subQueryString);
                }
                query = query.Where(queryString);
            }

            return query;
        }
    }
}
