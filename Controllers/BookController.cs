using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BookStore.Models;
using BookStore.Models.Repositories;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Controllers
{
    [Authorize]
    public class BookController : Controller
    {
        private readonly IBookStoreRepository<Book> bookRepository;
        private readonly IBookStoreRepository<Author> authorRepository;
        private readonly IWebHostEnvironment hosting;

        public BookController(IBookStoreRepository<Book> bookRepository, IBookStoreRepository<Author> authorRepository, IWebHostEnvironment hosting)
        {
            this.bookRepository = bookRepository;
            this.authorRepository = authorRepository;
            this.hosting = hosting;
        }
        // GET: BookController
        public ActionResult Index()
        {
            var books = bookRepository.List(); 
            return View(books);
        }

        // GET: BookController/Details/5
        public ActionResult Details(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // GET: BookController/Create
        public ActionResult Create()
        {
            return View(GetAllAuthors());
        }

        // POST: BookController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(BookAuthorViewModel model)
        {
            if (Validate(model.AuthorId) == false)
                return Create(); 

            try
            {
                var author = authorRepository.Find(model.AuthorId);
                model.ImageUrl = model.File.FileName;
                var book = GetNewBook(model, author);
                bookRepository.Add(book);
                UploadFile(model.File, book.Id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return Create();
            }
        }

        

        public ActionResult Search(string term)
        {
            if (term == null)
                return View("Index", bookRepository.List()); 
            var result = bookRepository.Search(term);
            return View("Index", result);
        }

        // GET: BookController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = GetNewModel(id);
            return View(model);
        }

        // POST: BookController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(BookAuthorViewModel model)
        {
            try
            {
                var author = authorRepository.Find(model.AuthorId);
                model.ImageUrl = UpdateFile(model.File, model.ImageUrl, model.BookId);
                var book = GetNewBook(model, author);
                bookRepository.Update(book);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Message = "Error Occured";
                return Edit(model.BookId);
            }
        }

        // GET: BookController/Delete/5
        public ActionResult Delete(int id)
        {
            var book = bookRepository.Find(id);
            return View(book);
        }

        // POST: BookController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmDelete(int id)
        {
            try
            {
                DeleteDir(id); 
                bookRepository.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                ViewBag.Massage = "Error Occured";
                return Delete(id); 
            }
        }
        string UpdateFile(IFormFile File, string ImageUrl, int Id)
        {
            if (File != null)
            {
                DeleteFile(ImageUrl, Id);
                UploadFile(File, Id);
                return File.FileName; 
            }
            return ImageUrl; 
        }
        void UploadFile(IFormFile File, int Id)
        {
            if (File != null)
            {
                string path = CreateDir(Id); 
                string FileName = File.FileName;
                string FullPath = Path.Combine(path, FileName);
                File.CopyTo(new FileStream(FullPath, FileMode.Create));
            }
        }
        void DeleteDir(int Id)
        {
            try
            {
                string path = Path.Combine(hosting.WebRootPath, Path.Combine("uploads", Id.ToString()));
                // second parameter to delete sub-dirs 
                Directory.Delete(path, true);
            }catch
            {

            }
        }
        void DeleteFile(string FileName, int Id)
        {
            string path = Path.Combine(hosting.WebRootPath, Path.Combine("uploads",Id.ToString()));
            string FullPath = Path.Combine(path, FileName);
            try
            {
                System.IO.File.Delete(FullPath);
            }
            catch
            {
            }
        }
        string CreateDir(int Id)
        {
            string path = Path.Combine(hosting.WebRootPath, Path.Combine("uploads", Id.ToString()));
            Directory.CreateDirectory(path);
            return path;
        }
        Book GetNewBook(BookAuthorViewModel model, Author author)
        {
            return new Book 
            {
                Id = model.BookId,
                Title = model.Title,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Author = author
            };
        }
        BookAuthorViewModel GetNewModel(int Id)
        {
            var book = bookRepository.Find(Id);
            return new BookAuthorViewModel 
            {
                BookId = book.Id,
                Title = book.Title,
                Description = book.Description,
                AuthorId = book.Author.Id,
                ImageUrl = book.ImageUrl,
                Authors = authorRepository.List().ToList()
            };
        }
        private bool Validate(int Id)
        {
            bool Valid = true;
            if (ModelState.IsValid == false)
                Valid = false;
            if (Id == -1)
            {
                ViewBag.Message = "Please select an author from the list!";
                Valid = false;
            }
            return Valid;
        }
        BookAuthorViewModel GetAllAuthors()
        {
            return new BookAuthorViewModel { Authors = FillSelectList() };
        }
        List<Author> FillSelectList()
        {
            var authors = authorRepository.List().ToList();
            authors.Insert(0, new Author { Id = -1, FullName = "--- Please Select an author ---" });
            return authors; 
        }
    }
}
