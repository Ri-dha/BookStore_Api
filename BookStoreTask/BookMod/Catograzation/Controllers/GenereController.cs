using BookStoreTask.BookMod.Catograzation.Dto;
using BookStoreTask.BookMod.Catograzation.Payload;
using BookStoreTask.BookMod.Catograzation.Services;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.BookMod.Catograzation.Controllers;

[Route("genere/")]
public class GenereController : BaseController
{
    private readonly IGenreServices _genreServices;

    public GenereController(IGenreServices genreServices)
    {
        _genreServices = genreServices;
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddGenere([FromBody] GenereCreateForm form)
    {
        var (genereDto, error) = await _genreServices.AddGenreAsync(form);
        if (error != null) return BadRequest(new { error });
        return Ok(genereDto);
    }

    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateGenere([FromBody] GenereUpdateForm form, Guid id)
    {
        var (genereDto, error) = await _genreServices.UpdateGenreAsync(form, id);
        if (error != null) return BadRequest(new { error });
        return Ok(genereDto);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteGenere(Guid id)
    {
        var (genereDto, error) = await _genreServices.DeleteGenreAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(genereDto);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetGenere(Guid id)
    {
        var (genereDto, error) = await _genreServices.GetGenreAsync(id);
        if (error != null) return BadRequest(new { error });
        return Ok(genereDto);
    }

    [HttpPost("get-all")]
    public async Task<IActionResult> GetGeneres([FromBody] GenereFilterForm form)
    {
        var (generes, totalCount, error) = await _genreServices.GetGenresAsync(form);
        if (error != null) return BadRequest(new { error });

        return Ok(new Page<GenereDto>()
        {
            Data = generes,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / form.PageSize),
            CurrentPage = form.PageNumber,
            TotalCount = totalCount
        });
    }
}