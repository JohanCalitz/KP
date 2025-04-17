namespace Web.Models
{
    public class UserAllInformationModel
    {
        public UserModel User { get; set; } 
        public List<GroupModel> Groups { get; set; } = new List<GroupModel>();
    }
}
