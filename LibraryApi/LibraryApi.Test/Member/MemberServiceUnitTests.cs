using System;
using System.Linq;
using AutoMapper;
using LibraryApi.Database;
using LibraryApi.Member;
using LibraryApi.Member.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryApi.Tests.Member;

public class MemberServiceUnitTests
{
    private const int TestId = 1;

    private readonly Mock<ILogger<MemberService>> mockedLogger;
    private readonly Mock<IMapper> mockedMapper;

    private static readonly LibraryApi.Member.Member _member = new()
    {
        Id = TestId, Address = "address", DateOfBirth = DateTime.Parse("2000-11-16"), Name = "name"
    };


    public MemberServiceUnitTests()
    {
        mockedLogger = new Mock<ILogger<MemberService>>();
        mockedMapper = new Mock<IMapper>();
    }

    private MemberService CreateService(LibraryContext libraryContext)
    {
        return new MemberService(libraryContext, mockedLogger.Object, mockedMapper.Object);
    }

    private static LibraryContext CreateContext()
    {
        DbContextOptions<LibraryContext> _options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new LibraryContext(_options);
    }

    private static void Teardown(LibraryContext _libraryContext)
    {
        _libraryContext.Database.EnsureDeleted();
    }

    [Theory]
    [InlineData("Dominik")]
    [InlineData("Gyulai Dominik")]
    public void CreateMemberShouldSucceed(string name)
    {
        // given
        var libraryContext = CreateContext();
        var memberService = CreateService(libraryContext);

        var createDto = new CreateMemberDto()
        {
            Address = _member.Address,
            DateOfBirth = _member.DateOfBirth,
            Name = name
        };

        _member.Name = name;

        mockedMapper.Setup(mapper => mapper.Map<LibraryApi.Member.Member>(createDto)).Returns(_member);

        // when
        memberService.CreateMember(createDto);

        // then
        Assert.Single(libraryContext.Members.ToList());

        Teardown(libraryContext);
    }

    [Theory]
    [InlineData("Dominik123")]
    [InlineData("  ")]
    [InlineData("Dominik*!")]
    [InlineData("Dominik  ")]
    public void CreateMemberShouldThrowArgumentExceptionIfNameContainsWrongCharacters(string name)
    {
        // given
        var libraryContext = CreateContext();
        var memberService = CreateService(libraryContext);

        var createDto = new CreateMemberDto()
        {
            Address = _member.Address,
            DateOfBirth = _member.DateOfBirth,
            Name = name
        };

        mockedMapper.Setup(mapper => mapper.Map<LibraryApi.Member.Member>(createDto)).Returns(_member);

        // when - then
        Assert.Throws<ArgumentException>(() => memberService.CreateMember(createDto));

        Teardown(libraryContext);
    }
}