using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
    public class AuthorController : Controller
    {
        private readonly IBookStoreRepository<Author> authorRepository; 
        public AuthorController(IBookStoreRepository<Author> authorRepository)
        {
            this.authorRepository = authorRepository; 
        }
        // GET: AuthorController
        public ActionResult Index()
        {
            var authors = authorRepository.List(); 
            return View(authors);
        }

        // GET: AuthorController/Details/5
        public ActionResult Details(int id)
        {
            var author = authorRepository.Find(id);
            return View(author);
        }

        // GET: AuthorController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AuthorController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Author author)
        {
            try
            {
                authorRepository.Add(author); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Error Ocured"; 
                return View();
            }
        }

        // GET: AuthorController/Edit/5
        public ActionResult Edit(int id)
        {
            var author = authorRepository.Find(id); 
            return View(author);
        }

        // POST: AuthorController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Author author)
        {
            try
            {
                authorRepository.Update(author); 
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Error Ocured";
                return Edit(author.Id);
            }
        }

        // GET: AuthorController/Delete/5
        public ActionResult Delete(int id)
        {
            var author = authorRepository.Find(id); 
            return View(author);
        }

        // POST: AuthorController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            // To Delete an author, there must be no books for him 
            try
            {
                authorRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Error Ocured";
                return RedirectToAction(nameof(Index));
            }
        }
        public ActionResult Search(string term)
        {
            if (term == null)
                return View("Index", authorRepository.List());
            var result = authorRepository.Search(term);
            return View("Index", result);
        }
    }
}
