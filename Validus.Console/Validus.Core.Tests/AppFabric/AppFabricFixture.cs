using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console;
using Validus.Console.DTO;
using Validus.Core.AppFabric;

namespace Validus.Core.Tests.AppFabric
{
    /// <summary>
    /// To allow user to access the cache...
    /// Grant-CacheAllowedClientAccount -Account "DOMAINNAME\username"
    /// </summary>
    [TestClass]
    [Ignore]
    public class AppFabricFixture
    {
        [ClassCleanup]
        public static void ClassCleanup()
        {
            CacheUtil.GetCache("Console").Flush();
        }

        [TestMethod]
        public void Put_Get_CacheItems_Success()
        {
            // Assign
            var cachePolicies = new List<RenewalPolicyDetailed>();
            var policies = GetRenewalPolicyDetailPolicies();

            for (int i = 0; i < 50; i++)
            {
                cachePolicies.AddRange(policies);
            }

            // Act
            CacheUtil.GetCache("Console")
                         .Put("RenewalData", cachePolicies, DateTime.Today.AddDays(1).Date - DateTime.Now);

            var actualReturnedCacheValues = CacheUtil.GetCache("Console")
                                    .Get("RenewalData") as List<RenewalPolicyDetailed>;

            // Assert
            Assert.IsTrue(actualReturnedCacheValues != null);
            Assert.IsTrue(actualReturnedCacheValues.Count > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(Microsoft.ApplicationServer.Caching.DataCacheException))]
        public void Put_Get_CacheItems_Exception()
        {
            // Assign
            var cachePolicies = new List<RenewalPolicyDetailed>();
            var policies = GetRenewalPolicyDetailPolicies();

            for (int i = 0; i < 200; i++)
            {
                cachePolicies.AddRange(policies);
            }

            // Act
            CacheUtil.GetCache("Console")
                         .Put("RenewalData", cachePolicies, DateTime.Today.AddDays(1).Date - DateTime.Now);

            var actualReturnedCacheValues = CacheUtil.GetCache("Console")
                                    .Get("RenewalData") as List<RenewalPolicyDetailed>;

            // Assert
        }

        private static List<RenewalPolicyDetailed> GetRenewalPolicyDetailPolicies()
        {
            var policies = new List<RenewalPolicyDetailed>();

            using (var conn = new SqlConnection("Data Source=ukroomdev01;Initial Catalog=Subscribe;Integrated Security=True"))
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
                    cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = "";
                        //_currentHttpContext.CurrentUser.Identity.Name;
                    cmd.Parameters.Add("@FrDt", SqlDbType.DateTime).Value = DateTime.Today.AddDays(-10);
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

            return policies;
        }

        [TestMethod]
        public void GetItemsFromCache_Success()
        {
            // Assign

            // Act

            // Assert
        }

        //public List<RenewalPolicyDetailed> GetRenewalPolicies(bool bypassCache)
        //{
        //    //var loadPolicies = new List<RenewalPolicyDetailed>();

        //    //var policies = CacheUtil.GetCache(Constants.ConsoleCacheKey)
        //    //                        .Get(Constants.RenewalCacheKey) as List<RenewalPolicyDetailed>;

        //    //if (bypassCache || policies == null)
        //    //{
        //        var policies = new List<RenewalPolicyDetailed>();

        //        using (var conn = new SqlConnection("Data Source=ukroomdev01;Initial Catalog=Subscribe;Integrated Security=True"))
        //        {
        //            using (var cmd = new SqlCommand("rpt_RnwlMonitor", conn))
        //            {
        //                cmd.CommandTimeout = 240;
        //                cmd.CommandType = CommandType.StoredProcedure;

        //                cmd.Parameters.Add("@ORIGOFF", SqlDbType.VarChar, 10).Value = "ALL";
        //                cmd.Parameters.Add("@COB", SqlDbType.VarChar, 10).Value = "ALL";
        //                cmd.Parameters.Add("@IsBound", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@ExclDECLAR", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@ExclNR", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@ExclRenBOUND", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@ExclRenNTU", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@ExclRenQuote", SqlDbType.Bit).Value = 1;
        //                cmd.Parameters.Add("@Bkr", SqlDbType.VarChar, 4).Value = "";
        //                cmd.Parameters.Add("@UserID", SqlDbType.VarChar, 255).Value = "";  //_currentHttpContext.CurrentUser.Identity.Name;
        //                cmd.Parameters.Add("@FrDt", SqlDbType.DateTime).Value = DateTime.Today.AddDays(-10);
        //                cmd.Parameters.Add("@ToDt", SqlDbType.DateTime).Value = DateTime.Today.AddDays(100);

