using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
	[RoutePrefix("api/camps")]
    public class CampsController : ApiController
    {
		private readonly ICampRepository _repository;
		private readonly IMapper _mapper;

		//The reason we can have the IMapper as a parameter is because we created and registered a map between Camp and CampModel using
		//AutoMapper, by creating a CampModel in the Models Folder, create the CampMappingProfile which draws the correlations in the mapping, and registering
		//that CampMappingProfile in the AutofacConfig.cs
		public CampsController(ICampRepository repository, IMapper mapper)
		{
			_repository = repository;
			_mapper = mapper;
		}

		[Route()]								//passsing this parameter to the get method will allow an optional query string to find Talks
		public async Task<IHttpActionResult> Get(bool includeTalks = false)
		{
			try
			{
				//We have created a CampModel(what the end user sees, we took out a couple properties) of the Camp shape(what is in the database), which
				//is exposed to the user through the API.
				//This section is where we map the CampModel to the Camp
				//To map we could do a bunch of foreach loops, however here we are using AutoMapper NuGet Package (Right-click-> manage NuGet Packages)

				//Mapping
				var result = await _repository.GetAllCampsAsync(includeTalks);

				var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);
				return Ok(mappedResult);
			}
			catch
			{
				// TODO Add Logging
				return InternalServerError();
			}
		}
		//This action We are allowing the user to get only an individual camp, here we use moniker inside the URL to specify which camp they are looking for
		[Route("{moniker}", Name ="GetCamp")]
		public async Task<IHttpActionResult> Get(string moniker)
		{
			try
			{
				//when doing async work, need to put the await keyword
				var result = await _repository.GetCampAsync(moniker);

				if (result == null) return NotFound();

				return Ok(_mapper.Map<CampModel>(result));
			}
			catch(Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		//adding eventDate to the URL is kind of redundant so if we make the searchByDate part of the route and add the constraint of it being a date the URL will be simpler
		//instead of being api/camps/searchByDate?evenDate=2018-10-19 it will be api/camps/searchByDate/2018-10-19
		[Route("searchByDate/{eventDate:datetime}")]
		//this won't work until you add the attribute below, because SearchByEventDate is not API Verb, Get, Push, Put, Delete
		//adding the attribute HttpGet below will help clarify this so we are actually matching the route and action to the SearchByEventDate method
		[HttpGet]
		public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
		{
			try
			{
				var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);
				return Ok(_mapper.Map<CampModel[]>(result));
			}
			catch(Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[Route()]
		public async Task<IHttpActionResult> Post(CampModel model)
		{
			try
			{
				if (await _repository.GetCampAsync(model.Moniker) != null)
				{
					ModelState.AddModelError("Moniker", "Moniker in use");
				}

				if (ModelState.IsValid)
				{
					var camp = _mapper.Map<Camp>(model);
					_repository.AddCamp(camp);

					if (await _repository.SaveChangesAsync())
					{
						var newModel = _mapper.Map<CampModel>(camp);
						
						return CreatedAtRoute("GetCamp", new { moniker = newModel.Moniker }, newModel);
					}
				}
			}
			catch(Exception ex)
			{
				return InternalServerError(ex);
			}

			return BadRequest(ModelState);
		}

		[Route("moniker")]
		public async Task<IHttpActionResult> Put(string moniker, CampModel model)
		{
			try
			{
				var camp = await _repository.GetCampAsync(moniker);
				if (camp == null) return NotFound();

				//Map from model to camp.  first property is source second is the destination.  this is diff from previous implementations where we go from camp to model
				_mapper.Map(model, camp);

				if (await _repository.SaveChangesAsync())
				{
					return Ok(_mapper.Map<CampModel>(camp));
				}
				else
				{
					return InternalServerError();
				}

			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}

		[Route("moniker")]					//if a route is included you need to include it in the action declaration
		public async Task<IHttpActionResult> Delete(string moniker)
		{
			try
			{
				var camp = await _repository.GetCampAsync(moniker);
				if (camp == null) return NotFound();

				_repository.DeleteCamp(camp);

				if (await _repository.SaveChangesAsync())
				{
					return Ok();
				}
				else
				{
					return InternalServerError();
				}
			}
			catch (Exception ex)
			{
				return InternalServerError(ex);
			}
		}
    }
}
