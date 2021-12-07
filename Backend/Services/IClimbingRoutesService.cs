using Backend.InputModels;
using Backend.ViewModels;

using Microsoft.AspNetCore.Mvc;

namespace Backend.Services; 

public interface IClimbingRoutesService {
    public Task<List<ClimbingRouteViewModel>> GetAll();
    Task<ActionResult<long>> CreateRoute(ClimbingRoutePostInput input);
}