using PetFamily.Application.Dto;

namespace PetFamily.API.Processors;

public class FormFileProcessor : IAsyncDisposable
{
    private readonly List<CreateFileDto> _filesDto = [];

    public List<CreateFileDto> Process(IFormFileCollection files)
    {
        foreach (var file in files)
        {
            var stream = file.OpenReadStream();
            var fileDto = new CreateFileDto(stream, file.FileName);
            
            _filesDto.Add(fileDto);
        }
        return _filesDto;
    }
    
    public async ValueTask DisposeAsync()
    {
        foreach (var file in _filesDto)
        {
            await file.Stream.DisposeAsync();
        }
    }
}