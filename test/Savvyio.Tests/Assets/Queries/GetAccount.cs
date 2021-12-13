using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Savvyio.Queries;

namespace Savvyio.Assets.Queries
{
    public class GetAccount : Query<string>
    {
        public GetAccount(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
