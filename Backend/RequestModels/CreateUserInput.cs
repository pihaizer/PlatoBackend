namespace Backend.InputModels; 

public class CreateUserInput {
    public string FirebaseId { get; set; }
    
    public string FirstName { get; set; }
    
    public string LastName { get; set; }
    
    public string? Nickname { get; set; }
    
    public byte Sex { get; set; }
    
    public short? StartDate { get; set; }
}