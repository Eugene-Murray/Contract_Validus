using Microsoft.ApplicationServer.Caching;

namespace Validus.Core.AppFabric
{
    public static class CacheUtil
	{
		private const string DefaultCache = "default";
		private const string DefaultItem = "Default";
        private static readonly DataCacheFactory factory = new DataCacheFactory();

		public static DataCache GetCache(string cacheName = CacheUtil.DefaultCache)
		{
			return factory.GetCache(cacheName);			
		}

	    public static DataCacheItem GetCacheItem(string cacheName = CacheUtil.DefaultCache,
	                                             string itemName = CacheUtil.DefaultItem)
	    {
		    var cache = CacheUtil.GetCache(cacheName);

		    return cache != null
			           ? cache.GetCacheItem(itemName)
			           : null;
	    }

	    public static void Flush(this DataCache cache)
        {
            foreach (var region in cache.GetSystemRegions())
            {
                cache.ClearRegion(region);
            }
        }
    }
}