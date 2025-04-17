namespace Web.KP.Models
{
    public class GridViewModel<T>
    {
        public IEnumerable<T> Items { get; set; }
        public string[] PropertiesToDisplay { get; set; }
        public string DeleteAction { get; set; }
        public string DeleteController { get; set; }
        public string EditAction { get; set; }
        public string EditController { get; set; }
        public string KeyProperty { get; set; }
    }
}
