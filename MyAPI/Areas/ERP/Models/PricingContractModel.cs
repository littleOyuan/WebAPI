using System.Collections.Generic;
using System.Runtime.Serialization;
using MyAPI.Base;

namespace MyAPI.Areas.ERP.Models
{
    public class TestModel : ModelBase
    {
        [DataMember(IsRequired = true)]
        public int CustomerId { get; set; }

        [DataMember(IsRequired = true)]
        public List<int> ProductSkuIds { get; set; }
    }
}