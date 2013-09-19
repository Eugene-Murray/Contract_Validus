using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Microsoft.ApplicationServer.Caching;
using Validus.Console.App_Start;
using Validus.Console.DTO;
using Validus.Core.AppFabric;
using Validus.Core.HttpContext;
using Validus.Core.LogHandling;
using Validus.Models;

namespace Validus.Console.Data
{
    public class PolicyData : IPolicyData
    {
        public const String RenewalCacheKey = "RenewalData";

        private readonly ILogHandler _logHandler;
        private ICurrentHttpContext _currentHttpContext;
        private readonly IConsoleRepository _repository;

        public PolicyData(IConsoleRepository repository, ILogHandler logHandler, ICurrentHttpContext currentHttpContext)
        {
			this._logHandler = logHandler;
			this._currentHttpContext = currentHttpContext;
			this._repository = repository;
        }

        public Broker GetBroker(int brokerSequenceId)
        {
            return this._repository.Query<Broker>().FirstOrDefault(b => b.BrokerSequenceId == brokerSequenceId);
        }

        public List<RenewalPolicyDetailed> GetRenewalPolicies(bool bypassCache)
        {
            var policies = CacheUtil.GetCache().Get(RenewalCacheKey) as List<RenewalPolicyDetailed>;

            if (bypassCache || (policies == null)) 
            {
                policies = new List<RenewalPolicyDetailed>();

                using (var conn = new SqlConnection(Properties.Settings.Default.SubscribeSQL))
                {
	                using (var cmd = new SqlCommand("rpt_RnwlMonitor", conn))
	                {
		                cmd.CommandTimeout = 240;
		                cmd.CommandType = CommandType.StoredProcedure;

		                cmd.Parameters.Add("@ORIGOFF", SqlDbType.VarChar, 10).Value = "ALL";
		                cmd.Parameters.Add("@COB", SqlDbType.VarChar, 10).Value = "ALL";
		                cmd.Parameters.Add("@IsBound", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@ExclDECLAR", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@ExclNR", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@ExclRenBOUND", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@ExclRenNTU", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@ExclRenQuote", SqlDbType.Bit).Value = 1;
		                cmd.Parameters.Add("@Bkr", SqlDbType.VarChar, 4).Value = "";
                        cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = _currentHttpContext.CurrentUser.Identity.Name;
		                cmd.Parameters.Add("@FrDt", SqlDbType.DateTime).Value = DateTime.Today.AddDays(-100);
		                cmd.Parameters.Add("@ToDt", SqlDbType.DateTime).Value = DateTime.Today.AddDays(100);

		                conn.Open();

		                using (var r = cmd.ExecuteReader())
		                {
			                while (r.Read())
			                {
				                var detail = new RenewalPolicyDetailed();

				                Utility.SetObjectPropertyValue(detail, "COB", r, "COB");
				                Utility.SetObjectPropertyValue(detail, "OriginatingOffice", r, "ORIGOFF");
				                Utility.SetObjectPropertyValue(detail, "Underwriter", r, "UWR");
				                Utility.SetObjectPropertyValue(detail, "Leader", r, "Leader");
				                Utility.SetObjectPropertyValue(detail, "Broker", r, "Broker");
				                Utility.SetObjectPropertyValue(detail, "BrokerContact", r, "Contact");
				                Utility.SetObjectPropertyValue(detail, "PolicyId", r, "Expiring Ref");
				                Utility.SetObjectPropertyValue(detail, "UMR", r, "UMR");
				                Utility.SetObjectPropertyValue(detail, "InsuredName", r, "InsdNm");
				                Utility.SetObjectPropertyValue(detail, "Description", r, "Description");
				                Utility.SetObjectPropertyValue(detail, "InceptionDate", r, "Inception");
				                Utility.SetObjectPropertyValue(detail, "ExpiryDate", r, "Expiry");
				                Utility.SetObjectPropertyValue(detail, "St", r, "St");
				                Utility.SetObjectPropertyValue(detail, "Line", r, "Line");
				                Utility.SetObjectPropertyValue(detail, "Currency", r, "Ccy");
				                Utility.SetObjectPropertyValue(detail, "MarketGrossPremium", r, "MktGrPm");
				                Utility.SetObjectPropertyValue(detail, "SyndicateGrossPremium", r, "SynGrPm");
				                Utility.SetObjectPropertyValue(detail, "SyndicateNetPremium", r, "SynNetPm");
				                Utility.SetObjectPropertyValue(detail, "SignedPremium", r, "Signed Pm");
				                Utility.SetObjectPropertyValue(detail, "PercentageOfEPI", r, "% of EPI");
				                Utility.SetObjectPropertyValue(detail, "HasClaims", r, "Clm");
				                Utility.SetObjectPropertyValue(detail, "RenewalPosition", r, "Non-renewable");
				                Utility.SetObjectPropertyValue(detail, "RenewalNotes", r, "Renewal Note");
				                Utility.SetObjectPropertyValue(detail, "ToBroker", r, "ToBroker");
				                Utility.SetObjectPropertyValue(detail, "ToBrokerContact", r, "ToContact");
				                Utility.SetObjectPropertyValue(detail, "ToPolicyId", r, "ToPolId");
				                Utility.SetObjectPropertyValue(detail, "ToStatus", r, "ToSt");
				                Utility.SetObjectPropertyValue(detail, "ToSubmissionStatus", r, "ToSubmSt");
				                Utility.SetObjectPropertyValue(detail, "ToEntryStatus", r, "ToEntSt");

				                policies.Add(detail);
			                }
		                }
	                }
                }

                CacheUtil.GetCache().Put(RenewalCacheKey, policies, DateTime.Today.AddDays(1).Date - DateTime.Now);
            }

            return policies;
        }

        public void RemovePolicyFromCache(string renewalPolicyId)
        {
            var updateCache = new Action(() =>
                {
                    if(string.IsNullOrEmpty(renewalPolicyId)) return;
                    var cacheClient = CacheUtil.GetCache();
                    var cacheItem = cacheClient.GetCacheItem(RenewalCacheKey);
                    var policies = cacheItem.Value as List<RenewalPolicyDetailed>;
                    if (policies == null) return;
                    // ReSharper disable SimplifyLinqExpression
                    if (!policies.Any(p => p.PolicyId == renewalPolicyId)) return;
                    // ReSharper restore SimplifyLinqExpression
                    policies.Remove(policies.First(p => p.PolicyId == renewalPolicyId));
                    cacheClient.Put(RenewalCacheKey, policies, cacheItem.Version, DateTime.Today.AddDays(1).Date - DateTime.Now);
                });

            var retry = true;
            do
            {
                try
                {
                    updateCache();
                    retry = false;
                }
                catch (DataCacheException dataCacheException)
                {
                   if (dataCacheException.ErrorCode == DataCacheErrorCode.CacheItemVersionMismatch)
                   {
                       retry = true;
                       System.Threading.Thread.Sleep(100);
                   }
                }
            } while (retry);
         
        }
        
        //public RenewalPolicyDetailed GetRenewalPolicyDetailsByPolId(string polId)
        //{
        //    List<RenewalPolicyDetailed> policies = HttpContext.Current.Cache[RenewalCacheKey] as List<RenewalPolicyDetailed>;

        //    if (policies == null)
        //    {
        //        GetRenewalPolicies(false);
        //        policies = HttpContext.Current.Cache[RenewalCacheKey] as List<RenewalPolicyDetailed>;
        //    }
        //    if (policies != null)
        //    {
        //        try
        //        {
        //            return policies.Single(p => p.PolicyId == polId);
        //        }
        //        catch
        //        {
        //            throw new Exception("Unable to retrieve the Renewal Policy Details.");
        //        }
        //    }
        //    else
        //        throw new Exception("Unable to retrieve the Renewal Policy Details.");
        //}
    }
}