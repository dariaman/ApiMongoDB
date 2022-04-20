﻿using ApiMongoDB.Models;
using ApiMongoDB.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiMongoDB.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookService _booksService;
        public BookController(BookService booksService)
        {
            _booksService = booksService;
        }

        [HttpGet]
        public async Task<List<BookModel>> Get() => await _booksService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<BookModel>> Get(string id)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null) return NotFound();

            return book;
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookModel newBook)
        {
            await _booksService.CreateAsync(newBook);

            return CreatedAtAction(nameof(Get), new { id = newBook.Id }, newBook);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, BookModel updatedBook)
        {
            var book = await _booksService.GetAsync(id);

            if (book is null) return NotFound();

            updatedBook.Id = book.Id;

            await _booksService.UpdateAsync(id, updatedBook);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var book = await _booksService.GetAsync(id);
            
            if (book is null) return NotFound();

            await _booksService.RemoveAsync(id);

            return NoContent();
        }

    }
}
