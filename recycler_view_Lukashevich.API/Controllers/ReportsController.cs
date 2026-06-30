using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using recycler_view_Lukashevich.Data;
using recycler_view_Lukashevich.Models;

namespace recycler_view_Lukashevich.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin,Librarian")]
public class ReportsController : ControllerBase
{
    private readonly LibraryDbContext _context;

    public ReportsController(LibraryDbContext context)
    {
        _context = context;
    }

    [HttpGet("debtors")]
    public async Task<IActionResult> GetDebtorsReport()
    {
        try
        {
            var debtors = await _context.Loans
                .Where(l => l.Status == "Active" && l.DueDate < DateTime.Now)
                .Include(l => l.Book)
                .Include(l => l.User)
                .ToListAsync();

            // Если нет данных — вернём пустой отчёт
            if (debtors == null || debtors.Count == 0)
            {
                return Ok(new { message = "Нет должников" });
            }

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Должники");

            ws.Cells[1, 1].Value = "Читатель";
            ws.Cells[1, 2].Value = "Книга";
            ws.Cells[1, 3].Value = "Дата выдачи";
            ws.Cells[1, 4].Value = "План. возврат";
            ws.Cells[1, 5].Value = "Просрочка (дней)";
            ws.Cells[1, 1, 1, 5].Style.Font.Bold = true;

            int row = 2;
            foreach (var loan in debtors)
            {
                ws.Cells[row, 1].Value = loan.User?.FullName ?? "Неизвестно";
                ws.Cells[row, 2].Value = loan.Book?.Title ?? "Неизвестно";
                ws.Cells[row, 3].Value = loan.LoanDate.ToString("dd.MM.yyyy");
                ws.Cells[row, 4].Value = loan.DueDate.ToString("dd.MM.yyyy");
                ws.Cells[row, 5].Value = (DateTime.Now - loan.DueDate).Days;
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Debtors.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularBooksReport()
    {
        try
        {
            var popular = await _context.Loans
                .GroupBy(l => l.BookId)
                .Select(g => new { BookId = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count)
                .Take(10)
                .Join(_context.Books, x => x.BookId, b => b.Id, (x, b) => new { b.Title, b.Author, x.Count })
                .ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Популярные книги");

            ws.Cells[1, 1].Value = "Название";
            ws.Cells[1, 2].Value = "Автор";
            ws.Cells[1, 3].Value = "Кол-во выдач";
            ws.Cells[1, 1, 1, 3].Style.Font.Bold = true;

            for (int i = 0; i < popular.Count; i++)
            {
                ws.Cells[i + 2, 1].Value = popular[i].Title;
                ws.Cells[i + 2, 2].Value = popular[i].Author;
                ws.Cells[i + 2, 3].Value = popular[i].Count;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "PopularBooks.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }

    [HttpGet("loans-period")]
    public async Task<IActionResult> GetLoansPeriodReport([FromQuery] DateTime from, [FromQuery] DateTime to)
    {
        try
        {
            var loans = await _context.Loans
                .Where(l => l.LoanDate >= from && l.LoanDate <= to)
                .Include(l => l.Book)
                .Include(l => l.User)
                .ToListAsync();

            using var package = new ExcelPackage();
            var ws = package.Workbook.Worksheets.Add("Выдачи за период");

            ws.Cells[1, 1].Value = "Читатель";
            ws.Cells[1, 2].Value = "Книга";
            ws.Cells[1, 3].Value = "Дата выдачи";
            ws.Cells[1, 4].Value = "Статус";
            ws.Cells[1, 1, 1, 4].Style.Font.Bold = true;

            int row = 2;
            foreach (var loan in loans)
            {
                ws.Cells[row, 1].Value = loan.User?.FullName ?? "Неизвестно";
                ws.Cells[row, 2].Value = loan.Book?.Title ?? "Неизвестно";
                ws.Cells[row, 3].Value = loan.LoanDate.ToString("dd.MM.yyyy");
                ws.Cells[row, 4].Value = loan.Status;
                row++;
            }

            ws.Cells[ws.Dimension.Address].AutoFitColumns();
            var stream = new MemoryStream(package.GetAsByteArray());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Loans_{from:yyyyMMdd}-{to:yyyyMMdd}.xlsx");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message, inner = ex.InnerException?.Message });
        }
    }
}