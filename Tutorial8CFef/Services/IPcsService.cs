using Tutorial8CFef.DTOs;

namespace Tutorial8CFef.Services;

public interface IPcsService
{
    Task<List<PcDto>> GetPcsAsync();
    Task<PcDetailsDto> GetPcByIdAsync(int id);
    Task<PcDto> AddPcAsync(CreatePcDto dto);
    Task UpdatePcAsync(int id, CreatePcDto dto);
    Task DeletePcAsync(int id);
}