using BookStoreTask.BookMod.Catograzation.Dto;
using BookStoreTask.BookMod.Catograzation.Payload;
using BookStoreTask.BookMod.Catograzation.Services;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.BookMod.Catograzation.Controllers;

[Route("author/")]
public class AuthorController:BaseController
{
    
    private readonly IAuthorServices _authorServices;

    public AuthorController(IAuthorServices authorServices)
    {
        _authorServices = authorServices;
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> AddAuthor([FromForm] AuthorCreateForm form)
    {
        var (authorDto, error) = await _authorServices.AddAuthorAsync(form);
        if (error != null)return BadRequest(new {error});
        return Ok(authorDto);
    }
    
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateAuthor([FromBody] AuthorUpdateForm form,Guid id)
    {
        var (authorDto, error) = await _authorServices.UpdateAuthorAsync(form,id);
        if (error != null)return BadRequest(new {error});
        return Ok(authorDto);
    }
    
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAuthor(Guid id)
    {
        var (authorDto, error) = await _authorServices.DeleteAuthorAsync(id);
        if (error != null)return BadRequest(new {error});
        return Ok(authorDto);
    }
    
    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetAuthor(Guid id)
    {
        var (authorDto, error) = await _authorServices.GetAuthorAsync(id);
        if (error != null)return BadRequest(new {error});
        return Ok(authorDto);
    }
    
    [HttpPost("get-all")]
    public async Task<IActionResult> GetAuthors([FromBody] AuthorFilterForm form)
    {
        var (authors, totalCount, error) = await _authorServices.GetAuthorsAsync(form);
        if (error != null)return BadRequest(new {error});
        
        return Ok(new Page<AuthorDto>()
        {
            Data = authors,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / form.PageSize),
            CurrentPage = form.PageNumber,
            TotalCount = totalCount
        });
    }
    
    [HttpGet("get-countries")]
    public async Task<IActionResult> GetCountries()
    {
        var (countries, error) = await _authorServices.GetCountriesAsync();
        if (error != null)return BadRequest(new {error});
        return Ok(countries);
    }
    
}