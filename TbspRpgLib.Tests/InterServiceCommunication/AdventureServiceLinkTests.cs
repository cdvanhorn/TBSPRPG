using Xunit;

namespace TbspRpgLib.Tests.InterServiceCommunication
{
    public class AdventureServiceLinkTests
    {
        public AdventureServiceLinkTests()
        {
            //moq service communication the CreateRestClient method to return our moq version of RestClient
            //may need to break RestClient creation in to it's own Class to make it easier to test
            //Service communication isn't really testable as it is.
        }

        #region GetAdventures

        // [Fact]
        // public async void GetAdventures_
        //verify token is found if not created
        //verify token added to header
        //verify if parameters are added
        //verify that it is trying to contact the correct endpoint
        //verify that the service is found and created
        
        //create a token manager class
        //test token creation for a given user id
        //this will be injected in to the service communication object
        
        //create a restsharp manager class
        //gets a client based on a service name
        //generate a request method so I can pass in a moq rest request and check generated correctly

        #endregion
    }
}