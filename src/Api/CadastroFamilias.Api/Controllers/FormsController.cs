using CadastroFamilias.Application.DTOs;
using CadastroFamilias.Application.Mappers;
using CadastroFamilias.Core.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CadastroFamilias.Api.Controllers;

/// <summary>
/// Controller REST para Gerenciamento de Formulários
/// Segue padrão REST e Princípios de API Design
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class FormsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IFormRepository _formRepository;

    public FormsController(IMediator mediator, IFormRepository formRepository)
    {
        _mediator = mediator;
        _formRepository = formRepository;
    }

    /// <summary>
    /// Cria um novo formulário de cadastro social
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(FormResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateFormRequest request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var response = await _mediator.Send(request, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
    }

    /// <summary>
    /// Retorna todos os formulários cadastrados
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<FormResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var forms = await _formRepository.GetAllAsync(cancellationToken);
        return Ok(forms.Select(f => f.ToResponse()));
    }

    /// <summary>
    /// Busca formulário por Id
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(FormResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
    {
        var form = await _formRepository.GetByIdAsync(id, cancellationToken);
        
        if (form == null)
            return NotFound($"Formulário com Id {id} não encontrado");
            
        return Ok(form.ToResponse());
    }

    /// <summary>
    /// Exclui formulário
    /// </summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        if (!await _formRepository.ExistsAsync(id, cancellationToken))
            return NotFound($"Formulário com Id {id} não encontrado");
            
        await _formRepository.DeleteAsync(id, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Busca formulários com filtros
    /// </summary>
    [HttpGet("search")]
    [ProducesResponseType(typeof(IEnumerable<FormResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] FormSearchFilters filters, CancellationToken cancellationToken)
    {
        var forms = await _formRepository.SearchAsync(filters, cancellationToken);
        return Ok(forms.Select(f => f.ToResponse()));
    }
}