/*
    Author:     Alex Erwin
    Purpose:    Unit Tests are to be put here
    Credit:     hamidmosalla.com
*/
using System;
using Xunit;

namespace Tests.SQLTests
{
    public class SQLUnitTests
    {


        [Theory]
        [SQLData("sp_member_add")]
        public void MemberAdd(SQLOperationOutput output)
        {
            Assert.Equal(1, output.returnCode);
        }
    }
}
