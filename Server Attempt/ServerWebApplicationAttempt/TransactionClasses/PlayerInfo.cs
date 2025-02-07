namespace ServerWebApplicationAttempt.TransactionClasses
{
    public record class PlayerInfo
    {
        public string name { get; set; }
        public string password { get; set; }
    }

    public record class PlayerInfoSec
    {
        public int Id { get; set; }
        public string name { get; set; }
        public string password { get; set; }
    }
}
