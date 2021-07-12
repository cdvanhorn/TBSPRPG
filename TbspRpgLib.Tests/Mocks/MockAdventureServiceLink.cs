using System;
using System.Collections.Generic;
using Moq;
using TbspRpgLib.InterServiceCommunication;
using TbspRpgLib.InterServiceCommunication.RequestModels;

namespace TbspRpgLib.Tests.Mocks
{
    public class MockAdventureServiceLink
    {
        public static IAdventureServiceLink CreateMockAdventureServiceLink(
            IscResponse GetInitialLocation = null,
            IscResponse GetAdventureById = null,
            IscResponse GetRoutesForLocation = null,
            IscResponse FailureGetRoutesForLocation = null)
        {
            var adventureServiceLink = new Mock<IAdventureServiceLink>();
            
            //get initial location
            adventureServiceLink.Setup(asl =>
                asl.GetInitialLocation(It.IsAny<AdventureRequest>(), It.IsAny<Credentials>())
            ).ReturnsAsync((AdventureRequest adventureRequest, Credentials creds) => GetInitialLocation);
            
            adventureServiceLink.Setup(asl =>
                asl.GetAdventureById(It.IsAny<AdventureRequest>(), It.IsAny<Credentials>())
            ).ReturnsAsync((AdventureRequest adventureRequest, Credentials creds) => GetAdventureById);
            
            //get routes
            adventureServiceLink.Setup(asl =>
                asl.GetRoutesForLocation(It.IsAny<AdventureRequest>(), It.IsAny<Credentials>())
            ).ReturnsAsync((AdventureRequest adventureRequest, Credentials creds) =>
            {
                if (adventureRequest.LocationId == Guid.Empty)
                {
                    return FailureGetRoutesForLocation;
                }

                return GetRoutesForLocation;
            });
            return adventureServiceLink.Object;
        }
    }
}