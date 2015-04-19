using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HospiceNiagara.DAL;
using HospiceNiagara.Models;
using System.IO;
using System.Web.Services;
using Microsoft.AspNet.Identity;
using HospiceNiagara.HospiceUserExtensions;

namespace HospiceNiagara.Controllers
{
    [Authorize]
    public class ResourcesController : Controller
    {
        private HospiceNiagaraContext db = new HospiceNiagaraContext();
        //Enumeration for quick matching of Domain IDs as in the Hospice Database.
        //Example: Check for specific DomainID on the ResourceCategory with db.ResourceCategories.Where(rc=>rc.TeamDomainID==(int)Domains.[nameOfDomain] and so forth
        //enum Domains { Volunteer = 1, Staff = 2, Board = 3, Organizational = 4 };
        // GET: Resources
        
        [Authorize]
        public ActionResult Index(bool? newUser)
        {
            //Collect Categories that are "panel" categories
            var categories = db.ResourceCategories.ToList();
            var resources = db.Resources.Include(r => r.FileStore).Include(r => r.ResourceCategory);
            var panelResources = db.Resources.Include(r => r.FileStore).Include(r => r.ResourceCategory).ToList();
            ResourcePanelsCollection rPC = new ResourcePanelsCollection();
            foreach(var cat in categories)
            {
                if (cat.Panel)
                {
                    string domain = ((Domains)cat.TeamDomainID).ToString();
                    ResourcePanel rpVM = new ResourcePanel(cat.Name,domain);
                    var temp = panelResources.Where(pr => pr.ResourceCategoryID == cat.ID).ToList();
                    foreach (var resource in temp)
                    {
                        if (resource.Panel)
                        {
                            var fileSource = resource.FileStore;
                            ResourcePanelItem rpI = new ResourcePanelItem{FileName=fileSource.FileName,Description=resource.FileDesc,FileID=fileSource.ID};
                            rpVM.AddItem(rpI);
                        }
                    }
                    rPC.Add(rpVM);
                    //get rid of temp resources, already handled
                    //if (temp.Count != 0)
                    //{
                    //    panelResources = (List<Resource>)panelResources.Except(temp);
                    //}
                }
            }
            ViewBag.PanelCollection = rPC;
            if (newUser.HasValue)
            {
                if (newUser.Value)
                {
                    int team = User.GetHospiceContact().TeamDomainID;
                string teamName = ((Domains)team).ToString();
                string clickHere = "This is the resource panel. Listed below are Organizational Resources and Links. Clicking on a file name in the resources tab will download the resource file listed. These resources contain important information for all members of Hospice Niagara. Clicking on the "+teamName+" Resources tab will display further documents and links relevant to you and your role at Hospice Niagara. You can come back to this screen at any time by logging in and clicking the Resources navigation link located on top of your screen.";
                ViewBag.NewUserMessage = clickHere;
                }
            }
            //ViewBag.VolunteerResources = vRes;
            return View(resources);
        }

