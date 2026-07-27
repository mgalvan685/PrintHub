using PrintHub.Database.Models;
using PrintHub.DTOs;

namespace PrintHub.Services.Interfaces
{
    public interface IPrinterService
    {
        Task<Printer> CreatePrinterAsync(NewPrinterDto newPrinter);
        Task<PrinterDto?> GetPrinterByIdAsync(int id);
        Task<List<Printer>> GetPrintersAsync();
        Task<PrinterDto?> UpdatePrinterAsync(int id, UpdatePrinterDto dto);
        Task<bool> DeletePrinterAsync(int id);
    }
}