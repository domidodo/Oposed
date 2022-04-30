using Microsoft.AspNetCore.Mvc;
using OposedApi.Attributes;
using OposedApi.Enum;
using OposedApi.Error;
using OposedApi.Utilities;
using Swashbuckle.AspNetCore.Annotations;

namespace OposedApi.Controllers
{
    public abstract class BasicConrollerFunctions<T> : ControllerBase
    {
        [HttpGet]
        [Auth(Role = UserRole.User)]
        [SwaggerOperation(Summary = "Get all")]
        public ActionResult<List<T>> GetAll()
        {
            return BasicUtilityFunctions<T>.GetAll();
        }

        [HttpGet]
        [Auth(Role = UserRole.User)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Get by id")]
        public ActionResult<T> Get(int id)
        {
            T t = BasicUtilityFunctions<T>.GetById(id);
            if (t != null)
                return t;
            else
                return ErrorManager.Get(Errors.ROOM_NOT_FOUND);
        }

        [HttpPut]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Update")]
        public ActionResult Update(T t)
        {
            var successful = BasicUtilityFunctions<T>.Update(t);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.ROOM_UPDATING_FAILED);
        }


        [HttpPost]
        [Auth(Role = UserRole.Admin)]
        [SwaggerOperation(Summary = "Add")]
        public ActionResult<int> Add(T t)
        {
            return BasicUtilityFunctions<T>.Save(t);
        }


        [HttpDelete]
        [Auth(Role = UserRole.Admin)]
        [Route("{id}")]
        [SwaggerOperation(Summary = "Delete by id")]
        public ActionResult Delete(int id)
        {
            var successful = BasicUtilityFunctions<T>.DeleteById(id);
            if (successful)
                return Ok();
            else
                return ErrorManager.Get(Errors.ROOM_DELETING_FAILED);
        }
    }
}
