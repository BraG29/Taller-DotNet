using Commercial_Office.Model;
using Commercial_Office.Hubs;
using Commercial_Office.Services;
using Moq;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace Taller_DotNet_Tests.Tests
{
    public class CommercialOfficeTests
    {
        
        private readonly OfficeService _officeService;
        private readonly Mock<IHubContext<CommercialOfficeHub>> _commercialOfficeHubMock;
        private readonly Mock<IOfficeRepository> _repositoryMock;
        private readonly Mock<ILogger<OfficeService>> _loggerMock;
        public CommercialOfficeTests()
        {

            _repositoryMock = new Mock<IOfficeRepository>();
            _loggerMock = new Mock<ILogger<OfficeService>>();

            _commercialOfficeHubMock = new Mock<IHubContext<CommercialOfficeHub>>();
            _officeService = new OfficeService(_repositoryMock.Object, _loggerMock.Object, _commercialOfficeHubMock.Object);


        }

        [Fact]
        public void RegisterUser_WhenOfficeNotFound()
        {
            // Simular que la oficina no existe
            _repositoryMock.Setup(repo => repo.GetOffice(It.IsAny<string>()))
                                 .Returns((Office)null);


            Assert.Throws<KeyNotFoundException>(() => _officeService.RegisterUser("validUserId", "invalidOfficeId"));

        }

        [Fact]
        public void RegisterUser_WhenUserIsNull()
        {
            // Id de usuario vacio.
            Assert.Throws<ArgumentNullException>(() => _officeService.RegisterUser(null, "invalidOfficeId"));

        }


        [Fact]
        public void RegisterUser_WhenOfficeFound()
        {

            ConcurrentQueue<string> _OfficeQueue1 = new ConcurrentQueue<string>();
            IList<AttentionPlace> attentionPlaces = new List<AttentionPlace>();
            AttentionPlace _AttentionPlace1 = new AttentionPlace(1, true);
            AttentionPlace _AttentionPlace2 = new AttentionPlace(2, true);
            attentionPlaces.Add(_AttentionPlace1);
            attentionPlaces.Add(_AttentionPlace2);
            Office office1 = new Office("OFI-1", _OfficeQueue1, attentionPlaces);


            // Simular que la oficina no existe
            _repositoryMock.Setup(repo => repo.GetOffice(office1.Identificator))
                                 .Returns((Office)office1);

            Debug.WriteLine("Hola desde el test");

            _officeService.RegisterUser("userPepe", office1.Identificator);

            _repositoryMock.Verify(repo => repo.GetOffice(office1.Identificator), Times.Once);

        }

        [Fact]
        public void RegisterUser_WhenOfficeFound_PlaceNotAvailable()
        {

            ConcurrentQueue<string> _OfficeQueue1 = new ConcurrentQueue<string>();
            IList<AttentionPlace> attentionPlaces = new List<AttentionPlace>();
            AttentionPlace _AttentionPlace1 = new AttentionPlace(1, false);
            AttentionPlace _AttentionPlace2 = new AttentionPlace(2, false);
            attentionPlaces.Add(_AttentionPlace1);
            attentionPlaces.Add(_AttentionPlace2);
            Office office1 = new Office("OFI-1", _OfficeQueue1, attentionPlaces);


            // Simular que la oficina no existe
            _repositoryMock.Setup(repo => repo.GetOffice(office1.Identificator))
                                 .Returns((Office)office1);

            Debug.WriteLine("Hola desde el test");

            _officeService.RegisterUser("userPepe", office1.Identificator);

            _repositoryMock.Verify(repo => repo.GetOffice(office1.Identificator), Times.Once);

        }

    }
}
