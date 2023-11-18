
namespace TicketSystem_Api.Controllers
{
    [Route("api/Tickets")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly APIResponse _response;
        private readonly IMapper _mapper;
        public TicketsController(ITicketRepository ticketRepository, IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _response = new APIResponse();
            _mapper = mapper;
        }

        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetTickets(int page = 1, int pageSize = 5)
        {
            try
            {
                var tickets = await _ticketRepository.GetTicketsAsync(page, pageSize);
                return Ok(tickets);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }
        [HttpGet("GetTicketsCount")]
        public async Task<ActionResult<APIResponse>> GetTicketsCount()
        {
            try
            {
                var ticketsCount = await _ticketRepository.GetTicketsCount();
                return Ok(ticketsCount);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

        [HttpGet("GetTicketById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> GetById (int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Invalid Id." };
                    return BadRequest(_response);
                }
                var ticket = await _ticketRepository.GetTicketByIdAsync(id);
                if (ticket == null)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Ticket Not Found" };
                    return NotFound(_response);
                }
                return Ok(ticket);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }


        [HttpPost("CreateTicket")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateTicket([FromBody]TicketDto model)
        {
            if (model == null)
            {
                return BadRequest("Invalid ticket data");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!Regex.IsMatch(model.PhoneNumber, @"^[0-9\s().-]+$"))
            {
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { "Invalid Phone Number. Only digits, and dashes are allowed." };
                return BadRequest(_response);
            }
            var _ticket = _mapper.Map<Ticket>(model);
            await _ticketRepository.CreateTicketAsync(_ticket);
            _response.Result = _mapper.Map<TicketDto>(_ticket);
            _response.StatusCode = HttpStatusCode.Created;
            _response.ErrorMessages = new List<string>() { "Ticket Created successfully." };
            return _response;
        }


        [HttpPut("IsHandled")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> IsHandled(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.NoContent;
                    _response.IsSuccess = false;
                    _response.ErrorMessages = new List<string>() { "Invalid Id." };
                    return BadRequest(_response);
                }
                var _status = await _ticketRepository.GetTicketByIdAsync(id);
                
                //TO DO: New Change Status

                _status.IsHandled = !_status.IsHandled;
                _response.Result = _mapper.Map<TicketChangeStatusDto>(_status);
                await _ticketRepository.UpdateHandhandledAsync(_status);
                _response.ErrorMessages = new List<string>() { "Status Updated" };
                _response.StatusCode = HttpStatusCode.NoContent;
                _response.IsSuccess = true;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessages = new List<string>() { ex.ToString() };
            }
            return _response;
        }

    }
}
