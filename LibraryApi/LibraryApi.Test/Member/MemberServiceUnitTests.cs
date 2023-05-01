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

    private readonly LibraryContext libraryContext;
    private readonly Mock<ILogger<MemberService>> mockedLogger;
    private readonly Mock<IMapper> mockedMapper;
    private readonly MemberService memberService;
    private readonly DbContextOptions<LibraryContext> _options;

    private static readonly LibraryApi.Member.Member _member = new()
    {
        Id = TestId, Address = "address", DateOfBirth = DateTime.Parse("2000-11-16"), Name = "name"
    };
    

    public MemberServiceUnitTests()
    {
        _options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        libraryContext = new LibraryContext(_options);
        mockedLogger = new Mock<ILogger<MemberService>>();
        mockedMapper = new Mock<IMapper>();
        memberService = new MemberService(libraryContext, mockedLogger.Object, mockedMapper.Object);
    }
    
    private void CleanUp()
    {
        libraryContext.Database.EnsureDeleted();
        libraryContext.Database.EnsureCreated();
    }

    [Fact]
    public void CreateMemberShouldSucceed()
    {
        // given
        var createDto = new CreateMemberDto()
        {
            Address = _member.Address,
            DateOfBirth = _member.DateOfBirth,
            Name = _member.Name
        };

        mockedMapper.Setup(mapper => mapper.Map<LibraryApi.Member.Member>(createDto)).Returns(_member);

        // when
        memberService.CreateMember(createDto);
        
        // then
        Assert.Single(libraryContext.Members.ToList());
        
        CleanUp();
    }
    
    [Theory]
    [InlineData("Dominik123")]
    [InlineData("  ")]
    [InlineData("Dominik*!")]
    public void CreateMemberShouldThrowArgumentExceptionIfNameContainsWrongCharacters(string name)
    {
        // given
        var createDto = new CreateMemberDto()
        {
            Address = _member.Address,
            DateOfBirth = _member.DateOfBirth,
            Name = name
        };

        mockedMapper.Setup(mapper => mapper.Map<LibraryApi.Member.Member>(createDto)).Returns(_member);

        // when - then
        Assert.Throws<ArgumentException>(() => memberService.CreateMember(createDto));
        CleanUp();
    }
}