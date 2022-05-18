using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cuemon;

namespace Savvyio
{
    public static class ValidatorExtensions
    {
        public static T InvalidState<T>(this Validator _, T value, Action<T> validator)
        {
            validator(value);
            return value;
        }
    }
}