        //                conn.Open();

        //                using (var r = cmd.ExecuteReader())
        //                {
        //                    while (r.Read())
        //                    {
        //                        var detail = new RenewalPolicyDetailed();

        //                        Utility.SetObjectPropertyValue(detail, "COB", r, "COB");
        //                        Utility.SetObjectPropertyValue(detail, "OriginatingOffice", r, "ORIGOFF");
        //                        Utility.SetObjectPropertyValue(detail, "Underwriter", r, "UWR");
        //                        Utility.SetObjectPropertyValue(detail, "Leader", r, "Leader");
        //                        Utility.SetObjectPropertyValue(detail, "Broker", r, "Broker");
        //                        Utility.SetObjectPropertyValue(detail, "BrokerContact", r, "Contact");
        //                        Utility.SetObjectPropertyValue(detail, "PolicyId", r, "Expiring Ref");
        //                        Utility.SetObjectPropertyValue(detail, "UMR", r, "UMR");
        //                        Utility.SetObjectPropertyValue(detail, "InsuredName", r, "InsdNm");
        //                        Utility.SetObjectPropertyValue(detail, "Description", r, "Description");
        //                        Utility.SetObjectPropertyValue(detail, "InceptionDate", r, "Inception");
        //                        Utility.SetObjectPropertyValue(detail, "ExpiryDate", r, "Expiry");
        //                        Utility.SetObjectPropertyValue(detail, "St", r, "St");
        //                        Utility.SetObjectPropertyValue(detail, "Line", r, "Line");
        //                        Utility.SetObjectPropertyValue(detail, "Currency", r, "Ccy");
        //                        Utility.SetObjectPropertyValue(detail, "MarketGrossPremium", r, "MktGrPm");
        //                        Utility.SetObjectPropertyValue(detail, "SyndicateGrossPremium", r, "SynGrPm");
        //                        Utility.SetObjectPropertyValue(detail, "SyndicateNetPremium", r, "SynNetPm");
        //                        Utility.SetObjectPropertyValue(detail, "SignedPremium", r, "Signed Pm");
        //                        Utility.SetObjectPropertyValue(detail, "PercentageOfEPI", r, "% of EPI");
        //                        Utility.SetObjectPropertyValue(detail, "HasClaims", r, "Clm");
        //                        Utility.SetObjectPropertyValue(detail, "RenewalPosition", r, "Non-renewable");
        //                        Utility.SetObjectPropertyValue(detail, "RenewalNotes", r, "Renewal Note");
        //                        Utility.SetObjectPropertyValue(detail, "ToBroker", r, "ToBroker");
        //                        Utility.SetObjectPropertyValue(detail, "ToBrokerContact", r, "ToContact");
        //                        Utility.SetObjectPropertyValue(detail, "ToPolicyId", r, "ToPolId");
        //                        Utility.SetObjectPropertyValue(detail, "ToStatus", r, "ToSt");
        //                        Utility.SetObjectPropertyValue(detail, "ToSubmissionStatus", r, "ToSubmSt");
        //                        Utility.SetObjectPropertyValue(detail, "ToEntryStatus", r, "ToEntSt");

        //                        policies.Add(detail);
        //                    }
        //                }
        //            }
        //        }

        //        var myPolicies = new List<RenewalPolicyDetailed>();
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);
        //        myPolicies.AddRange(policies);

        //        CacheUtil.GetCache(Constants.ConsoleCacheKey)
        //                 .Put(Constants.RenewalCacheKey, myPolicies, DateTime.Today.AddDays(1).Date - DateTime.Now);


        //        loadPolicies = CacheUtil.GetCache(Constants.ConsoleCacheKey)
        //                            .Get(Constants.RenewalCacheKey) as List<RenewalPolicyDetailed>;
        //    }
        //    else
        //    {
        //        loadPolicies.AddRange(policies);
        //    }

        //    return loadPolicies;
        //}
    }
}
