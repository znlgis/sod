using System;
using System.Linq;
using System.Xml;
using Microsoft.OData.Client;
using Microsoft.OData.Edm;
using Microsoft.OData.Edm.Csdl;

namespace PWMIS.OData.Client
{
    /// <summary>
    ///     OData V4 Version ASP.NET WebAPI OData RestFull Client Context
    ///     <remarks>v1.0 2015.4.1 http://www.pwmis.com/sqlmap </remarks>
    /// </summary>
    public class ODataV4ContextBase : DataServiceContext
    {
        /// <summary>
        ///     V4 OData Init
        /// </summary>
        /// <param name="serviceRoot">V4 OData ASP.NET WebAPI url base</param>
        public ODataV4ContextBase(string serviceRoot)
            : base(new Uri(serviceRoot), ODataProtocolVersion.V4)
        {
            if (!serviceRoot.EndsWith("/"))
                serviceRoot = serviceRoot + "/";
            var gem = new GeneratedEdmModel(serviceRoot);
            Format.LoadServiceModel = gem.GetEdmModel;
            Format.UseJson();
        }

        /// <summary>
        ///     Create a New OData Service Query
        /// </summary>
        /// <typeparam name="T">entity Type class</typeparam>
        /// <param name="name">entitySetName</param>
        /// <returns></returns>
        public IQueryable<T> CreateNewQuery<T>(string name) where T : class
        {
            return base.CreateQuery<T>(name);
        }

        private class GeneratedEdmModel
        {
            private readonly string ServiceRootUrl;

            public GeneratedEdmModel(string serviceRootUrl)
            {
                ServiceRootUrl = serviceRootUrl;
            }

            public IEdmModel GetEdmModel()
            {
                var metadataUrl = ServiceRootUrl + "$metadata";
                return LoadModelFromUrl(metadataUrl);
            }

            private IEdmModel LoadModelFromUrl(string metadataUrl)
            {
                var reader = CreateXmlReaderFromUrl(metadataUrl);
                try
                {
                    return CsdlReader.Parse(reader);
                }
                finally
                {
                    ((IDisposable)reader).Dispose();
                }
            }

            private static XmlReader CreateXmlReaderFromUrl(string inputUri)
            {
                return XmlReader.Create(inputUri);
            }
        }
    }
}