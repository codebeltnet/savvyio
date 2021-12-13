using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Savvyio.Queries
{
    //public abstract class Query : Model, IQuery
    //{
    //    protected Query(IMetadata metadata = null)
    //    {
    //        this.MergeMetadata(metadata);
    //    }
    //}

    public abstract class Query<TResult> : Model, IQuery<TResult>
    {
        protected Query(IMetadata metadata = null)
        {
            this.MergeMetadata(metadata);
        }
    }
}
