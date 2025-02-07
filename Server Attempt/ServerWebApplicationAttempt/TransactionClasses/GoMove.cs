namespace ServerWebApplicationAttempt.TransactionClasses
{
    public record class GoMove
    {
        public int SideId { get; set; }
        public string move {  get; set; }
    }

    public record class ClientGameInfo      //information to be returned when starting a new game
    {
        public int GameId { get; set; }
        public int SideId { get; set; }
        public string Color { get; set; }
    }

}
