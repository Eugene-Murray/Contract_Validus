using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Core.Tests.Data.Model
{
    public class Person:ISoftDelete
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Address Address { get; set; }
        public bool IsDeleted { get; set; }
    }
}
