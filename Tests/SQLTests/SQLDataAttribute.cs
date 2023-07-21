/*
    Author:     Alex Erwin
    Purpose:    Retrieve the data using our custom adapter.
    Credit:     hamidmosalla.com
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Reflection;
using System.Text;
using Xunit.Sdk;

namespace Tests.SQLTests
{
    [DataDiscoverer("SQLDataAdapterAttributeDiscoverer", "Tests")]
    class SQLDataAttribute : DataAttribute
    {
        // connection string for the Azure LocalDB
        const string localDBConn = "Persist Security Info=False;Integrated Security=true;Initial Catalog=newsboardDB;server=(local)";
        // global SQL objects
        private SqlConnection conn;
        private SqlCommand command;
        // parent class parameter
        public bool EnableDiscoveryEnumeration { get; set; }
        // procedure with no parameters
        public SQLDataAttribute(string procedure) : this(procedure, null) { }
        // procedure with parameters
        public SQLDataAttribute(string procedure, Dictionary<string, string> parameters)
        {
            // create a SQL Operation
            var input = new SQLOperationInput { 
                targetName = procedure,
                targetType = SQLTargetTypes.Procedure
            };
            // push the parameters
            input.AddProcKVP(parameters);
            // route the operation
            RouteRequest(input);
        }
        // direct input method (future)
        public SQLDataAttribute(SQLOperationInput input)
        {
            // directly route the operation
            RouteRequest(input);
        }
        /// <summary>
        /// XUnit Data Collection Method
        /// Overriden to return our customized set
        /// </summary>
        public override IEnumerable<object[]> GetData(MethodInfo methodUnderTest)
        {
            // must use this as a decorator
            if (methodUnderTest == null)
                // pair this class as a decorator
                throw new ArgumentException(nameof(methodUnderTest));
            // dataset to hold return data
            DataSet         dataSet = new DataSet();
            // adapter retrieving the data
            SqlDataAdapter  adapter = new SqlDataAdapter();
            // return code storage
            int             rcValue = 0;
            bool            rcExist = false;
            // wrap in try/catch
            try
            {
                // open a connection
                conn.Open();
                // specify this is a stored procedure (change when implementing table queries)
                command.ExecuteNonQuery();
                // tell the adapter to execute 
                adapter.SelectCommand = command;
                // fill the rowset
                adapter.Fill(dataSet);
                // check if a return code exists
                if (command.Parameters.Contains("@retValue"))
                {
                    // set the value
                    rcValue = (int)command.Parameters["@retValue"].Value;
                    // set exists to true
                    rcExist = true;
                }
                // return the results from the RowCollector
                yield return RowCollector(dataSet, rcValue, rcExist);
            }
            finally
            {
                // mark the adapter as disposable
                IDisposable disposable = adapter;
                // get rid of the adapter after we are done
                if (disposable != null)
                    disposable.Dispose();
            }
        }
        /// <summary>
        /// Returns an array of SQLOperationOutput representing each row
        /// combined with a Boolean value stating if we received the return code
        /// and a value for the return code which maybe default of zero.
        /// </summary>
        /// <param name="dataSet">The returned dataset from the database</param>
        /// <param name="rcValue">Int value representing transaction state</param> 
        /// <param name="rcExist">True/False value stating if the return code was collected</param>
        /// <returns>A collection of SQLOperationOutput data members</returns> 
        private object[] RowCollector(DataSet dataSet, int rcValue, bool rcExist)
        {
            // get the row count. if null then at least one row containing the rcValue will be returned
            int      retCount = (dataSet == null) ? 1 : dataSet.Tables[0].Rows.Count;
            int      counter = 0;
            // create a result object
            object[] result = new object[retCount];
            // check if null
            if (dataSet == null)
            {
                // create a default return value
                result[0] = new SQLOperationOutput
                {
                    rcReturned = rcExist,
                    returnCode = rcValue,
                    returnData = null
                };
            }
            else
            {
                // convert rowsets
                foreach (DataRow row in dataSet.Tables[0].Rows)
                    result[counter++] = AddRowOutput(row.ItemArray, rcValue, rcExist);
            }
            // return the result
            return result;
        }
        /// <summary>
        /// Returns a class to contain the rowset and return code from the procedure
        /// if there is one.
        /// </summary>
        /// <param name="values">The row being operated on</param>
        /// <returns>SQLOperationOutput</returns> 
        private SQLOperationOutput AddRowOutput(object[] values, int rcValue, bool rcExist)
        {
            // create the object to contain the data rows
            object[] result = new object[values.Length];
            // iterate over the columns and check the values for nulls
            for (int idx = 0; idx < values.Length; idx++)
                result[idx] = ConvertParameter(values[idx]);
            // return the row data
            return new SQLOperationOutput 
            { 
                rcReturned = rcExist,
                returnCode = rcValue,
                returnData = result
            };
        }
        /// <summary>
        /// Converts a parameter to its destination parameter type, if necessary.
        /// </summary>
        /// <param name="parameter">The parameter value</param>
        /// <returns>The converted parameter value</returns>
        private object ConvertParameter(object parameter)
        {
            // cast to C# null type
            if (parameter is DBNull)
                return null;
            // return unaltered
            return parameter;
        }
        // defaults to procedure type
        // where filter implementation needs to include logical symbols
        private void RouteRequest(SQLOperationInput input)
        {
            // create a new sql server connection
            conn = new SqlConnection(localDBConn);
            //
            // sql string
            //string sqlString = "";
            //string whereClause = "";
            // check input as minimally filled
            if (input.IsMinimallySet() == false)
                throw new Exception("Target Name Can't Be Empty.");
            // figure out what type of target this is
            switch (input.targetType)
            {
                case SQLTargetTypes.Procedure:
                    // prepare the sql command
                    PrepareProcedure(input);
                    break;
                case SQLTargetTypes.Table:
                    // check if there's a where clause
                    //if (!input.tFilterData.IsNullOrEmpty())
                    //    whereClause = "WHERE " + ParseParameters(input.tFilterData);
                    // determine what operation to perform on the table
                    switch (input.tOperationType)
                    {
                        case SQLOperationTypes.Select:
                            break;
                        case SQLOperationTypes.Update:
                            break;
                        case SQLOperationTypes.Delete:
                            break;
                    }
                    break;
            }
        }
       
        private void PrepareProcedure(SQLOperationInput input)
        {
            // parameter binding variable
            SqlParameter param = new SqlParameter();
            // create a command to the db
            command = new SqlCommand(input.targetName, conn);
            // mark as a stored procedure
            command.CommandType = CommandType.StoredProcedure;
            // need to implement datatyping
            if (!input.pInputData.IsNullOrEmpty())
            {
                // iterate over the collection
                foreach (var kvp in input.pInputData)
                {
                    // add the name and value
                    param = command.Parameters.AddWithValue("@" + kvp.Key, kvp.Value);
                    //
                }
            }
            // add the return code on out
            command.Parameters.Add("@retValue", SqlDbType.Int).Direction = ParameterDirection.ReturnValue;
        }
    }
}
