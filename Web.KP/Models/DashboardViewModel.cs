namespace Web.KP.Models
{
    using System.ComponentModel.DataAnnotations;

    public class DashboardViewModel
    {
        public int Users { get; set; }
        public int Groups { get; set; }
        public int Permissions { get; set; }
    }
}
