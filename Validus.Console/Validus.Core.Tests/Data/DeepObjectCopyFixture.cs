using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Validus.Console.DTO;
using Validus.Models;

namespace Validus.Core.Tests.Data
{
    [TestClass]
    public class DeepObjectCopyFixture
    {
        private static SubmissionDto _energySubmissionDto;

		//[ClassInitialize]
		//public static void Init(TestContext context)
		//{
		//	_energySubmissionDto = new SubmissionDto
		//	{
		//		InsuredName = "- N/A",
		//		BrokerCode = "1111",
		//		BrokerPseudonym = "AAA",
		//		BrokerSequenceId = 822,
		//		InsuredId = 182396,
		//		Brokerage = 1,
		//		BrokerContact = "ALLAN MURRAY",
		//		Description = "Test Submission",
		//		UnderwriterCode = "AED",
		//		UnderwriterContactCode = "JAC",
		//		QuotingOfficeId = "LON",
		//		Leader = "AG",
		//		Domicile = "AD",
		//		Title = "Seed Submission",
		//		SubmissionTypeId = "EN",
		//		ExtraProperty1 = "Test Val 1",
		//		ExtraProperty2 = "Test Val 2",
		//		Options = new List<OptionDto>{
		//				new OptionDto { 
		//					Id = 1, 
		//					Title = "Seed Submission",
		//					OptionVersions = new List<OptionVersionDto>{
		//						new OptionVersionDto { 
		//							OptionId = 0, 
		//							VersionNumber = 0, 
		//							Comments = "OptionVersion Comments", 
		//							Title = "Unit Test Submission", 
		//							Quotes = new List<QuoteDto>
		//								{
		//									new QuoteENDto
		//									{ 
		//										COBId = "AD", 
		//										MOA = "FA", 
		//										InceptionDate = DateTime.Now, 
		//										ExpiryDate = DateTime.Now.AddMonths(12), 
		//										QuoteExpiryDate = DateTime.Now, 
		//										AccountYear = 2013, 
		//										Currency = "USD", 
		//										LimitCCY = "USD", 
		//										ExcessCCY = "USD", 
		//										CorrelationToken = Guid.NewGuid(), 
		//										IsSubscribeMaster = true, 
		//										PolicyType = "NONMARINE", 
		//										EntryStatus = "PARTIAL", 
		//										SubmissionStatus = "SUBMITTED", 
		//										TechnicalPricingBindStatus = "PRE", 
		//										TechnicalPricingPremiumPctgAmt = "AMT", 
		//										TechnicalPricingMethod = "UW" ,
		//										OriginatingOfficeId = "LON",
		//										//QuoteExtraProperty1 = "gfdgdfg",
		//									}
		//								}
		//						}}
		//				}}
		//	};
		//}


		//[TestMethod]
		//public void AutoMapper_MapDtoToEntity()
		//{
		//	// NOTE: does not automatically map to QuoteEN
            
		//	// Assign 
		//	var expectedQuoteType = "Quote";
            
		//	// Act
		//	Mapper.CreateMap<SubmissionDto, Validus.Models.SubmissionEN>()
		//		  .ForMember(x => x.Options, y => y.Ignore());

		//	var energySubmission = Mapper.Map<Validus.Models
		//		.SubmissionEN>(_energySubmissionDto);

		//	Mapper.CreateMap<QuoteDto, Validus.Models.Quote>()
		//		.Include<QuoteENDto, Validus.Models.QuoteEN>();
		//	Mapper.CreateMap<OptionVersionDto, Validus.Models.OptionVersion>();
		//	Mapper.CreateMap<OptionDto, Validus.Models.Option>();

		//	var optionList = Mapper.Map<List<Validus.Models
		//		.Option>>(_energySubmissionDto.Options);

		//	energySubmission.Options = optionList;

		//	// Assert
		//	Assert.AreEqual(_energySubmissionDto.Title, energySubmission.Title);
		//	Assert.AreEqual(expectedQuoteType,
		//		energySubmission.Options.FirstOrDefault().OptionVersions.FirstOrDefault().Quotes.FirstOrDefault().GetType().Name);
		//}

        [Ignore]
        [TestMethod]
        public void Reflection_DeepClone()
        {
            // Assert

            // Act
            //try
            //{
            //    var energySubmission = new SubmissionEN();
            //    energySubmission.Map(_energySubmissionDto);
            //}
            //catch (Exception ex)
            //{

            //    var err = ex.ToString();
            //}

            // Assert
        }


    }

    public static class SubmissionHelper
    {
        public static void Map<T>(Quote quote, T dto)
        {
            SetProperties(quote, dto);
        }
        
        public static void Map<T>(OptionVersion optionVersion, T dto)
        {
            SetProperties(optionVersion, dto);
        }
        
        public static void Map<T>(Option option, T dto)
        {
            SetProperties(option, dto);
        }


        public static void Map<T>(this Submission submission, T dto)
        {
            SetProperties(submission, dto);
        }

        private static void SetProperties<T>(object mapTo, T mapFrom)
        {
            var targetProperties = mapTo.GetType().GetProperties();
            var sourceProperties = mapFrom.GetType().GetProperties();

            foreach (var targetProperty in targetProperties)
            {
                foreach (var sourceProperty in sourceProperties)
                {
                    //if (targetProperty.)
                    
                    if (targetProperty.Name == sourceProperty.Name)
                    {
                        targetProperty.SetValue(mapTo, sourceProperty.GetValue(mapFrom, null), null);
                        break;
                    }
                }
            }
        }
    }
}
