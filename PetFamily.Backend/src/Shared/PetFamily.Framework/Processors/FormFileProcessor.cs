using Microsoft.AspNetCore.Http;
using PetFamily.Core.DTOs;

namespace PetFamily.Framework.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<UploadFileDto> _fileDtos = [];

    public List<UploadFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();

            var fileName = Path.ChangeExtension(
                Guid.NewGuid().ToString("N"), 
                Path.GetExtension(file.FileName));
            
            var fileDto = new UploadFileDto(stream, fileName);
            
            _fileDtos.Add(fileDto);
        }
        return _fileDtos;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _fileDtos)
        {
            await file.Stream.DisposeAsync();
        }
    }
}