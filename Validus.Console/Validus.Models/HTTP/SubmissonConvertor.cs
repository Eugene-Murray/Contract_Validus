using System;
using System.Runtime.Remoting.Messaging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Validus.Models.HTTP
{
    public class SubmissonConvertor : JsonCreationConverter<Submission>
    {

        protected override Submission Create(Type objectType, JObject jObject)
        {
         var xSubmissionType =  CallContext.LogicalGetData("X-SubmissionType") as string;     
            switch (xSubmissionType)
            {
                case "EN":
                    {
                        return new SubmissionEN();
                    }
                case "PV":
                    {
                        return new SubmissionPV();
                    }
                case "FI":
                    {
                        return new Submission();
                    }
                case "CA":
                    {
                        return new Submission();
                    }
                case "HM":
                    {
                        return new Submission();
                    }
                case "ME":
                    {
                        return new Submission();
                    }
                    break;
            }
            return new Submission();
        }
    }

    public class OptionConvertor : JsonCreationConverter<Option>
    {
        protected override Option Create(Type objectType, JObject jObject)
        {
            var xSubmissionType = CallContext.LogicalGetData("X-SubmissionType") as string;
            switch (xSubmissionType)
            {
                case "EN":
                    {
                        return new Option();
                    }
                case "PV":
                    {
                        return new Option();
                    }
                case "FI":
                    {
                        return new OptionFI();
                    }
                case "CA":
                    {
                        return new Option();
                    }
                case "HM":
                    {
                        return new Option();
                    }
                case "ME":
                    {
                        return new Option();
                    }
                    break;
            }
            return new Option();
        }
    }
     
    public class OptionVersionConvertor : JsonCreationConverter<OptionVersion>
    {
        protected override OptionVersion Create(Type objectType, JObject jObject)
        {
            var xSubmissionType = CallContext.LogicalGetData("X-SubmissionType") as string;
            switch (xSubmissionType)
            {
                case "EN":
                    {
                        return new OptionVersion();
                    }
                case "PV":
                    {
                        return new OptionVersionPV();
                    }
                case "FI":
                    {
                        return new OptionVersion();
                    }
                case "CA":
                    {
                        return new OptionVersionCA();
                    }
                case "HM":
                    {
                        return new OptionVersionHM();
                    }
                case "ME":
                    {
                        return new OptionVersion();
                    }
                    break;
            }
            return new OptionVersion();
        }
    }

    public class QuoteConvertor : JsonCreationConverter<Quote>
    {
        protected override Quote Create(Type objectType, JObject jObject)
        {
            var xSubmissionType = CallContext.LogicalGetData("X-SubmissionType") as string;
            switch (xSubmissionType)
            {
                case "EN":
                    {
                        return new QuoteEN();
                    }
                case "PV":
                    {
                        return new QuotePV();
                    }
                case "FI":
                    {
                        return new QuoteFI();
                    }
                case "CA":
                    {
                        return new QuoteCA();
                    }
                case "HM":
                    {
                        return new QuoteHM();
                    }
                case "ME":
                    {
                        return new QuoteME();
                    }
                    break;
            }
            return new Quote();
        }
    }

    public abstract class JsonCreationConverter<T> : JsonConverter
    {
        /// <summary>
        /// Create an instance of objectType, based properties in the JSON object
        /// </summary>
        /// <param name="objectType">type of object expected</param>
        /// <param name="jObject">contents of JSON object that will be deserialized</param>
        /// <returns></returns>
        protected abstract T Create(Type objectType, JObject jObject);

        public override bool CanConvert(Type objectType)
        {
            return typeof(T).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            var jObject = JObject.Load(reader);

            // Create target object based on JObject
            T target = Create(objectType, jObject);

            // Populate the object properties
            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }

        public override bool CanWrite
        {
            get
            {
                return false;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            //http://stackoverflow.com/questions/12856853/webapi-json-net-custom-date-handling
            throw new NotImplementedException();
        }
    }
}
    
