namespace Backend.Services; 

public interface IStorageService {
    public Task<string> UploadPictureBase64(string pictureBase64);
}