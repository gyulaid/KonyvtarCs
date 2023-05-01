using System;
using AutoMapper;
using LibraryApi.Database;
using LibraryApi.Exception;
using LibraryApi.Lending;
using LibraryApi.Lending.Dto;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace LibraryApi.Tests.Lending;

[Collection("TestDatabaseCollection")]
public class LendingServiceUnitTests
{
    private const int TestId = 1;

    private readonly LibraryContext libraryContext;
    private readonly Mock<ILogger<LendingService>> mockedLogger;
    private readonly Mock<IMapper> mockedMapper;
    private readonly LendingService lendingService;
    private readonly DbContextOptions<LibraryContext> _options;

    private static LibraryApi.Member.Member _member = new()
    {
        Id = TestId, Address = "address", DateOfBirth = DateTime.Parse("2000-11-16"), Name = "name"
    };

    private static LibraryApi.Book.Book _book = new()
    {
        Id = TestId, Author = "author", IsAvailable = true, Publisher = "publisher", PublishYear = 2000, Title = "title"
    };

    private static LibraryApi.Lending.Lending _lending = new LibraryApi.Lending.Lending()
    {
        Id = TestId, Member = _member, Book = _book, DateOfLend = DateTime.Today,
        DeadlineOfReturn = DateTime.Today.Add(TimeSpan.FromDays(10))
    };


    public LendingServiceUnitTests()
    {
        _options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;
        
        libraryContext = new LibraryContext(_options);
        mockedLogger = new Mock<ILogger<LendingService>>();
        mockedMapper = new Mock<IMapper>();
        lendingService = new LendingService(libraryContext, mockedLogger.Object, mockedMapper.Object);
    }
    
    private void CleanUp()
    {
        libraryContext.Database.EnsureDeleted();
        libraryContext.Database.EnsureCreated();
    } 

    [Fact]
    public void CreateLendingShouldSucceed()
    {
        // given
        libraryContext.Books.Add(_book);
        libraryContext.Members.Add(_member);
        libraryContext.SaveChanges();

        var createDto = new CreateLendingDto();
        createDto.DateOfLend = DateTime.Now;
        createDto.DeadlineOfReturn = DateTime.Today.Add(TimeSpan.FromDays(10));
        createDto.BookId = TestId;
        createDto.MemberId = TestId;

        var responseDto = new LendingResponseDto();

        mockedMapper.Setup(mapper => mapper.Map<LibraryApi.Lending.Lending>(createDto)).Returns(_lending);
        mockedMapper.Setup(mapper => mapper.Map<LendingResponseDto>(_lending)).Returns(responseDto);

        // when
        var actual = lendingService.CreateLending(createDto);

        // then
        Assert.Equal(responseDto, actual);
        
        CleanUp();
    }

    [Fact]
    public void CreateLendingShouldThrowInvalidDateExceptionWhenDeadlineIsEarlierThanLendDate()
    {
        // given
        var createDto = new CreateLendingDto();
        createDto.DateOfLend = DateTime.Today.Add(TimeSpan.FromDays(10));
        createDto.DeadlineOfReturn = DateTime.Today;
        createDto.BookId = TestId;
        createDto.MemberId = TestId;

        // when - then
        Assert.Throws<InvalidDateException>(() => lendingService.CreateLending(createDto));
    }

    [Fact]
    public void CreateLendingShouldThrowNotAvailableExceptionWhenNoBookWasFoundAvailableWithBookId()
    {
        // given
        var createDto = new CreateLendingDto();
        createDto.DateOfLend = DateTime.Today;
        createDto.DeadlineOfReturn = DateTime.Today.Add(TimeSpan.FromDays(10));
        createDto.BookId = TestId;
        createDto.MemberId = TestId;

        // when - then
        Assert.Throws<NotAvailableException>(() => lendingService.CreateLending(createDto));
    }

    [Fact]
    public void ReturnLendingShouldSucceedAndUpdateBookToAvailable()
    {
        // given
        _book.IsAvailable = false;
        libraryContext.Books.Add(_book);
        libraryContext.Members.Add(_member);
        libraryContext.Lendings.Add(_lending);
        libraryContext.SaveChanges();

        // when
        lendingService.ReturnLending(
            _lending.Id,
            new UpdateLendingDto() { dateOfReturn = _lending.DateOfLend.Add(TimeSpan.FromDays(1)) }
        );

        // then
        Assert.NotNull(libraryContext.Lendings.Find(_lending.Id)!.DateOfReturn);
        Assert.True(libraryContext.Books.Find(_lending.Book.Id)!.IsAvailable);
        
        CleanUp();
    }

    [Fact]
    public void ReturnLendingShouldThrowInvalidDateExceptionIfReturnDateIsEarlierThanLendDate()
    {
        // given
        libraryContext.Books.Add(_book);
        libraryContext.Members.Add(_member);
        libraryContext.Lendings.Add(_lending);
        libraryContext.SaveChanges();

        // when - then
        Assert.Throws<InvalidDateException>(() => lendingService.ReturnLending(
                _lending.Id,
                new UpdateLendingDto() { dateOfReturn = _lending.DateOfLend.Subtract(TimeSpan.FromDays(1)) }
            )
        );
        
        CleanUp();
    }
}