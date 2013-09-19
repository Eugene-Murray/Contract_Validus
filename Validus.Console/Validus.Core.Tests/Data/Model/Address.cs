using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Validus.Core.Data.Interceptor.Interceptors;

namespace Validus.Core.Tests.Data.Model
{
    public class Address : ISoftDelete, IVersion
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public virtual ICollection<Person> Persons { get; set; }
        public bool IsDeleted { get; set; }
        public int VersionNo {get;set;}
        public Guid Key { get; set; }
        public bool IsObsolete { get; set; }
    }
}
