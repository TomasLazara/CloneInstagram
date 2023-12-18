namespace InstagramClone.API.DTOs
{
    public class UserProfileDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public DateTime CreationDate { get; set; }
        public int Followers { get; set; }

    }
}
