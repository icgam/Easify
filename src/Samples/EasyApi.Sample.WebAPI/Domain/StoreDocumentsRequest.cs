using System;
using System.Collections.Generic;

namespace EasyApi.Sample.WebAPI.Domain
{
    public class StoreDocumentsRequest
    {
        public StoreDocumentsRequest()
        {
            Documents = new List<Document>();
        }

        public Guid RequestId { get; set; }
        public string Operation { get; set; }

        public Owner Owner { get; set; }
        public IEnumerable<Document> Documents { get; set; }
    }
}