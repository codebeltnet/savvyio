﻿using System;
using Cuemon.Extensions.IO;
using Codebelt.Extensions.Xunit;
using Savvyio.Assets.Commands;
using Xunit;
using Xunit.Abstractions;

namespace Savvyio.Extensions.Text.Json.Commands
{
    public class CommandTest : Test
    {
        public CommandTest(ITestOutputHelper output) : base(output)
        {
        }

        [Fact]
        public void CreateAccount_ShouldUseCustomConverter()
        {
            var ca = new CreateAccount(Guid.NewGuid(), "Michael Mortensen", "root@gimlichael.dev");

            var json = new JsonMarshaller().Serialize(ca);
            var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

            TestOutput.WriteLine(jsonString);

            var rehydrated = new JsonMarshaller().Deserialize<CreateAccount>(json);

            Assert.Equivalent(ca, rehydrated, true);
        }

        [Fact]
        public void CreateAccountWithValueObjects_ShouldUseCustomConverter()
        {
	        var ca = new CreateAccountWithValueObjects(Guid.NewGuid(), "Michael Mortensen", "root@gimlichael.dev");

	        var json = new JsonMarshaller().Serialize(ca);
	        var jsonString = json.ToEncodedString(o => o.LeaveOpen = true);

	        TestOutput.WriteLine(jsonString);

	        var rehydrated = new JsonMarshaller().Deserialize<CreateAccountWithValueObjects>(json);

	        Assert.Equivalent(ca, rehydrated, true);
        }
    }
}
