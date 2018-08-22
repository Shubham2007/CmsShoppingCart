using CmsShoppingCart.Models.Data;
using CmsShoppingCart.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CmsShoppingCart.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index() //Get all pages
        {
            //Declare the list of PagesVM
            List<PagesVM> pagesList;

            //Init the list
            using (Db db = new Db())
            {
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PagesVM(x)).ToList();
            }

                //Return view with list
                return View(pagesList);
        }

        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddPage(PagesVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {
                string slug;

                PagesDTO page = new PagesDTO();

                page.Title = model.Title;

                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                if (db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any(x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("","Title or slug already exist");
                    return View(model);
                }

                page.Slug = slug;
                page.Sorting = 100;
                page.HasSidebar = model.HasSidebar;
                page.Body = model.Body;

                //Save DTO
                db.Pages.Add(page);
                db.SaveChanges();

                //Set Tempdata Message
                TempData["SuccessMessage"] = "Page Successfully Added";

                return RedirectToAction("AddPage");
            }            
             
        }

        //GET: Admin/Pages/EditPage/id
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            PagesVM model;

            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);

                if (dto == null)
                {
                    return Content("Page does not exists");
                }

                model = new PagesVM(dto);

                return View(model);
            }
        }

        [HttpPost]
        public ActionResult EditPage(PagesVM model)
        {
            if(!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                int id = model.Id;

                string slug = string.Empty;

                PagesDTO dto = db.Pages.Find(id);

                dto.Title = model.Title;

                if(model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }
                }
                
                //make sure title and slug are unique
                if(db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || 
                    db.Pages.Where(x => x.Id != id).Any(x => x.Slug == model.Slug) )
                {
                    ModelState.AddModelError("","Title or slug already taken");
                    return View(model);
                }

                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //Save dto
                db.SaveChanges();

                TempData["SuccessMessage"] = "Successfully edited";

                return RedirectToAction("EditPage");
            }
                
        }

        public ActionResult PageDetails(int id)
        {
            PagesVM model;

            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);

                if(dto == null)
                {
                    return Content("Page Does Not Exist");
                }

                model = new PagesVM(dto);

                return View(model);
            }
        }

        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                PagesDTO dto = db.Pages.Find(id);

                if (dto != null)
                {
                    db.Pages.Remove(dto);
                    db.SaveChanges();
                }
                else
                {
                    return Content("Page Not Exists");
                }

                return RedirectToAction("Index");
            }
        }

        public void ReorderPages(int[] id)
        {
            using(Db db = new Db())
            {
                int count = 1;

                PagesDTO dto = null;
                foreach(var item in id)
                {
                    dto = db.Pages.Find(item);
                    dto.Sorting = count;

                    db.SaveChanges();
                    count++;
                }
            }
        }

        [HttpGet]
        public ActionResult EditSidebar()
        {
            SidebarVM model = null;

            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);

                model = new SidebarVM(dto);
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
            using (Db db = new Db())
            {
                SidebarDTO dto = db.Sidebar.Find(1);
                dto.Body = model.Body;

                db.SaveChanges();
            }

            TempData["SuccessMessage"] = "Successfully Edited The Sidebar";
            return RedirectToAction("EditSidebar");
        }

    }
}