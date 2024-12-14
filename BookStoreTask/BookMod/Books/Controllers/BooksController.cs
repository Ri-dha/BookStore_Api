using BookStoreTask.BookMod.Books.Dto;
using BookStoreTask.BookMod.Books.Payloads;
using BookStoreTask.BookMod.Books.services;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.BookMod.Books.Controllers;

[Route("books/")]
public class BooksController:BaseController
{

    private readonly IBooksServices _booksServices;

    public BooksController(IBooksServices booksServices)
    {
        _booksServices = booksServices;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddBook([FromForm] BookCreateForm form)
    {
        var (bookDto, error) = await _booksServices.AddBookAsync(form);
        if (error != null)return BadRequest(new {error});
        return Ok(bookDto);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateBook([FromBody] BookUpdateForm form,Guid id)
    {
        var (bookDto, error) = await _booksServices.UpdateBookAsync(form,id);
        if (error != null)return BadRequest(new {error});
        return Ok(bookDto);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var (bookDto, error) = await _booksServices.DeleteBookAsync(id);
        if (error != null)return BadRequest(new {error});
        return Ok(bookDto);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetBook(Guid id)
    {
        var (bookDto, error) = await _booksServices.GetBookAsync(id);
        if (error != null)return BadRequest(new {error});
        return Ok(bookDto);
    }
    
    [HttpPost("get-all")]
    public async Task<IActionResult> GetBooks([FromBody] BookFilterForm form)
    {
        var (books, totalCount, error) = await _booksServices.GetBooksAsync(form);
        if (error != null)return BadRequest(new {error});
        
        return Ok(new Page<BookDto>()
        {
            Data = books,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / form.PageSize),
            CurrentPage = form.PageNumber,
            TotalCount = totalCount
        });
    }
    
    [HttpGet("get-Language")]
    public async Task<IActionResult> GetLanguages()
    {
        var (languages, error) = await _booksServices.GetLanguagesAsync();
        if (error != null)return BadRequest(new {error});
        return Ok(languages);
    }
    
}