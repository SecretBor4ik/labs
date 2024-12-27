using AutoMapper;
using BLL.DTO;
using BLL.Services.Interfaces;
using CCL.Security;
using CCL.Security.Identity;
using DAL.Entities;
using DAL.UnitOfWork;

namespace BLL.Services.Impl
{
    internal class TicketService 
        : ITicketService
    {
        private readonly IUnitOfWork _datebase;
        private int pageSize = 10;

        public TicketService(IUnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
            _datebase = unitOfWork;
        }
        /// <exception cref="MethodAccessException"></exception>

        public IEnumerable<TicketDTO> GetTickets(int pageNumber)
        {
            var user = SecurityContext.GetUser();
            if (user is null || !user.Roles.Contains(Role.Client))
            {
                throw new MethodAccessException();
            }

            var userId = user.UserId;
            var ticketEntities = _datebase.Tickets.Find(o => o.UserId == userId, pageNumber, pageSize);
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Ticket, TicketDTO>()).CreateMapper();
            var ticketDTO = mapper.Map<IEnumerable<Ticket>, List<TicketDTO>>(ticketEntities);

            return ticketDTO;
        }
    }
}