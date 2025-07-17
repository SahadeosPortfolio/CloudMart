using Carts.Application.DTOs;
using Carts.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Carts.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CartsController : ControllerBase
{
    private readonly ICartService _service;

    public CartsController(ICartService service)
    {
        _service = service;
    }

    /// <summary>
    /// Get the cart for a specific user.
    /// </summary>
    [HttpGet("{userId:guid}")]
    public async Task<IActionResult> GetCartAsync(Guid userId)
    {
        var cart = await _service.GetCartAsync(userId);
        return Ok(cart);
    }

    /// <summary>
    /// Add an item to a user's cart.
    /// </summary>
    [HttpPost("{userId:guid}/items")]
    public async Task<IActionResult> AddItemAsync(Guid userId, [FromBody] CartItemDto itemDto)
    {
        if (itemDto == null)
            return BadRequest("Item data is required.");

        await _service.AddItemAsync(userId, itemDto);
        return NoContent();
    }

    /// <summary>
    /// Remove an item from a user's cart.
    /// </summary>
    [HttpDelete("{userId:guid}/items/{productId:guid}")]
    public async Task<IActionResult> RemoveItemAsync(Guid userId, Guid productId)
    {
        await _service.RemoveItemAsync(userId, productId);
        return NoContent();
    }
}
