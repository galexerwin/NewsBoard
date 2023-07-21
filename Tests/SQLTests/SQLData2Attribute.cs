/*
    Author:     Alex Erwin
    Purpose:    Decorator class for passing the data in
    Credit:     hamidmosalla.com
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;

namespace Tests.SQLTests
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SQLData2Attribute : SQLDataAdapterAttribute
    {
        const string connectionStringFormat = "Provider=SQLOLEDB; Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};";

        readonly string sqlConnString;
        readonly string sqlStmtString;

        public SQLData2Attribute(string server, string database, string user, string pass, string sql)
        {
            this.sqlConnString = String.Format(CultureInfo.InvariantCulture, connectionStringFormat, server, database, user, pass);
            this.sqlStmtString = sql;
        }

        public OleDbDataAdapter Execute(string procedure, Dictionary<string, string> parameters)
        {
            // execute string
            string sqlString = "";
            // check procedure
            if (String.IsNullOrEmpty(procedure))
                throw new Exception("Procedure Name Can't Be Empty.");
            // build up string
            sqlString = "Execute " + procedure + " " + ParseParameters(parameters) + ";";



            return new OleDbDataAdapter(sqlString, sqlConnString);
        }

        private string ParseParameters(Dictionary<string, string> parameters)
        {
            // string to our
            string strOut = "";
            // wrap in try
            try
            {
                // check if there are parameters
                if (parameters.Count > 0)
                {
                    // iterate over parameters
                    foreach (var kvp in parameters)
                        strOut += "@" + kvp.Key + "='" + kvp.Value.Replace("'", "''") + "', ";
                    // trim final comma and white space
                    strOut.Substring(0, strOut.Length - 2);
                }
            }
            catch (Exception ex)
            {
                // write error out to log
                Console.WriteLine("Error Occurred: " + ex.Message);
            }
            // default to return empty string
            return strOut;
        }

        public string SQLStatement
        {
            get { return sqlStmtString; }
        }
        protected override IDataAdapter DataAdapter
        {
            get { return new OleDbDataAdapter(sqlStmtString, sqlConnString); }
        }
    }
}
