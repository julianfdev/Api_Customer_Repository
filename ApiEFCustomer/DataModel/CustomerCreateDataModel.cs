namespace EFCoreInMemory.DataModel
{
    public class CustomerCreateDataModel
    {
        public string Name { get; set; } = string.Empty;
        public string? DNI { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Mobile { get; set; }
        public string? Email { get; set; }
        public string? State { get; set; }
        public string? City { get; set; }
    }
}
