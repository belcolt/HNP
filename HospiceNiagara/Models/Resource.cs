using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HospiceNiagara.Models
{
    public class Resource
    {
        public int ID { get; set; }

        //File link, need to make sure, File does not get loaded
        [Required]
        [DisplayName("Description")]
        public string FileDesc { get; set; }

        [DisplayName("Date Added")]
        public DateTime DateAdded { get; set; }

        //ResourceType
        [Required]
        [DisplayName("Category")]
        public int ResourceCategoryID { get; set; }
        public virtual ResourceCategory ResourceCategory { get; set; }

        [DisplayName("Subcategory")]
        public int? ResourceSubCategoryID { get; set; }
        public virtual ResourceSubCategory ResourceSubCategory { get; set; }

        [ForeignKey("FileStore")]
        [DisplayName("Attach File")]
        public int FileStoreID { get; set; }
        public virtual FileStore FileStore { get; set; }

        [DisplayName("Display in Panel")]
        [DefaultValue(false)]
        public bool Panel { get; set; }

    }

    //Resource View Models
    public class ResourcePanelItem
    {
        public string Description {get; set;}
        public string FileName{get; set;}
        public int FileID { get; set; }
    }
    public class ResourcePanel
    {
        private List<ResourcePanelItem> rPanelItems = new List<ResourcePanelItem>();

        public ResourcePanel(string panelName, string panelDomain)
        {
            PanelCategory = panelName;
            PanelDomain = panelDomain;
        }
        public string PanelCategory { get; set; }

        public string PanelDomain { get; set; }
        public List<ResourcePanelItem> PanelItems
        {
            get { return rPanelItems; }
        }
        public void AddItem(ResourcePanelItem panelItem)
        {
            rPanelItems.Add(panelItem);
        }

    }

    public class ResourcePanelsCollection
    {
        private List<ResourcePanel> panels = new List<ResourcePanel>();

        public IEnumerable<ResourcePanel> Panels
        {
            get { return panels; }
        }

        public void Add(ResourcePanel rpV)
        {
            panels.Add(rpV);
        }
    }

    //Resouce Category View Models
}