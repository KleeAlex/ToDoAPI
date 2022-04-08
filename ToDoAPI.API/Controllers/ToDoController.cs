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
    public class ToDoController : ApiController
    {
        TodosEntities1 db = new TodosEntities1();

        //GET
        public IHttpActionResult GetToDoa()
        {
            List<ToDoItemViewModel> toDos = db.Todos.Include("Category").Select(t => new ToDoItemViewModel()
            {

                Todoid = t.Todoid,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).ToList<ToDoItemViewModel>();

            if (toDos.Count == 0)
            {
                return NotFound();
            }
            return Ok(toDos);

        }

        //GET (id)
        public IHttpActionResult GetToDo(int id)
        {
            ToDoItemViewModel toDo = db.Todos.Include("Category").Where(t => t.Todoid == id).Select(t => new ToDoItemViewModel()
            {

                Todoid = t.Todoid,
                Action = t.Action,
                Done = t.Done,
                CategoryId = t.CategoryId,
                Category = new CategoryViewModel()
                {
                    CategoryId = t.Category.CategoryId,
                    Name = t.Category.Name,
                    Description = t.Category.Description
                }
            }).FirstOrDefault();

            if (toDo == null)
            {
                return NotFound();
            }
            return Ok(toDo);
        }


        //POST
        public IHttpActionResult PostToDo(ToDoItemViewModel toDo)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Todo todoitem = new Todo()
            {
                Action = toDo.Action,
                Done = toDo.Done,
                CategoryId = toDo.CategoryId
            };

            db.Todos.Add(todoitem);
            db.SaveChanges();

            return Ok(todoitem);





        }

        //PUT
        public IHttpActionResult PutToDo(ToDoItemViewModel toDo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data");
            }

            Todo existingTodoitem = db.Todos.Where(t => t.Todoid == toDo.Todoid).FirstOrDefault();

            if (existingTodoitem != null)
            {
                existingTodoitem.Todoid = toDo.Todoid;
                existingTodoitem.Action = toDo.Action;
                existingTodoitem.Done = toDo.Done;
                existingTodoitem.CategoryId = toDo.CategoryId;

                db.SaveChanges();
                return Ok();
            }

            else
            {
                return NotFound();
            }
        }


        //DELETE
        public IHttpActionResult DeleteToDo(int id)
        {
            Todo todoitem = db.Todos.Where(t => t.Todoid == id).FirstOrDefault();

            if (todoitem != null)
            {
                db.Todos.Remove(todoitem);
                db.SaveChanges();
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }


        //DISPOSE
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
