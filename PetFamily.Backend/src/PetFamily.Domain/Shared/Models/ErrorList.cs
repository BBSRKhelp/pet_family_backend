using System.Collections;

namespace PetFamily.Domain.Shared.Models;

public class ErrorList : IEnumerable<Error>
{
    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = [..errors];
    }

    private readonly List<Error> _errors;

    public IEnumerator<Error> GetEnumerator()
    {
        return _errors.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public static implicit operator ErrorList(Error error) => new ErrorList([error]);
}