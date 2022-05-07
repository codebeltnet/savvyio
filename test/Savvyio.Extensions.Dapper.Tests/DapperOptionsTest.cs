using System;
using System.Data;
using System.Threading;
using Cuemon.Extensions;
using Cuemon.Extensions.Xunit;
using Dapper;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Dapper
{
    public class DapperOptionsTest : Test
    {
        public DapperOptionsTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void DapperOptions_Ensure_Initialization_Defaults()
        {
            var sut = new DapperOptions();

            var cd = (CommandDefinition)sut;

            Assert.Null(sut.CommandText);
            Assert.Equal(CommandFlags.Buffered, sut.CommandFlags);
            Assert.Equal(TimeSpan.FromSeconds(30), sut.CommandTimeout);
            Assert.Equal(CommandType.Text, sut.CommandType);
            Assert.Null(sut.Parameters);
            Assert.Null(sut.Transaction);
            Assert.Equal(default(CancellationToken), sut.CancellationToken);
            Assert.Equal(sut.CommandText, cd.CommandText);
            Assert.Equal(sut.CommandFlags, cd.Flags);
            Assert.Equal(sut.CommandTimeout.TotalSeconds.As<int>(), cd.CommandTimeout);
            Assert.Equal(sut.CommandType, cd.CommandType);
            Assert.Equal(sut.Parameters, cd.Parameters);
            Assert.Equal(sut.Transaction, cd.Transaction);
            
        }
    }
}
