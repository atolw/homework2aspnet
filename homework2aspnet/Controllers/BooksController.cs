using homework2aspnet.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace homework2aspnet.Controllers
{
    /*Завдання 1 Оновіть GET /api/books, щоб повертався список книг. 
     * Оновіть GET /api/books/{id}, щоб за відсутності запису повертався 404 Not Found з текстом Book not found.
     * Встановіть обмеження маршруту для параметра {id} як цілого числа.
     * Перевірте відповіді та статуси 200 і 404 для наявного та неіснуючого ідентифікатора. 
     * Завдання 2 У BooksController додайте маршрут GET /api/books/search з параметрами рядка запиту: 
     * Обов'язковий параметр title (підрядковий пошук без урахування регістру). 
     * Необов'язковий параметр author (якщо вказано — додатково фільтрувати за автором). 
     * За відсутності обов'язкового title повертайте 400 Bad Request зі зрозумілим повідомленням. 
     * За коректних параметрів повертайте 200 OK і список результатів (дозволено повертати порожній список). 
     * Перевірте сценарії: Пошук тільки за title (є збіги). 
     * Пошук за title + author (звуження результату). 
     * Запит без title (очікується 400 Bad Request).*/
    [ApiController]
    [Route("/api/books")]
    public class BooksController : ControllerBase
    {
        private readonly List<Book> Books = [new Book(0, "The Divine Comedy", "Dante Alighieri"), new Book(1, "The Not divine Comedy", "Dante Alighieri"), new Book(2, "Odyssei", "Homer")];
        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(Books);
        }
        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Book? book = Books.FirstOrDefault(x => x.Id == id);
            return book != null ? Ok(book) : NotFound("Book not found");
        }
        [HttpGet("search")]
        public IActionResult SearchByQuery([FromQuery][Required] string? title, [FromQuery] string? author)
        {
            if (string.IsNullOrWhiteSpace(title))
                return BadRequest("Parameter 'title' is required.");

            var result = Books.Where(b =>
                b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (!string.IsNullOrWhiteSpace(author))
                result = result.Where(b =>
                    b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));

            return Ok(result.ToList());
        }
    }
}
