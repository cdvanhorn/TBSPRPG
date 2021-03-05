using Xunit;

using TbspRpgLib.Services;
using TbspRpgLib.Repositories;

using System;

namespace TbspRpgLib.Tests.Services {
    public class AggregateTypeServiceTests {
        private AggregateTypeService _aggregateTypeService;

        public AggregateTypeServiceTests() {
            _aggregateTypeService = new AggregateTypeService(new AggregateTypeRepository());
        }

        [Fact]
        public void GetAggregateTypeName_Valid_GetsName() {
            //arrange
            //act
            string typeName = _aggregateTypeService.GetAggregateTypeName("content_87656335-9169-4314-b1e2-c4568b59ebf9");
            
            //assert
            Assert.Equal(AggregateTypeRepository.CONTENT_AGGREGATE_TYPE, typeName);
        }

        [Fact]
        public void GetAggregateTypeName_Default_GetsName() {
            //arrange
            //act
            string typeName = _aggregateTypeService.GetAggregateTypeName("87656335-9169-4314-b1e2-c4568b59ebf9");
            
            //assert
            Assert.Equal(AggregateTypeRepository.DEFAULT_AGGREGATE_TYPE, typeName);
        }

        [Fact]
        public void GetAggregateTypeName_InValid_Exception() {
            //arrange
            //act
            //assert
            var exception = Assert.Throws<ArgumentException>(() => 
                _aggregateTypeService.GetAggregateTypeName("conten_87656335-9169-4314-b1e2-c4568b59ebf9"));
        }
    }
}