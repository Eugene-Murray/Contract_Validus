using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ApplicationServer.Caching;

namespace Validus.Core.AppFabric
{
    public static class CacheUtil
    {
        private static DataCacheFactory _factory = null;
        private static DataCache _cache = null;
        public static DataCache GetCache()
        {
            if (_cache != null)
                return _cache;

            _factory = new DataCacheFactory();
            //Get reference to named cache called "default"      
            _cache = _factory.GetCache("Console");
            return _cache;
        }

        public static void Flush(this DataCache cache)
        {
            foreach (var regionName in cache.GetSystemRegions())
            {
                cache.ClearRegion(regionName);
            }
        }
    } 
}
