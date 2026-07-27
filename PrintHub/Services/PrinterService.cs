using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class PrinterService : IPrinterService
{
    private readonly PrintHubContext _context;

    public PrinterService(PrintHubContext context)
    {
        _context = context;
    }

    public async Task<Printer> CreatePrinterAsync(NewPrinterDto newPrinter)
    {
        var printer = new Printer
        {
            Brand = newPrinter.Brand,
            Type = newPrinter.Type,
            Name = newPrinter.Name,
            Power_Per_Hour = newPrinter.Power_Per_Hour
        };

        _context.Printers.Add(printer);
        await _context.SaveChangesAsync();
        return printer;
    }

    public async Task<List<Printer>> GetPrintersAsync()
    {
        return await _context.Printers.ToListAsync();
    }

    public async Task<PrinterDto?> GetPrinterByIdAsync(int id)
    {
        var printer = await _context.Printers.FindAsync(id);
        return printer != null ? new PrinterDto
        {
            Id = printer.Id,
            Brand = printer.Brand,
            Type = printer.Type,
            Name = printer.Name,
            Power_Per_Hour = printer.Power_Per_Hour
        } : null;
    }

    public async Task<PrinterDto?> UpdatePrinterAsync(int id, UpdatePrinterDto dto)
    {
        var printer = await _context.Printers.FirstOrDefaultAsync(p => p.Id == id);
        if (printer == null)
            return null;

        printer.Brand = dto.Brand ?? printer.Brand;
        printer.Type = dto.Type ?? printer.Type;
        printer.Name = dto.Name ?? printer.Name;
        printer.Power_Per_Hour = dto.Power_Per_Hour ?? printer.Power_Per_Hour;

        await _context.SaveChangesAsync();

        return new PrinterDto
        {
            Id = printer.Id,
            Brand = printer.Brand,
            Type = printer.Type,
            Name = printer.Name,
            Power_Per_Hour = printer.Power_Per_Hour
        };
    }

    public async Task<bool> DeletePrinterAsync(int id)
    {
        var printer = await _context.Printers.FirstOrDefaultAsync(p => p.Id == id);
        if (printer == null)
            return false;

        _context.Printers.Remove(printer);
        await _context.SaveChangesAsync();

        return true;
    }

}
