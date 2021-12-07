using Backend.Models;

namespace Backend.ViewModels; 

public class TagViewModel {
    public long Id { get; set; }
    
    public string Value { get; set; } = string.Empty;
    
    public TagViewModel() {}

    public TagViewModel(Tag tag) {
        Id = tag.Id;
        Value = tag.Value;
    }
}