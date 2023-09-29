using IvanPurebaAPI.Datos;
using IvanPurebaAPI.Models;
using IvanPurebaAPI.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IvanPurebaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PruebaController : ControllerBase
    {

        private readonly ILogger <PruebaController> _logger; // injectandi el servicio logger

        public PruebaController(ILogger<PruebaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<PruebaDTO>> GetPruebas()
        {
            _logger.LogInformation("Obetener registros");
            return PruebaStore.pruebalist;

        }

        [HttpGet("id:int", Name = "GetPrueba")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PruebaDTO> GetPrueba(int id)
        {

            if (id == 0)
            {
                return BadRequest();
            }
            var prueba = PruebaStore.pruebalist.FirstOrDefault(p => p.Id == id);
            if (prueba == null)
            {
                _logger.LogError("Error al enviar el id" + id);
                return NotFound();
            }
            return Ok(prueba);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]             //--------------
        [ProducesResponseType(StatusCodes.Status400BadRequest)]          //Estados de Respuesta
        [ProducesResponseType(StatusCodes.Status500InternalServerError)] //--------------

        public ActionResult<PruebaDTO> Crear([FromBody] PruebaDTO pruebaDTO)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (PruebaStore.pruebalist.FirstOrDefault(v => v.Nombre.ToLower() == pruebaDTO.Nombre.ToLower()) != null)//validacion perzonalizada
            {
                ModelState.AddModelError("NombreExiste", "El nombre ya existe");//Agrega un nuevo estado 
                return BadRequest(ModelState);// retorno el nuevo estado con un BadRequest
            }
            if (pruebaDTO == null)
            {
                return BadRequest();
            }
            if (pruebaDTO.Id > 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            pruebaDTO.Id = PruebaStore.pruebalist.OrderByDescending(p => p.Id).FirstOrDefault().Id + 1;
            PruebaStore.pruebalist.Add(pruebaDTO);// agregfa un nuevo registro

            return CreatedAtRoute("GetPrueba", new { id = pruebaDTO.Id }, pruebaDTO);// retorna al GET que buca por id para mostrar el nuevo registro
        }

        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult DeletePrueba(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var registro = PruebaStore.pruebalist.FirstOrDefault(v => v.Id == id);

            if (registro != null)
            {
                return NotFound();
            }
            PruebaStore.pruebalist.Remove(registro);
            return NoContent();
        }

        [HttpPut("id:int")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult Actualizar(int id,[FromBody] PruebaDTO pruebaDTO)
        {
            if (pruebaDTO==null || id!=pruebaDTO.Id)
            {
                return BadRequest();
            }
            var registro = PruebaStore.pruebalist.FirstOrDefault(r=>r.Id == id);
            registro.Nombre = pruebaDTO.Nombre;
            return NoContent();
        }
        
    }
}
