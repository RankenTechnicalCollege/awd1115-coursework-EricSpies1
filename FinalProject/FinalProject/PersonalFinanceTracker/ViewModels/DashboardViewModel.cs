using System.Collections.Generic;
using PersonalFinanceTracker.Models;

namespace PersonalFinanceTracker.ViewModels
{
    public class DashboardViewModel
    {
        public string ActiveAccountId { get; set; } = "all";
        public string ActiveCategoryId { get; set; } = "all";

        public List<Account> Accounts { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<Transaction> RecentTransactions { get; set; } = new();

        public decimal MonthToDateSpending { get; set; }
        public decimal MonthToDateDeposits { get; set; }
        public int TransactionCount { get; set; }
    }
}
