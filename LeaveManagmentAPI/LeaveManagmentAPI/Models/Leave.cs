namespace LeaveManagmentAPI.Models
{
    public class Leave
    {
        public int Id { get; set; }
        public int UserId { get; set; }// Foreign key to User
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; }

        // Other leave request properties if any are needed

        public User User { get; set; } // Navigation property to User works like relationships in DBs
    }
}
