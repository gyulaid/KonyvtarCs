using LibraryApi.Lending.Dto;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Lending;

[ApiController]
[Route("/lendings")]
public class LendingController : ControllerBase
{
    private readonly LendingService lendingService;

    public LendingController(LendingService lendingService)
    {
        this.lendingService = lendingService;
    }

    [HttpGet]
    public List<LendingResponseDto> GetAllLendingsByMember([FromQuery(Name = "memberId")] int? memberId)
    {
        if (memberId == null)
        {
            return this.lendingService.GetAllLendings();
        }

        return this.lendingService.GetLendingsByMemberId(memberId);
    }

    [HttpGet("/active")]
    public List<LendingResponseDto> GetAllActiveLendingsByMember([FromQuery(Name = "memberId")] int memberId)
    {
        return this.lendingService.GetActiveLendingsByMemberId(memberId);
    }

    [HttpGet("{id}")]
    public LendingResponseDto GetLending(int id)
    {
        return this.lendingService.GetLendingById(id);
    }

    [HttpPost]
    public LendingResponseDto CreateLending([FromBody] CreateLendingDto createDto)
    {
        return this.lendingService.CreateLending(createDto);
    }

    [HttpPatch("{id}")]
    public LendingResponseDto ReturnLending(int id, [FromBody] UpdateLendingDto updateDto)
    {
        return this.lendingService.ReturnLending(id, updateDto);
    }

    [HttpDelete("{id}")]
    public void DeleteLending(int id)
    {
        this.lendingService.DeleteLending(id);
    }
}