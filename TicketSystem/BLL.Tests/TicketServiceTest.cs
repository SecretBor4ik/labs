using User = CCL.Security.Identity.User;
using BLL.Services.Impl;
using BLL.Services.Interfaces;
using CCL.Security;
using CCL.Security.Identity;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using DAL.UnitOfWork;
using Moq;

namespace BLL.Tests
{
    public class TicketServiceTest
    {
        [Fact]
        public void Ctor_InputNull_ThrowArgumentNullException()
        {
            // Arrange
            IUnitOfWork nullUnitOfWork = null;

            // Act

            // Assert
            Assert.Throws<ArgumentNullException>(
                () => new TicketService(nullUnitOfWork));
        }

        [Fact]
        public void GetTickets_UserIsClient_ThrowMethodAccessException()
        {
            // Arrange
            var user = new User(1, Role.Client);
            SecurityContext.SetUser(user);

            var mockUnitOfWork = new Mock<IUnitOfWork>();
            ITicketService ticketService = new TicketService(mockUnitOfWork.Object);

            // Act
            var actualGetTicketsFunc = () => ticketService.GetTickets(0);
            var exception = Record.Exception(actualGetTicketsFunc);

            // Assert
            Assert.IsNotType<MethodAccessException>(exception);
        }

        [Fact]
        public void GetTickets_TicketFromDAL_CorrectMappingToTicketDTO()
        {
            // Arrange
            User user = new User(1, Role.Client);
            SecurityContext.SetUser(user);
            var ticketService = GetTicketService();

            // Act
            var actualTicketDTO = ticketService.GetTickets(0).First();

            // Assert
            Assert.True(
                actualTicketDTO.Id == 1
                && actualTicketDTO.Date == DateTime.MinValue
                && actualTicketDTO.BusId == 1
                && actualTicketDTO.UserId == 1
            );
        }
        ITicketService GetTicketService()
        {
            var mockContext = new Mock<IUnitOfWork>();
            var expectedTicket = new Ticket()
            {
                Id = 1,
                Date = DateTime.MinValue,
                BusId = 1,
                UserId = 1
            };
            var mockDbSet = new Mock<ITicketRepository>();
            mockDbSet.Setup(z => z.Find(It.IsAny<Func<Ticket, bool>>(),
                It.IsAny<int>(),
                It.IsAny<int>()))
                .Returns(new List<Ticket>() { expectedTicket });
            mockContext.Setup(context => context.Tickets)
                .Returns(mockDbSet.Object);
            ITicketService ticketService = new TicketService(mockContext.Object);
            return ticketService;
        }
    }
}