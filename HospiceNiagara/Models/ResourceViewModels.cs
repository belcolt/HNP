using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    //ViewModels for ALL Resource prefixed Model (ie: Resource, ResourceCategory, ResourceSubCategory)

    public class ResourceIndexView
    {
        public int ID { get; set; }

        public string ResourceDescription { get; set; }

        public string ResourceCategory { get; set; }

        [DisplayName("File")]
        public string FileName { get; set; }
    }
    public class ResourceCatDD
    {
        public int ResourceCategoryID { get; set; }
        public string RCatName { get; set; }
        public string TeamDomainName { get; set; }
    }
}