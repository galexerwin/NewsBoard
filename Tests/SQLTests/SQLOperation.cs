using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Tests.SQLTests
{
    public enum SQLTargetTypes
    {
        Procedure,
        Table
    }

    public enum SQLOperationTypes
    {
        Select,
        Update,
        Delete
    }

    public class SQLOperationInput
    {
        public SQLTargetTypes targetType { get; set; }
        public string targetName { get; set; }
        public Dictionary<string, string>  pInputData { get; private set; }
        public SQLOperationTypes tOperationType { get; set; }
        public List<string> tInputKeys { get; private set; }
        public Dictionary<string, string> tFilterData { get; private set; }

        public bool IsMinimallySet()
        {
            // target name can't be empty
            if (!string.IsNullOrEmpty(targetName))
                return true;
            // default
            return false;
        }
        public void AddProcKVP(string key, string value)
        {
            // check if set
            if (pInputData.IsNullOrEmpty())
                pInputData = new Dictionary<string, string>();
            // check if key exists
            if (pInputData.ContainsKey(key))
                // update the value
                pInputData[key] = value;
            else
                // add the key/value
                pInputData.Add(key, value); 
        }

        public void AddProcKVP(Dictionary<string, string> input)
        {
            this.pInputData = input;
        }
        
        public void AddTableColumn(string column)
        {
            // check if set
            if (tInputKeys.IsNullOrEmpty())
                tInputKeys = new List<string>();
            // check if the column exists
            if (!tInputKeys.Contains(column))
                tInputKeys.Add(column);
        }

        public void AddTableColumn(List<string> columns)
        {
            // directly add the data
            tInputKeys = columns;
        }

        public void AddTableFilter(string key, string value)
        {
            // check if set
            if (tFilterData.IsNullOrEmpty())
                tFilterData = new Dictionary<string, string>();
            // check if key exists
            if (tFilterData.ContainsKey(key))
                // update the value
                tFilterData[key] = value;
            else
                // add the key/value
                tFilterData.Add(key, value);
        }
    }

    public class SQLOperationOutput
    {
        public int      returnCode { get; set; }
        
        public object[] returnData { get; set; }

        public bool     rcReturned { get; set; }
    }
}
