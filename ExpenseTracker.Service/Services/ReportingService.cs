using ExpenseTracker.Repository.Constants;
using ExpenseTracker.Repository.Models;
using ExpenseTracker.Service.Interfaces;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace ExpenseTracker.Service.Services;

public class ReportingService
{
    private readonly ITransactionService _transactionService;
    private readonly IScheduledService _scheduledService;
    public ReportingService(ITransactionService transactionService, IScheduledService scheduledService)
    {
        _transactionService = transactionService;
        _scheduledService = scheduledService;
    }
    public async Task<byte[]> GeneratePdfAsync(int accountId)
    {
        using (var ms = new MemoryStream())
        {
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var writer = PdfWriter.GetInstance(document, ms);
            document.Open();

            var font = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);

            var title = new Paragraph("Monthly summary", titleFont);
            title.Alignment = Element.ALIGN_CENTER;
            document.Add(title);

            document.Add(new Paragraph("\n"));

            PdfPTable table = new PdfPTable(4);
            table.WidthPercentage = 100;

            table.AddCell(GetCell("Incomes", font, true));
            table.AddCell(GetCell("Expenses", font, true));
            table.AddCell(GetCell("Scheduled incomes", font, true));
            table.AddCell(GetCell("Scheduled expenses", font, true));

            var ListOfTransactions = await _transactionService.GetAllTransactionsOfAccount(accountId);
            List<Scheduled> ListOfScheduledTransactions = await _scheduledService.GetAllScheduledOfAccount(accountId);
            List<Transaction> incomeTransactions = [];
            List<Transaction> expenseTransactions = [];
            List<Scheduled> scheduledIncomeTransactions = [];
            List<Scheduled> scheduledExpenseTransactions = [];

            SortTransactionsToExpenseOrIncome(ListOfTransactions, incomeTransactions, expenseTransactions);
            SortScheduledTransactions(ListOfScheduledTransactions, scheduledIncomeTransactions, scheduledExpenseTransactions);
            var maxLengthTransactions = Math.Max(incomeTransactions.Count, expenseTransactions.Count);
            var maxLengthScheduled = Math.Max(scheduledExpenseTransactions.Count, scheduledIncomeTransactions.Count);
            var maxLength = Math.Max(maxLengthScheduled, maxLengthTransactions);
            for (int i = 0; i < maxLength; i++)
            {
                if (i < incomeTransactions.Count)
                {
                    table.AddCell(GetCell(incomeTransactions[i].Name + " Amount: " + incomeTransactions[i].Amount, font));
                }
                else
                {
                    table.AddCell(GetCell("", font));
                }
                if (i < expenseTransactions.Count())
                {
                    table.AddCell(GetCell(expenseTransactions[i].Name + " Amount: " + expenseTransactions[i].Amount, font));
                }
                else
                {
                    table.AddCell(GetCell("", font));
                }
                if (i < scheduledIncomeTransactions.Count())
                {
                    table.AddCell(GetCell(scheduledIncomeTransactions[i].Name + " Amount: " + scheduledIncomeTransactions[i].Amount, font));
                }
                else
                {
                    table.AddCell(GetCell("", font));
                }
                if (i < scheduledExpenseTransactions.Count())
                {
                    table.AddCell(GetCell(scheduledExpenseTransactions[i].Name + " Amount: " + scheduledExpenseTransactions[i].Amount, font));
                }
                else
                {
                    table.AddCell(GetCell("", font));
                }
            }
            table.AddCell(GetCell("Sum: " + await _transactionService.GetSumOfIncomesForAMonth(accountId), font));
            table.AddCell(GetCell("Sum: " + await _transactionService.GetSumOfExpensesForAMonth(accountId), font));
            table.AddCell(GetCell("Sum: " + await _scheduledService.GetSumOfIncomesForAMonth(accountId), font));
            table.AddCell(GetCell("Sum: " + await _scheduledService.GetSumOfExpensesForAMonth(accountId), font));


            document.Add(table);

            document.Close();
            writer.Close();

            return ms.ToArray();
        }
    }

    private static void SortScheduledTransactions(List<Scheduled> ListOfScheduledTransactions, List<Scheduled> scheduledIncomeTransactions, List<Scheduled> scheduledExpenseTransactions)
    {
        foreach (var scheduledTransaction in ListOfScheduledTransactions)
        {
            if (scheduledTransaction.Indicator == IndicatorIds.Income)
            {
                scheduledIncomeTransactions.Add(scheduledTransaction);
            }
            else
            {
                scheduledExpenseTransactions.Add(scheduledTransaction);
            }
        }
    }

    private static void SortTransactionsToExpenseOrIncome(List<Transaction> ListOfTransactions, List<Transaction> incomeTransactions, List<Transaction> expenseTransactions)
    {
        foreach (var transaction in ListOfTransactions)
        {
            if (transaction.Indicator == IndicatorIds.Income)
            {
                incomeTransactions.Add(transaction);
            }
            else
            {
                expenseTransactions.Add(transaction);
            }
        }
    }

    private PdfPCell GetCell(string text, Font font, bool isHeader = false)
    {
        PdfPCell cell = new PdfPCell(new Phrase(text, font));
        if (isHeader)
        {
            cell.BackgroundColor = BaseColor.LIGHT_GRAY;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        }
        else
        {
            cell.HorizontalAlignment = Element.ALIGN_LEFT;
            cell.VerticalAlignment = Element.ALIGN_MIDDLE;
        }
        cell.Padding = 5;

        return cell;
    }
}
