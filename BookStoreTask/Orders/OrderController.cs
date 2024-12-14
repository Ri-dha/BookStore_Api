using BookStoreTask.Utli;
using Microsoft.AspNetCore.Mvc;

namespace BookStoreTask.Orders;

[Route("orders/")]
public class OrderController : BaseController
{
    private readonly IOrdersService _ordersServices;

    public OrderController(IOrdersService ordersServices)
    {
        _ordersServices = ordersServices;
    }


    [HttpPut("accept/{id}")]
    public async Task<IActionResult> AcceptOrder(Guid id)
    {
        var (orderDto, error) = await _ordersServices.AcceptOrder(id);
        if (error != null) return BadRequest(new { error });
        return Ok(orderDto);
    }

    [HttpPut("ship/{id}")]
    public async Task<IActionResult> ShipOrder(Guid id)
    {
        var (orderDto, error) = await _ordersServices.ShipOrder(id);
        if (error != null) return BadRequest(new { error });
        return Ok(orderDto);
    }

    [HttpPut("deliver/{id}")]
    public async Task<IActionResult> DeliverOrder(Guid id)
    {
        var (orderDto, error) = await _ordersServices.DeliverOrder(id);
        if (error != null) return BadRequest(new { error });
        return Ok(orderDto);
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var (orderDto, error) = await _ordersServices.GetOrder(id);
        if (error != null) return BadRequest(new { error });
        return Ok(orderDto);
    }

    [HttpPost("get-all")]
    public async Task<IActionResult> GetOrders([FromQuery] OrderFilterForm form)
    {
        var (orders, totalCount, error) = await _ordersServices.GetOrders(form);

        if (error != null) return BadRequest(new { error });

        return Ok(new Page<OrdersDto>()
        {
            Data = orders,
            PagesCount = (int)Math.Ceiling((double)(totalCount ?? 0) / form.PageSize),
            CurrentPage = form.PageNumber,
            TotalCount = totalCount
        });
    }
}