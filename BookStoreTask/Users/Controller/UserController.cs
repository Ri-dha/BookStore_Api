using BookStoreTask.Users.Admins;
using BookStoreTask.Users.BaseUser.Dto;
using BookStoreTask.Users.Customers;
using BookStoreTask.Users.Payloads;
using BookStoreTask.Users.Service;
using BookStoreTask.Utli;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.Users.Controller;

[Route("user/")]
public class UserController : BaseController
{
    private readonly IUserServices _userServices;

    public UserController(IUserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("all-customers")]
    public async Task<IActionResult> GetUsers([FromQuery] CustomerFilter userFilter)
    {
        var (users, totalCount, error) = await _userServices.GetAllCustomers(userFilter);
        if (error != null) return BadRequest(new { error });
        return Ok(new Page<CustomerDto>()
        {
            Data = users,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / userFilter.PageSize),
            CurrentPage = userFilter.PageNumber,
            TotalCount = totalCount
        });
    }

    [HttpGet("all-admins")]
    public async Task<IActionResult> GetAdmins([FromQuery] AdminFilter userFilter)
    {
        var (users, totalCount, error) = await _userServices.GetAllAdmins(userFilter);
        if (error != null) return BadRequest(new { error });
        return Ok(new Page<AdminDto>()
        {
            Data = users,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / userFilter.PageSize),
            CurrentPage = userFilter.PageNumber,
            TotalCount = totalCount
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        var (user, error) = await _userServices.Get(id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var (user, error) = await _userServices.Delete(id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPost("change-password/{id}")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm changePasswordForm, Guid id)
    {
        var (user, error) =
            await _userServices.ChangePassword(changePasswordForm.OldPassword, changePasswordForm.NewPassword, id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("update-admin/{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateAdmin(Guid id, [FromForm] AdminUpdateForm adminUpdateForm)
    {
        var (user, error) = await _userServices.UpdateAdmin(adminUpdateForm, id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }

    [HttpPut("update-customer/{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateCustomer(Guid id, [FromForm] CustomerUpdateForm customerUpdateForm)
    {
        var (user, error) = await _userServices.UpdateCustomer(customerUpdateForm, id);
        if (error != null) return BadRequest(new { error });
        return Ok(user);
    }


    [HttpGet("get-roles")]
    [AllowAnonymous]
    public async Task<IActionResult> GetRoles()
    {
        var (roles, error) = await _userServices.GetRoles();
        if (error != null) return BadRequest(new { error });
        return Ok(roles);
    }

    [HttpGet("get-admin-roles")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAdminRoles()
    {
        var (roles, error) = await _userServices.GetAdminRoles();
        if (error != null) return BadRequest(new { error });
        return Ok(roles);
    }
}