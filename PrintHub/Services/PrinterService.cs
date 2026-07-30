using FluentValidation;
using Microsoft.EntityFrameworkCore;
using PrintHub.Database;
using PrintHub.Database.Models;
using PrintHub.DTOs;
using PrintHub.Services.Interfaces;

namespace PrintHub.Services;

public class PrinterService : BaseService, IPrinterService
{
    private readonly PrintHubContext _context;
    private readonly IValidator<NewPrinterDto> _newValidator;
    private readonly IValidator<UpdatePrinterDto> _updateValidator;

    public PrinterService(PrintHubContext context, IValidator<NewPrinterDto> newValidator, IValidator<UpdatePrinterDto> updateValidator)
    {
        _context = context;
        _newValidator = newValidator;
        _updateValidator = updateValidator;
    }

    public async Task<Printer> CreatePrinterAsync(NewPrinterDto newPrinter)
    {
        var validationResult = await _newValidator.ValidateAsync(newPrinter);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var printer = new Printer
        {
            Brand = newPrinter.Brand,
            Type = newPrinter.Type,
            Name = newPrinter.Name,
            Power_Per_Hour = newPrinter.Power_Per_Hour
        };

        SetCreatedFields(printer);

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
        var validationResult = await _updateValidator.ValidateAsync(dto);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var printer = await _context.Printers.FirstOrDefaultAsync(p => p.Id == id);
        if (printer == null)
            return null;

        printer.Brand = dto.Brand ?? printer.Brand;
        printer.Type = dto.Type ?? printer.Type;
        printer.Name = dto.Name ?? printer.Name;
        printer.Power_Per_Hour = dto.Power_Per_Hour ?? printer.Power_Per_Hour;

        SetUpdatedFields(printer);

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
