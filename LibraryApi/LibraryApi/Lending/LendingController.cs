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
    public List<Lending> GetAllLendings()
    {
        return this.lendingService.GetAllLendings();
    }

    [HttpGet("{id}")]
    public Lending GetLending(int id)
    {
        return this.lendingService.GetLendingById(id);
    }

    [HttpPost]
    public Lending CreateLending([FromBody] Lending lending)
    {
        return this.lendingService.CreateLending(lending);
    }

    [HttpPatch("{id}")]
    public Lending ReturnLending(int id, [FromBody] DateTime dateOfReturn)
    {
        return this.lendingService.ReturnLending(id, dateOfReturn);
    }

    [HttpDelete("{id}")]
    public void DeleteLending(int id)
    {
        this.lendingService.DeleteLending(id);
    }
}