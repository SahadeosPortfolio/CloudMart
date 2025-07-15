using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Products.Application.Dtos;
using Products.Application.Interfaces;

namespace Products.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
//[Authorize(Policy = Policies.AdminOnly)]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    /// <summary>
    /// Retrieves a list of products based on the specified filter and sorting criteria.
    /// </summary>
    /// <param name="productQueryParameters"></param>
    /// <returns></returns>

    [HttpGet]
    [ProducesResponseType(typeof(PaginatedResponse<ProductResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> GetAllAsync([FromQuery] ProductQueryParameters productQueryParameters)
    {
        var products = await _productService.GetAllAsync(productQueryParameters);

        return Ok(products);
    }

    /// <summary>
    /// Retrieves a product by its unique identifier.
    /// </summary>
    /// <remarks>This action returns an HTTP 200 response with the product data if the product is found, or an
    /// HTTP 404 response if the product does not exist.</remarks>
    /// <param name="id">The unique identifier of the product to retrieve.</param>
    /// <returns>An <see cref="IActionResult"/> containing the product if found, or a <see cref="NotFoundResult"/> if no product
    /// exists with the specified identifier.</returns>

    [HttpGet("{id}", Name = "GetProductById")]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous] // Allow anonymous access for demonstration purposes; adjust as needed.
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        return product is null
            ? NotFound()
            : Ok(product);
    }

    /// <summary>
    /// Creates a new product using the provided product data and returns the result.
    /// </summary>
    /// <remarks>The response includes the URI of the newly created product resource in the "Location"
    /// header.</remarks>
    /// <param name="productDto">The product data to create a new product. Cannot be null.</param>
    /// <returns>An <see cref="IActionResult"/> containing the HTTP response.  Returns <see langword="BadRequest"/> if <paramref
    /// name="productDto"/> is null.  Returns <see langword="Created"/> with the created product and its location if
    /// successful.</returns>

    [HttpPost]
    [ProducesResponseType(typeof(ProductResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> CreateAsync([FromBody] ProductCreateRequestDto productCreateRequestDto)
    {
        var createdProduct = await _productService.CreateAsync(productCreateRequestDto);

        return CreatedAtRoute("GetProductById", new { id = createdProduct?.Id }, createdProduct);
    }

    /// <summary>
    /// Updates an existing product.
    /// </summary>
    /// <param name="productId"></param>
    /// <param name="productUpdateRequestDto"></param>
    /// <returns></returns>
    [HttpPut("{productId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> UpdateAsync(Guid productId, [FromBody] ProductUpdateRequestDto productUpdateRequestDto)
    {
        var updated = await _productService.UpdateAsync(productId, productUpdateRequestDto);

        return updated
            ? NoContent() : NotFound();
    }

    /// <summary>
    /// Soft deleting a product by marking it as deleted
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    [HttpDelete]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [AllowAnonymous]
    public async Task<IActionResult> DeleteAsync(Guid productId)
    {
        var deleted = await _productService.DeleteAsync(productId);

        return deleted
             ? NoContent() : NotFound();
    }
}