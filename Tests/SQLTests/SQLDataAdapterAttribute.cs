/*
    Author:     Alex Erwin
    Purpose:    Retrieve the data using our custom adapter.
    Credit:     hamidmosalla.com
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using Xunit.Sdk;

namespace Tests.SQLTests
{
    [DataDiscoverer("SQLDataAdapterAttributeDiscoverer", "Tests")]
    public abstract class SQLDataAdapterAttribute : DataAttribute
    {
        protected abstract IDataAdapter DataAdapter { get; }

        public bool EnableDiscoveryEnumeration { get; set; }

        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            DataSet dataSet = new DataSet();
            IDataAdapter adapter = DataAdapter;

            try
            {
                adapter.Fill(dataSet);

                foreach (DataRow row in dataSet.Tables[0].Rows)
                    yield return ConvertParameters(row.ItemArray);
            }
            finally
            {
                IDisposable disposable = adapter as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }

        object[] ConvertParameters(object[] values)
        {
            object[] result = new object[values.Length];

            for (int idx = 0; idx < values.Length; idx++)
                result[idx] = ConvertParameter(values[idx]);

            return result;
        }
        protected virtual object ConvertParameter(object parameter)
        {
            if (parameter is DBNull)
                return null;

            return parameter;
        }
    }
}
