using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MiniCMS.Application.DTOs;
using MiniCMS.Application.Interfaces;

namespace MiniCMS.Api.Controllers;

[Authorize] // Bắt buộc phải có JWT Token mới được thao tác
[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly IValidator<CreateCustomerDto> _createValidator;
    private readonly IValidator<UpdateCustomerDto> _updateValidator;

    public CustomersController(
        ICustomerService customerService,
        IValidator<CreateCustomerDto> createValidator,
        IValidator<UpdateCustomerDto> updateValidator)
    {
        _customerService = customerService;
        _createValidator = createValidator;
        _updateValidator = updateValidator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] string? searchTerm, [FromQuery] bool? isActive)
    {
        var customers = await _customerService.GetAllAsync(searchTerm, isActive);
        return Ok(customers);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var customer = await _customerService.GetByIdAsync(id);
        if (customer == null)
        {
            return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}." });
        }
        return Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCustomerDto dto)
    {
        // Kiểm tra FluentValidation
        var validationResult = await _createValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        // Kiểm tra trùng mã KH
        if (await _customerService.ExistByCodeAsync(dto.CustomerCode))
        {
            return BadRequest(new { message = $"Mã khách hàng '{dto.CustomerCode}' đã tồn tại trong hệ thống." });
        }

        // Kiểm tra trùng Email
        if (await _customerService.ExistByEmailAsync(dto.Email))
        {
            return BadRequest(new { message = $"Email '{dto.Email}' đã được sử dụng bởi khách hàng khác." });
        }

        var created = await _customerService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateCustomerDto dto)
    {
        // Kiểm tra FluentValidation
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));
        }

        // Kiểm tra tồn tại
        var existing = await _customerService.GetByIdAsync(id);
        if (existing == null)
        {
            return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}." });
        }

        // Kiểm tra trùng Email với người khác
        if (await _customerService.ExistByEmailAsync(dto.Email, excludeId: id))
        {
            return BadRequest(new { message = $"Email '{dto.Email}' đã được sử dụng bởi khách hàng khác." });
        }

        var updated = await _customerService.UpdateAsync(id, dto);
        return Ok(updated);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _customerService.DeleteAsync(id);
        if (!success)
        {
            return NotFound(new { message = $"Không tìm thấy khách hàng có ID = {id}." });
        }
        return NoContent(); // 204 No Content
    }
}