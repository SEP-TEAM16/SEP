using SEP.Common.Enums;
using SEP.Common.Models;

namespace SEP.Bank2.DTO
{
    public class CardDTO
    {
        public string Id { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Number { get; set; }
        public string SecurityCode { get; set; }

        public CardDTO() { }

        public CardDTO(string id, string month, string year, string number, string securityCode)
        {
            Id = id;
            Month = month;
            Year = year;
            Number = number;
            SecurityCode = securityCode;
        }
    }
}
