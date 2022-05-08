using System;
using System.Data;
using Dapper;

namespace Savvyio.Assets
{
    public class GuidHandler : SqlMapper.TypeHandler<Guid>
    {
        public override void SetValue(IDbDataParameter parameter, Guid value) => parameter.Value = value;

        public override Guid Parse(object value) => Guid.Parse((string)value);
    }
}