        [Authorize(Roles = "Administrator")]
        [SessionTracking.Logging]
        public ActionResult ResourceAdminIndex(string SearchString)
        {
            var resources = db.Resources.Include(r => r.FileStore).Include(r => r.ResourceCategory);
            ViewBag.VolunteerResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 1).ToList();
            ViewBag.StaffResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 2).ToList();
            ViewBag.OrgResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 4).ToList();
            ViewBag.BoardResources = resources.Where(r => r.ResourceCategory.TeamDomainID == 3).ToList();
            if (!String.IsNullOrEmpty(SearchString))
            {
                resources = resources.Where(d => d.FileDesc.ToUpper().Contains(SearchString.ToUpper()));
                ViewBag.Resources = resources.ToList();
                return View(resources);
            }
            ViewBag.Resources = resources.ToList();
            return View(resources);
        }
        
        //Details Views

        // GET: Resources/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // GET: Resources/Create
        [Authorize(Roles="Administrator")]
        public ActionResult Create()
        {
            PopulateCategoryDropdown(null);
            return View();
        }

        public JsonResult LoadSubcategories(int catID)
        {
            var sCats = db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == catID).Select(sc => new { sc.ID, sc.Name });
            return Json(sCats, JsonRequestBehavior.AllowGet);

        }

        // POST: Resources/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create([Bind(Include = "ID,FileDesc,Panel,ResourceCategoryID,ResourceSubCategoryID")] Resource resource)
        {
            var files = Request.Files;
            string mimeType = Request.Files[0].ContentType;
            string fileName = Path.GetFileName(Request.Files[0].FileName);
            int fileLength = Request.Files[0].ContentLength;
            if (!(fileName == "" || fileLength == 0))
            {
                Stream fileStream = Request.Files[0].InputStream;
                byte[] fileData = new byte[fileLength];
                fileStream.Read(fileData, 0, fileLength);
                //resource type already existent
                resource.DateAdded = Convert.ToDateTime(DateTime.Now);
                resource.FileStore = new FileStore
                {
                    FileContent = fileData,
                    MimeType = mimeType,
                    FileName = fileName
                };
            };
            if (resource.ResourceSubCategoryID==-1)
            {
                resource.ResourceSubCategoryID = null;
            }
            if (ModelState.IsValid)
            {
                db.Resources.Add(resource);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ResourceCategoryD = new SelectList(db.ResourceCategories, "ID", "Name", resource.ResourceCategoryID);
            return View(resource);
        }
        // GET: Resources/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            return View(resource);
        }

        // POST: Resources/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Resource resource = db.Resources.Find(id);
            db.Resources.Remove(resource);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        // GET: Resources/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Resource resource = db.Resources.Find(id);
            if (resource == null)
            {
                return HttpNotFound();
            }
            PopulateCategoryDropdown(resource.ResourceCategoryID);
            return View(resource);
        }

        // POST: Resources/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit([Bind(Include = "ID,FileDesc,ResourceCategoryID,ResourceSubCategoryID,FileStoreID,Panel,DateAdded")] Resource resource)
        {
            if (ModelState.IsValid)
            {
                db.Entry(resource).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            PopulateCategoryDropdown(null);
            return View(resource);
        }

        //Resource Category Action Methods
        ////////////////////////////////////////////////////////////////////
        public ActionResult ResourceCategoryIndex(int? domID)
        {
            var resourceCats = db.ResourceCategories.ToList();
            if (TempData["CategoryAdded"] != null)
            {
                bool success = (bool)TempData["CategoryAdded"];
                if (success)
                {
                    ViewBag.CategoryStatus = "<span class='text-success'>Category successfully added.</span>";
                }
                else
                    ViewBag.CategoryStatus = "<span class='text-success'>Category did not success fully add.</span>";
            }
            List<ResourceCategoryViewModel> rCV = new List<ResourceCategoryViewModel>();
            if (domID!=null)
            {
                resourceCats.Where(rc => rc.TeamDomainID == domID);
            }
            foreach (var rc in resourceCats)
            {
                rCV.Add(new ResourceCategoryViewModel { CategoryID = rc.ID, Panel = rc.Panel, Name = rc.Name, TeamDomain = ((Domains)rc.TeamDomainID).ToString() });
            }
            return View(rCV);
        }
        public ActionResult CreateResourceCategory()
        {
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            return View();
        }

        [HttpPost]
        public ActionResult CreateResourceCategory([Bind(Include = "ID,TeamDomainID,Name,Panel")]ResourceCategory rCat)
        {
            if (ModelState.IsValid)
            {
                db.ResourceCategories.Add(rCat);
                db.SaveChanges();
                TempData["CategoryAdded"] = true;
                return RedirectToAction("ResourceCategoryIndex");
            }
            ViewBag.TeamDomainID = new SelectList(db.TeamDomains, "ID", "Description");
            return View(rCat);
        }
        

        // GET: Resources/EditResourceCategory/5
        [Authorize(Roles = "Administrator")]
        public ActionResult EditResourceCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceCategory rCat= db.ResourceCategories.Find(id);
            if (rCat == null)
            {
                return HttpNotFound();
            }
            PopulateDomainDropdown(id);
            return View(rCat);
        }

        // POST: Resources/EditResourceCategory/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult EditResourceCategory([Bind(Include = "ID,Name,TeamDomainID,Panel")] ResourceCategory rCat)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rCat).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ResourceCategoryIndex");
            }
            PopulateDomainDropdown(rCat.ID);
            return View(rCat);
        }

        // GET: Resources/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult DeleteResourceCategory(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResourceCategory rCat = db.ResourceCategories.Find(id);
            var resources = db.Resources.Where(r => r.ResourceCategoryID == id).ToList();
            if (resources.Count!=0)
            {
                ViewBag.DisableDelete = true;
                ViewBag.DeleteCategoryMessage = "<span class='text-warning'>There are currently resources with this category type. Please edit them before removing this category.</span>";
                return View(rCat);
            }
            ViewBag.DisableDelete = false;
            ViewBag.DeleteCategoryMessage = "Are you sure you want to delete this?";
            if (rCat == null)
            {
                return HttpNotFound();
            }
            return View(rCat);
        }

        // POST: Resources/Delete/5
        [Authorize(Roles = "Administrator")]
        [HttpPost, ActionName("DeleteResourceCategory")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteResourceCategoryConfirmed(int id)
        {
            ResourceCategory rc = db.ResourceCategories.Find(id);
            db.ResourceCategories.Remove(rc);
            db.SaveChanges();
            return RedirectToAction("ResourceCategoryIndex");
        }
        //Resource Subcategories
        /////////////////////////////////////////
        public ActionResult ResourceSubcategories(int? catID)
        {
            var subcats = catID != null ? db.ResourceSubCategories.Where(sc => sc.ResourceCategoryID == catID).ToList() : db.ResourceSubCategories.ToList() ;
            //if (catID!=null)
            //{
            //    subcats.Where(sc => sc.ResourceCategoryID == catID);
            //}
            if (catID!=null)
            {
                string catName = db.ResourceCategories.Where(rc => rc.ID == catID).Select(rc => rc.Name).Single().ToString();
                string message;
                if (subcats.Count == 0)
                {
                    message = "<span class='text-warning'>No subcategories found for " + catName + "</span>";
                }
                else
                {
                    message = "<span class='text-success'>Displaying subcategories for " + catName + "</span>";
                }
                ViewBag.Message = message;
            }
            
            return View(subcats.ToList());
        }
        
        //////////////Resource Controller Methods
        public void PopulateCategoryDropdown(int? id)
        {
            var rcs = db.ResourceCategories.OrderBy(rc => rc.TeamDomainID).ToList();
            var teamNames = db.TeamDomains.OrderBy(td => td.ID).Select(td => td.Description).ToList();
            ViewBag.AllSubCats = db.ResourceSubCategories.ToList();
            List<ResourceCatDD> rcats = new List<ResourceCatDD>();
            foreach (var rc in rcs)
            {
                rcats.Add(new ResourceCatDD { ResourceCategoryID = rc.ID, RCatName = rc.Name, TeamDomainName = teamNames[rc.TeamDomainID - 1] });
            }
            ViewBag.ResourceCategories = new SelectList(rcats, "ResourceCategoryID", "RCatName", "TeamDomainName", id, null, null);
        }
        public void PopulateDomainDropdown(int?id)
        {
            var teams = db.TeamDomains.OrderBy(td => td.Description).ToList();
            ViewBag.TeamDomains = new SelectList(teams, "ID", "Description", id);
        }
        [SessionTracking.TrackDownload]
        public FileContentResult Download(int id)
        {
            var theFile = db.FileStores.Where(f => f.ID == id).SingleOrDefault();
            return File(theFile.FileContent, theFile.MimeType, theFile.FileName);
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}