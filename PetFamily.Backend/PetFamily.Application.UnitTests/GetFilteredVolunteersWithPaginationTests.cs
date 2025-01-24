using System.Data;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using PetFamily.Application.DTOs;
using PetFamily.Application.DTOs.Read;
using PetFamily.Application.DTOs.Volunteer;
using PetFamily.Application.Interfaces.Database;
using PetFamily.Application.VolunteerAggregate.Queries.GetFilteredVolunteersWithPagination;

namespace PetFamily.Application.UnitTests;

public class GetFilteredVolunteersWithPaginationTests
{
    private readonly Mock<IDbConnection> _dbConnectionMock = new();
    private readonly Mock<IDbCommand> _dbCommandMock = new();
    private readonly Mock<IDataReader> _dbDataReaderMock = new();
    private readonly Mock<ISqlConnectionFactory> _sqlConnectionFactoryMock = new();
    private readonly Mock<IValidator<GetFilteredVolunteersWithPaginationQuery>> _validatorMock = new();
    private readonly Mock<ILogger<GetFilteredVolunteersWithPaginationHandlerDapper>> _loggerMock = new();

    [Fact]
    public async Task Handle_ShouldGetFilteredVolunteersWithPagination()
    {
        //Arrange
        var cancellationToken = new CancellationTokenSource().Token;

        const string CONNECTION_STRING =
            "Server=localhost;Port=5432;Database=pet_family;UserId=postgres;Password=postgres;";
        const int PAGE_NUMBER = 1;
        const int PAGE_SIZE = 3;
        const string SORT_DIRECTION = "ASC";
        const string SORT_BY = "work_experience"; //"id", "first_name", "last_name", "patronymic", "work_experience"

        var query = new GetFilteredVolunteersWithPaginationQuery(
            PAGE_NUMBER,
            PAGE_SIZE,
            null,
            null,
            null,
            null,
            SORT_DIRECTION,
            SORT_BY);

        _dbConnectionMock
            .Setup(c => c.CreateCommand())
            .Returns(() =>
            {
                _dbCommandMock
                    .Setup(cmd => cmd.ExecuteReader())
                    .Returns(() =>
                    {
                        var volunteers = new List<VolunteerDto>
                        {
                            new VolunteerDto
                            {
                                Id = Guid.NewGuid(),
                                Description = "description",
                                Email = "email",
                                FirstName = "first_name",
                                LastName = "last_name",
                                Patronymic = "patronymic",
                                PhoneNumber = "phone_number",
                                Requisites =
                                [
                                    new RequisiteDto("title", "description"),
                                    new RequisiteDto("email", "email")
                                ],
                                SocialNetworks =
                                [
                                    new SocialNetworkDto("title", "Url"),
                                    new SocialNetworkDto("email", "Url"),
                                    new SocialNetworkDto("email", "Url")
                                ],
                                IsDeleted = false
                            }
                        };

                        var enumerator = volunteers.GetEnumerator();

                        _dbDataReaderMock
                            .SetupSequence(reader => reader.Read())
                            .Returns(() => enumerator.MoveNext())
                            .Returns(false);

                        // _dbDataReaderMock.Setup(reader => reader["Id"]).Returns(() => enumerator.Current.Id);
                        // _dbDataReaderMock.Setup(reader => reader["FirstName"]).Returns(() => enumerator.Current.FirstName);
                        // _dbDataReaderMock.Setup(reader => reader["LastName"]).Returns(() => enumerator.Current.LastName);
                        // _dbDataReaderMock.Setup(reader => reader["PhoneNumber"]).Returns(() => enumerator.Current.PhoneNumber);

                        return _dbDataReaderMock.Object;
                    });

                return _dbCommandMock.Object;
            });

        //sqlConnectionFactory
        _sqlConnectionFactoryMock
            .Setup(c => c.GetConnection())
            .Returns(_dbConnectionMock.Object);

        //validator
        _validatorMock
            .Setup(v => v.ValidateAsync(query, cancellationToken))
            .ReturnsAsync(new ValidationResult());

        //createHandler
        var handler = new GetFilteredVolunteersWithPaginationHandlerDapper(
            _sqlConnectionFactoryMock.Object,
            _validatorMock.Object,
            _loggerMock.Object);

        //Act
        var result = await handler.HandleAsync(query, cancellationToken);

        //Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count.Should().Be(3);
        result.Value.HasNextPage.Should().BeFalse();
        result.Value.HasPreviousPage.Should().BeFalse();
        result.Value.Items[0].Email.Contains("@").Should().BeTrue();
        result.Value.Items[0].PhoneNumber.Contains("89").Should().BeTrue();
        // result.Value.Items[0].Requisites[0].Title.Should().Be("string");
        // result.Value.Items[0].Requisites[0].Description.Should().Be("string");
        // result.Value.Items[0].SocialNetworks[0].Title.Should().Be("string");
        // result.Value.Items[0].SocialNetworks[0].Url.Should().Be("string");
    }
}