using DAlertsApi.Models.Pagination;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.ApiV1
{
    public class Donations
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Message_type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public int Amount { get; set; } 
        public string Currency { get; set; } = string.Empty;
        public int Is_shown { get; set; } 
        public string Created_at { get; set; } = string.Empty;
        public string Shown_at { get; set; } = string.Empty;

        public override string ToString()
        {
            return "Donations:\n" +
                   "Id: " + Id + "\n" +
                   "Name: " + Name + "\n" +
                   "Username: " + Username + "\n" +
                   "Message_type: " + Message_type + "\n" +
                   "Message: " + Message + "\n" +
                   "Amount: " + Amount + "\n" +
                   "Currency: " + Currency + "\n" +
                   "Is_shown: " + Is_shown + "\n" +
                   "Created_at: " + Created_at + "\n" +
                   "Shown_at: " + Shown_at + "\n";
        }
    }

    public class DonationsWrap
    {
        public List<Donations> Data { get; set; } = new List<Donations>();
        public PaginationInfo Links { get; set; } = new PaginationInfo();
        public PaginationInfo Meta { get; set; } = new PaginationInfo();
    }
}
