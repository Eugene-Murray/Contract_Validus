using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Core.Tests.Data.Model
{
    [Version]
    public class Office
    {

        public int Id { get; set; }

        public string Name { get; set; }

        [LatestVersionAttribute]
        public Address Address { get; set; }
    }
}
