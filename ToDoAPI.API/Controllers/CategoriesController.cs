using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ToDoAPI.API.Models;
using ToDoAPI.DATA.EF;
using System.Web.Http.Cors;

namespace ToDoAPI.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class CategoriesController : ApiController
    {

        TodosEntities1 db = new TodosEntities1();

        //GET - api/Categories
        public IHttpActionResult GetCategories()
        {
            List<CategoryViewModel> cats = db.Categories.Select(c => new CategoryViewModel()
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            }).ToList<CategoryViewModel>();

            if (cats.Count == 0)
                return NotFound();

            return Ok(cats);
        }

        //GET - api/Categories/id
        public IHttpActionResult GetCategory(int id)
        {
            CategoryViewModel cat = db.Categories.Where(c => c.CategoryId == id).Select(c => new CategoryViewModel()
            {

                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description

            }).FirstOrDefault();

            if (cat == null)
            {
                return NotFound();

            }

            return Ok(cat);
        }


        //POST -api/categories (HttpPost)
        public IHttpActionResult PostCategory(CategoryViewModel cat)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            db.Categories.Add(new Category()
            {
                Name = cat.Name,
                Description = cat.Description
            });

            db.SaveChanges();
            return Ok();


        }


        //PUT - api/categories (HttpPut)
        public IHttpActionResult PutCategory(CategoryViewModel cat)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Category existingCat = db.Categories.Where(c => c.CategoryId == cat.CategoryId).FirstOrDefault();

            if (existingCat != null)
            {
                existingCat.Name = cat.Name;
                existingCat.Description = cat.Description;
                db.SaveChanges();
                return Ok();
            }

            else
            {
                return NotFound();
            }
        }

        //DELETE - api/categories/id (HttpDelete)
        public IHttpActionResult DeleteCategory(int id)
        {
            Category category = db.Categories.Where(c => c.CategoryId == id).FirstOrDefault();

            if (category != null)
            {
                db.Categories.Remove(category);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }

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
