using Microsoft.AspNetCore.Mvc;
using Tutorial8CFef.DTOs;
using Tutorial8CFef.Exceptions;
using Tutorial8CFef.Services;

namespace Tutorial8CFef.Controllers;

[ApiController]
[Route("api/pcs")]
public class PcsController : ControllerBase
{
    private readonly IPcsService _pcsService;

    public PcsController(IPcsService pcsService)
    {
        _pcsService = pcsService;
    }

    [HttpGet]
    public async Task<IActionResult> GetPcs()
    {
        var pcs = await _pcsService.GetPcsAsync();
        return Ok(pcs);
    }

    [HttpGet("{id:int}/components")]
    public async Task<IActionResult> GetPcById(int id)
    {
        try
        {
            var pc = await _pcsService.GetPcByIdAsync(id);
            return Ok(pc);
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> AddPc(CreatePcDto dto)
    {
        try
        {
            var pc = await _pcsService.AddPcAsync(dto);
            return Created($"api/pcs/{pc.Id}", pc);
        }
        catch (ConflictException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdatePc(int id, CreatePcDto dto)
    {
        try
        {
            await _pcsService.UpdatePcAsync(id, dto);
            return Ok();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (ConflictException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePc(int id)
    {
        try
        {
            await _pcsService.DeletePcAsync(id);
            return NoContent();
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
    }
}