using Microsoft.EntityFrameworkCore;
using Tutorial8CFef.Data;
using Tutorial8CFef.DTOs;
using Tutorial8CFef.Entities;
using Tutorial8CFef.Exceptions;

namespace Tutorial8CFef.Services;

public class PcsService : IPcsService
{
    private readonly AppDbContext _context;

    public PcsService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PcDto>> GetPcsAsync()
    {
        return await _context.Pcs
            .AsNoTracking()
            .Select(pc => new PcDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock
            })
            .ToListAsync();
    }

    public async Task<PcDetailsDto> GetPcByIdAsync(int id)
    {
        var pc = await _context.Pcs
            .AsNoTracking()
            .Where(pc => pc.Id == id)
            .Select(pc => new PcDetailsDto
            {
                Id = pc.Id,
                Name = pc.Name,
                Weight = pc.Weight,
                Warranty = pc.Warranty,
                CreatedAt = pc.CreatedAt,
                Stock = pc.Stock,

                Components = pc.PcComponents.Select(pcComponent => new PcComponentDto
                {
                    Amount = pcComponent.Amount,
                    Component = new ComponentDto
                    {
                        Code = pcComponent.Component.Code,
                        Name = pcComponent.Component.Name,
                        Description = pcComponent.Component.Description,
                        Manufacturer = new ManufacturerDto
                        {
                            Id = pcComponent.Component.ComponentManufacturer.Id,
                            Abbreviation = pcComponent.Component.ComponentManufacturer.Abbreviation,
                            FullName = pcComponent.Component.ComponentManufacturer.FullName,
                            FoundationDate = pcComponent.Component.ComponentManufacturer.FoundationDate
                        },
                        Type = new ComponentTypeDto
                        {
                            Id = pcComponent.Component.ComponentType.Id,
                            Abbreviation = pcComponent.Component.ComponentType.Abbreviation,
                            Name = pcComponent.Component.ComponentType.Name
                        }
                    }
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (pc is null)
            throw new NotFoundException("PC not found");

        return pc;
    }

    public async Task<PcDto> AddPcAsync(CreatePcDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ConflictException("Invalid PC data");

        var pc = new Pc
        {
            Name = dto.Name,
            Weight = dto.Weight,
            Warranty = dto.Warranty,
            CreatedAt = dto.CreatedAt,
            Stock = dto.Stock
        };

        _context.Pcs.Add(pc);
        await _context.SaveChangesAsync();

        return new PcDto
        {
            Id = pc.Id,
            Name = pc.Name,
            Weight = pc.Weight,
            Warranty = pc.Warranty,
            CreatedAt = pc.CreatedAt,
            Stock = pc.Stock
        };
    }

    public async Task UpdatePcAsync(int id, CreatePcDto dto)
    {
        var pc = await _context.Pcs
            .FirstOrDefaultAsync(pc => pc.Id == id);

        if (pc is null)
            throw new NotFoundException("PC not found");

        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ConflictException("Invalid PC data");

        pc.Name = dto.Name;
        pc.Weight = dto.Weight;
        pc.Warranty = dto.Warranty;
        pc.CreatedAt = dto.CreatedAt;
        pc.Stock = dto.Stock;

        await _context.SaveChangesAsync();
    }

    public async Task DeletePcAsync(int id)
    {
        var pc = await _context.Pcs
            .Include(pc => pc.PcComponents)
            .FirstOrDefaultAsync(pc => pc.Id == id);

        if (pc is null)
            throw new NotFoundException("PC not found");

        _context.PcComponents.RemoveRange(pc.PcComponents);
        _context.Pcs.Remove(pc);

        await _context.SaveChangesAsync();
    }
}