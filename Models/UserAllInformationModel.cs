namespace API.Models
{
    public class UserAllInformationModel
    {
        public UserModel User { get; set; } // User details
        public List<GroupModel> Groups { get; set; } = new List<GroupModel>(); // Groups the user is part of, with permissions
    }
}
